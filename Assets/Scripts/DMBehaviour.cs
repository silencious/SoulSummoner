using UnityEngine;
using UnityEditor.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class DMBehaviour : MonoBehaviour {
	string stageName;
	Elements container;			// Elements DM has accumulated

	DataAdapter data;
	Stage stage;
	float remTime;

	// Use this for initialization
	void Start () {
		stageName = EditorSceneManager.GetActiveScene ().name;
		Debug.Log (stageName + " DM starts");
		container = Elements.zero;

		data = DataAdapter.GetInstance ();
		stage = data.GetStageByName (stageName);

		remTime = 0;
		Test ();
	}

	void Update () {
		remTime += Time.deltaTime;
		while (remTime >= stage.gapTime) {
			remTime -= stage.gapTime;
			var acc = stage.baseElements * stage.gapTime * TimeFactor(stage.factor) * Random.Range (0.5f, 1.5f);
			container += acc;
			GenerateMobs ();
		}
	}

	float TimeFactor(float f){
		if (f <= 1.0f)
			return 1.0f;
		var t = Time.time;
		return Mathf.Pow(f, t/200);
	}

	// Note: for now, we generate no more than 1 mob each time
	void GenerateMobs(){
		// choose from mob list
		var mob = stage.mobs[Random.Range(0,stage.mobs.Count)];
		var e = data.GetElementsByName (mob);
		if (!container.over (e))
			return;			// not enough for the mob, pass
		
		// choose from spawns
		// Note: mob should spawn near PC
		var spawn = stage.spawns [Random.Range (0, stage.spawns.Count)];

		// generate mob
		GenerateMob(spawn, mob);
	}

	// generate a mob, decrease container accordingly
	void GenerateMob(Vector3 pos, string soulName){
		var obj = Resources.Load ("Prefabs/Mob/" + soulName);
		if (obj == null){
			Debug.Log ("Mob '" + soulName + "' not found");
			return;
		}
		var gameObj = Instantiate (obj, pos, Quaternion.identity) as GameObject;
		var soul = gameObj.GetComponent<MobBehaviour> ();
		if(soul==null){
			Debug.Log ("Mob '" + soulName + "' is not attached with script");
			return;
		}
		soul.soulName = soulName;
		container -= soul.elements;
	}

	void Route(GameObject gameObject, Vector3 pos){
		
	}

	void Test(){
		
	}
}
