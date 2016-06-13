using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


public class Soul{
	[XmlAttribute("name")]
	public string soulName;
	[XmlElement("elements")]
	public Elements elements;

	private Soul(){}

	public Soul(string name, Elements elements){
		this.soulName = name;
		this.elements = elements;
	}
}

[XmlRoot("SoulCollection")]
public class SoulContainer{
	[XmlArray("Souls"), XmlArrayItem("Soul")]
	public List<Soul> souls = new List<Soul>();

	public static SoulContainer Load(string path){
		var serializer = new XmlSerializer (typeof(SoulContainer));
		using(var stream = new FileStream (path, FileMode.OpenOrCreate, FileAccess.ReadWrite)){
			return serializer.Deserialize (stream) as SoulContainer;
		}
	}

	public void Save(string path){
		Debug.Log ("Save " + souls.Count + "souls");
		var serializer = new XmlSerializer (typeof(SoulContainer));
		using(var stream = new FileStream (path, FileMode.OpenOrCreate, FileAccess.ReadWrite)){
			serializer.Serialize(stream, this);
		}
	}

	public Dictionary<string, Soul> ToDict (){
		var dict = new Dictionary<string, Soul>();
		foreach(var soul in souls){
			dict.Add (soul.soulName, soul);
		}
		return dict;
	}
}

public class Stage{
	[XmlAttribute("stageName")]
	public string stageName;
	[XmlElement("baseElements")]
	public Elements baseElements;	// base number of Elements added to DM per second
	[XmlElement("factor")]
	public float factor;
	[XmlElement("gapTime")]
	public float gapTime;
	[XmlElement("mobs")]
	public List<string> mobs;
	[XmlElement("spawns")]
	public List<Vector3> spawns;

	private Stage(){}

	public Stage(string name, Elements baseElements, float factor, float gapTime, List<string> mobs, List<Vector3> spawns){
		this.stageName = name;
		this.baseElements = baseElements;
		this.factor = factor;
		this.gapTime = gapTime;
		this.mobs = mobs;
		this.spawns = spawns;
	}

	public static Stage empty = new Stage ("", Elements.one, 1, 1, new List<string> (), new List<Vector3> ());
}

[XmlRoot("StageCollection")]
public class StageContainer{
	[XmlArray("Stages"), XmlArrayItem("Stage")]
	public List<Stage> stages = new List<Stage>();


	public static StageContainer Load(string path){
		var serializer = new XmlSerializer (typeof(StageContainer));
		using(var stream = new FileStream (path, FileMode.OpenOrCreate, FileAccess.ReadWrite)){
			return serializer.Deserialize (stream) as StageContainer;
		}
	}

	public void Save(string path){
		Debug.Log ("Save " + stages.Count + "stages");
		var serializer = new XmlSerializer (typeof(StageContainer));
		using(var stream = new FileStream (path, FileMode.OpenOrCreate, FileAccess.ReadWrite)){
			serializer.Serialize(stream, this);
		}
	}

	public Dictionary<string, Stage> ToDict (){
		var dict = new Dictionary<string, Stage>();
		foreach(var stage in stages){
			dict.Add (stage.stageName, stage);
		}
		return dict;
	}
}

public class DataAdapter {
	static DataAdapter instance;
	const string soulPath = "Assets/Data/SoulElements.xml";
	const string stagePath = "Assets/Data/Stages.xml";
	Dictionary<string, Soul> soulDict;	// dictionary takes soul name and return element data
	Dictionary<string, Stage> stageDict;

	private DataAdapter(){
		/*var container = new SoulContainer ();
		container.souls.Add (new Soul ("slime", Elements.one));
		container.Save (soulPath);*/
		// init eDic
		var soulContainer = SoulContainer.Load (soulPath);
		soulDict = soulContainer.ToDict ();
		var stageContainer = StageContainer.Load (stagePath);
		stageDict = stageContainer.ToDict ();
	}

	public static DataAdapter GetInstance(){
		if(instance == null){
			instance = new DataAdapter();
		}
		return instance;
	}

	public Elements GetElementsByName(string name){
		if (name == null||name.Equals("")){
			Debug.Log ("Get Elements with null/empty name");
			return Elements.zero;
		}
		Soul soul;
		if(!soulDict.TryGetValue(name, out soul)){
			Debug.Log ("Failed to find soul with name:" + name);
			return Elements.zero;
		}
		return soul.elements;
	}

	public List<Soul> Names2Souls(List<string> names){
		List<Soul> ret = new List<Soul> ();
		foreach(var name in names){
			Soul soul;
			if(soulDict.TryGetValue(name, out soul)){
				ret.Add (soul);
			}
		}
		return ret;
	}

	public Stage GetStageByName(string name){
		if (name == null||name.Equals("")){
			Debug.Log ("Get Stage with null/empty name");
			return Stage.empty;
		}
		Stage ret;
		if(!stageDict.TryGetValue(name, out ret)){
			Debug.Log ("Failed to find stage with name:" + name);
			return Stage.empty;
		}
		return ret;		
	}


}
