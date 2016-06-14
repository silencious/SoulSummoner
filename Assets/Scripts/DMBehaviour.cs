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

		//routeMap = new RouteMap(routeDir+stageName);
		routeMap = new RouteMap (routeDir + "test");
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
		
		// choose from spawns
		// Note: mob should spawn near PC
		if (stage.spawnPoints.Count == 0)
			return;
		var pos = stage.spawnPoints [Random.Range (0, stage.spawnPoints.Count)];

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
		reserve -= mob.elements;
		mobs.Add (gameObject);
	}

	void RouteTo(GameObject gameObject, Vector3 pos){
		
	}

	void Test(){
		if(Input.GetMouseButtonDown(0)){
			var l = routeMap.Path (new Position (0, 0), new Position (22, 33));
			Debug.Log (l.Count);
			/*
			int i = 0;
			foreach(var p in l){
				i++;
				Debug.Log ("x="+Mathf.FloorToInt(p.x)+",y="+(49-Mathf.FloorToInt(p.z))+"i="+i);
			}*/
		}
	}
}
