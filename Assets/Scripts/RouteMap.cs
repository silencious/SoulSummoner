using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Position{
	public int x;
	public int y;
	public Position(int x, int y){
		this.x = x;
		this.y = y;
	}
	public Position(float x, float y) :this(Mathf.FloorToInt(x), Mathf.FloorToInt(y)){}
	public Position(Vector2 v) :this(Mathf.FloorToInt(v.x), Mathf.FloorToInt(v.y)){}

	public override bool Equals(object obj){
		var p = obj as Position;
		if (obj == null)
			return false;
		return Equals (p);
	}
	public bool Equals(Position p){
		if (p == null){
			return false;
		}
		return (x == p.x) && (y == p.y);
	}
	public override int GetHashCode(){
		return x ^ y;
	}

	public static Position operator +(Position p1, Position p2){
		return new Position (p1.x + p2.x, p1.y + p2.y);
	}
	public static Position operator -(Position p1, Position p2){
		return new Position (p1.x - p2.x, p1.y - p2.y);
	}

	public int Distance(){
		int ax = Mathf.Abs (x), ay = Mathf.Abs (y);
		if (ax >= ay){
			return 10 * ax + 4 * ay;
		}else{
			return 4 * ax + 10 * ay;
		}
	}

	public static int Distance(Position p1, Position p2){
		return (p2 - p1).Distance ();
	}
}

public class WayPoint{
	public Position pos;
	public int gScore;		// distance from start
	public int hScore;		// distance to goal
	public int fScore;

	public WayPoint(Position pos, int gScore, int hScore){
		this.pos = pos;
		this.gScore = gScore;
		this.hScore = hScore;
		fScore = gScore + hScore;
	}

	public static bool operator <(WayPoint p1, WayPoint p2){
		return p1.fScore < p2.fScore;
	}
	public static bool operator >(WayPoint p1, WayPoint p2){
		return p1.fScore > p2.fScore;
	}
	public static bool operator <=(WayPoint p1, WayPoint p2){
		return p1.fScore <= p2.fScore;
	}
	public static bool operator >=(WayPoint p1, WayPoint p2){
		return p1.fScore >= p2.fScore;
	}
}

public class Heap<T>:List<T> where T:WayPoint{
	void Swap(int i, int j){
		T t = this[i];
		this[i] = this[j];
		this[j] = t;
	}

	void SiftUp(int i){
		while(i>0){
			int p = (i - 1) / 2;
			if(this[i]<this[p]){
				Swap (i, p);
				i = p;
			}else{
				break;
			}
		}
	}

	void SiftDown(int i){
		int n = Count, c = 2 * i + 1;
		while(c<n){
			if(c!=(n-1) && this[c+1]<this[c]){
				c++;
			}
			if(this[i]<this[c]){
				break;
			}
			Swap (i, c);
			i = c;
			c = 2 * i + 1;
		}
	}

	public T First(){
		return this[0];
	}

	public void Enqueue(T v){
		Add (v);
		SiftUp (Count - 1);
	}

	public T Dequeue(){
		var ret = First();
		Swap (0, Count - 1);
		RemoveAt (Count - 1);
		SiftDown (0);
		return ret;
	}
}

public class RouteMap {
	static RouteMap instance;
	static Dictionary<string, Texture2D> maps = new Dictionary<string, Texture2D>();
	static string currentMapPath = "RouteMaps/Stage01.png";
	static Texture2D currentMap = Resources.Load(currentMapPath) as Texture2D;
	static float scale = 1.0f;	// size of a tile
	static int width;
	static int height;

	private RouteMap(){
		
	}

	public static RouteMap GetInstance(){
		if(instance==null){
			instance = new RouteMap ();
		}
		return instance;
	}

	public static void SetCurrentMap(string path){
		if(!maps.ContainsKey(path)){
			maps.Add (path, Resources.Load (path) as Texture2D);
		}
		currentMapPath = path;
		currentMap = maps [path];
		width = currentMap.width;
		height = currentMap.height;
	}

	static Texture2D GetMap (string mapPath){
		if(!maps.ContainsKey(mapPath)){
			maps.Add (mapPath, Resources.Load (mapPath) as Texture2D);
		}
		return maps [mapPath];
	}

	// return the center coordinate in world for a given position on map
	public static Vector3 Map2World(Position v){
		return new Vector3 (v.x + 0.5f, 0, v.y + 0.5f) * scale;
	}
	// return a position on map for a given world coordinate
	public static Position World2Map(Vector3 v){
		v /= scale;
		return new Position (v.x, v.z);
	}
	static bool Passable(int x, int y){
		return currentMap.GetPixel (x, y).Equals (Color.white);
	}
	static bool Passable(Position p){
		return Passable (p.x, p.y);
	}
	static public bool Passable(Vector3 pos){
		return Passable (World2Map (pos));
	}

	static bool InRange(Vector3 world, Position map){
		return World2Map (world).Equals(map);
	}

	static void AddNewPos(List<Position> list, Position pos, HashSet<Position> closeSet){
		if(Passable(pos) && !closeSet.Contains(pos)){
			list.Add (pos);
		}
	}

	static List<Position> Neighbors(Position pos, HashSet<Position> closeSet){
		var ret = new List<Position> ();
		int x = pos.x, y = pos.y;
		if(x>0){
			AddNewPos (ret, new Position (x - 1, y), closeSet);
			if(y>0){
				AddNewPos (ret, new Position (x - 1, y - 1), closeSet);
			}
			if(y<currentMap.height-1){
				AddNewPos (ret, new Position (x - 1, y + 1), closeSet);
			}
		}
		if(y>0){
			AddNewPos (ret, new Position (x, y - 1), closeSet);			
		}
		if(y<currentMap.height-1){
			AddNewPos (ret, new Position (x, y + 1), closeSet);			
		}
		if(x<currentMap.width-1){
			AddNewPos (ret, new Position (x + 1, y), closeSet);
			if(y>0){
				AddNewPos (ret, new Position (x + 1, y - 1), closeSet);
			}
			if(y<currentMap.height-1){
				AddNewPos (ret, new Position (x + 1, y + 1), closeSet);
			}
			
		}
		return ret;
	}

	// use A* algoritm to find the path from src to dst, return in world coordinate
	// return empty list if src==dst or no path is found
	public static LinkedList<Vector3> Path(Position src, Position dst){
		var ret = new LinkedList<Vector3> ();
		if(src.Equals(dst) || !Passable(dst)){
			return ret;
		}

		var closeSet = new HashSet<Position> ();
		var openSet = new Heap<WayPoint>();
		openSet.Enqueue (new WayPoint (src, 0, Position.Distance (src, dst)));
		var cameFrom = new Dictionary<Position, Position> ();

		//int maxLoop = 1000, count = 0;
		while(openSet.Count>0){
			var current = openSet.Dequeue ();
			var pos = current.pos;
			closeSet.Add (pos);
			if(current.pos.Equals(dst)){
				// if reach dst, add all waypoints to ret
				while(!pos.Equals(src)){
					ret.AddFirst (Map2World(pos));
					pos = cameFrom [pos];
				}
				break;
			}
			/*
			count++;
			if (count > maxLoop){
				while(!pos.Equals(src)){
					ret.AddFirst (Map2World(pos));
					pos = cameFrom [pos];
				}
				break;				
			}*/
			var neighbors = Neighbors (pos, closeSet);
			//Debug.Log ("Neighbor size:" + neighbors.Count+"loop:"+count);
			foreach(var neighbor in neighbors){
				int tempGScore = current.gScore + Position.Distance (pos, neighbor);
				var p = openSet.Find (x => x.pos.Equals (neighbor));
				if(p==null){
					//Debug.Log ("not find:" + neighbor.x+","+neighbor.y +" loop:"+count);
					p = new WayPoint (neighbor, tempGScore, Position.Distance (neighbor, dst));
					openSet.Enqueue (p);
				}else if(tempGScore >= p.gScore){
					continue;
				}
				cameFrom [neighbor] = pos;
				p.gScore = tempGScore;
				p.fScore = p.gScore + p.hScore;
			}
		}
		return ret;
	}

	public static LinkedList<Vector3> Path(Vector3 src, Vector3 dst){
		return Path (World2Map (src), World2Map (dst));
	}
}
