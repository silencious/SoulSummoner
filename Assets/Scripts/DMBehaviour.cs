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

	const string routeDir = "BitMaps/";
	Texture2D routeMap;

	// Use this for initialization
	void Start () {
		stageName = EditorSceneManager.GetActiveScene ().name;
		Debug.Log (stageName + " DM starts");
		reserve = Elements.zero;

		data = DataAdapter.GetInstance ();
		stage = data.GetStageByName (stageName);

		remTime = 0;
		pc = GameObject.Find (pcName);

		routeMap = Resources.Load (routeDir + stageName) as Texture2D;
		Debug.Log (routeMap.format);
	}

	void Update () {
		remTime += Time.deltaTime;
		while (remTime >= stage.gapTime) {
			remTime -= stage.gapTime;
			var acc = stage.baseElements * stage.gapTime * TimeFactor(stage.factor) * Random.Range (0.5f, 1.5f);
			reserve += acc;
			SpawnMobs ();
		}
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
		Texture2D tex = new Texture2D (50, 50, TextureFormat.Alpha8, false);
	}
}
