using UnityEngine;
using UnityEditor.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class DMBehaviour : MonoBehaviour {
	string stageName;
	Elements reserve;			// Elements DM has accumulated

	DataAdapter data;
	Stage stage;
	float remTime;

	const string pcName = "PC";
	GameObject pc;
	Transform pcTransform;
	List<GameObject> mobs = new List<GameObject>();
	List<GameObject> lives = new List<GameObject>();

	const string routeDir = "RouteMaps/";
	RouteMap routeMap;

	// Use this for initialization
	void Start () {
		stageName = EditorSceneManager.GetActiveScene ().name;
		Debug.Log (stageName + " DM starts");
		reserve = Elements.zero;

		data = DataAdapter.GetInstance ();
		stage = data.GetStageByName (stageName);

		remTime = 0;
		pc = GameObject.Find (pcName);
		pcTransform = pc.transform;

		//routeMap = new RouteMap(routeDir+stageName);
		routeMap = new RouteMap (routeDir + stageName);
	}

	void Update () {
		remTime += Time.deltaTime;
		while (remTime >= stage.gapTime) {
			remTime -= stage.gapTime;
			var acc = stage.baseElements * stage.gapTime * TimeFactor(stage.factor) * Random.Range (0.5f, 1.5f);
			reserve += acc;
			SpawnMobs ();
		}

		//Test ();
	}

	float TimeFactor(float f){
		if (f <= 1.0f)
			return 1.0f;
		var t = Time.time;
		return Mathf.Pow(f, t/200);
	}

	// Note: for now, we spawn no more than 1 mob each time
	void SpawnMobs(){
		// choose from mob list
		if (stage.mobs.Count == 0)
			return;
		var mob = stage.mobs[Random.Range(0,stage.mobs.Count)];
		var e = data.GetElementsByName (mob);
		if (!reserve.over (e))
			return;			// not enough for the mob, pass
		
		// choose from spawn points, spawn near PC
		if (stage.spawnPoints.Count == 0)
			return;
		var pos = stage.spawnPoints [0];
		var dpos = (pcTransform.position - pos).magnitude;
		foreach(var p in stage.spawnPoints){
			var dp = (pcTransform.position - p).magnitude;
			if(dp<dpos){
				pos = p;
				dpos = dp;
			}
		}

		// spawn mob
		Spawn (mob, pos, ref reserve);
	}

	// spawn a mob, decrease reserve accordingly
	private void Spawn(string soulName, Vector3 pos, ref Elements reserve){
		var obj = Resources.Load ("Prefabs/Mob/" + soulName, typeof(GameObject));
		if (obj == null){
			Debug.Log ("Mob '" + soulName + "' not found");
			return;
		}
		var gameObject = Instantiate (obj, pos, Quaternion.identity) as GameObject;
		var mob = gameObject.GetComponent<MobBehaviour> ();
		if(mob!=null){
			mob.soulName = soulName;
			mob.dm = this;
			reserve -= data.GetElementsByName (soulName);
			mobs.Add (gameObject);
			ReRoute (mob);
			Debug.Log ("Spawn mob:" + soulName);
			return;
		}

		Debug.Log (soulName + "' is not attached with script");
	}

	// summon a soul
	public void Summon(string soulName, Vector3 pos, ref Elements reserve){
		var obj = Resources.Load ("Prefabs/Summon/" + soulName, typeof(GameObject));
		if (obj == null){
			Debug.Log ("Soul '" + soulName + "' not found");
			return;
		}
		var gameObject = Instantiate (obj, pos, Quaternion.identity) as GameObject;
		var live = gameObject.GetComponent<LiveBehaviour> ();
		if(live!=null){
			live.soulName = soulName;
			live.dm = this;
			reserve -= data.GetElementsByName (soulName);
			lives.Add (gameObject);
			ReRoute (live);
			Debug.Log ("Summon soul:" + soulName);
			return;
		}
	}

	public Vector3 PickMob (Vector3 livePos){
		if(mobs.Count==0){
			return pc.transform.position;
		}else{
			return mobs[Random.Range(0, mobs.Count)].transform.position;
		}
	}

	public void RouteTo(LiveBehaviour live, Vector3 pos){
		//Debug.Log ("Route from " + live.transform.position + " to " + pos);
		var path = routeMap.Path (live.transform.position, pos);
		if(path.Count==0){
			path.AddLast (pos);
		}else{
			path.Last.Value = pos;
		}
		live.waypoints = path;
	}

	public void ReRoute(LiveBehaviour live){
		if(live is MobBehaviour){
			RouteTo (live, pcTransform.position);
		}else{
			RouteTo (live, PickMob (live.transform.position));
		}
	}

	public void Remove(GameObject obj){
		var live = obj.GetComponent<LiveBehaviour> ();
		if(live is MobBehaviour){
			mobs.Remove (obj);
		}else{
			lives.Remove (obj);
		}
	}

	void Test(){
		if(Input.GetMouseButtonDown(0)){
			var t = System.Diagnostics.Stopwatch.StartNew ();
			var path = routeMap.Path (new Position (0, 0), new Position (220, 220));
			Debug.Log ("step="+path.Count+" time="+t.Elapsed.TotalSeconds);
		}
	}
}
