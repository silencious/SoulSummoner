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

	const string pcName = "pc";
	GameObject pc;
	Transform pcTransform;
	List<GameObject> mobs = new List<GameObject>();

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

		Test ();
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
		Spawn (pos, mob);
	}

	// spawn a mob, decrease reserve accordingly
	void Spawn(Vector3 pos, string soulName){
		var obj = Resources.Load ("Prefabs/Mob/" + soulName, typeof(GameObject));
		if (obj == null){
			Debug.Log ("Mob '" + soulName + "' not found");
			return;
		}
		var gameObject = Instantiate (obj, pos, Quaternion.identity) as GameObject;
		var mob = gameObject.GetComponent<MobBehaviour> ();
		if(mob==null){
			Debug.Log ("Mob '" + soulName + "' is not attached with script");
			return;
		}
		mob.soulName = soulName;
		mob.dm = this;
		reserve -= mob.elements;
		mobs.Add (gameObject);
		RouteTo (mob, pcTransform.position);
	}

	public void RouteTo(SoulBehaviour soul, Vector3 pos){
		soul.waypoints = routeMap.Path (soul.transform.position, pos);
	}

	public void ReRoute(SoulBehaviour soul){
		var mob = soul as MobBehaviour;
		if(mob!=null){
			RouteTo (mob, pcTransform.position);
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
