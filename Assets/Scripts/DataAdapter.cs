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
		soulName = name;
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
		Debug.Log ("Save " + souls.Count);
		var serializer = new XmlSerializer (typeof(SoulContainer));
		using(var stream = new FileStream (path, FileMode.OpenOrCreate, FileAccess.ReadWrite)){
			serializer.Serialize(stream, this);
		}
	}

	public Dictionary<string, Elements> ToDict (){
		var dict = new Dictionary<string, Elements>();
		foreach(var soul in souls){
			dict.Add (soul.soulName, soul.elements);
		}
		return dict;
	}
}

public class DataAdapter {
	static DataAdapter instance;
	const string soulPath = "Assets/Data/SoulElements.xml";
	Dictionary<string, Elements> soulDict;	// dictionary takes soul name and return element data

	private DataAdapter(){
		/*var container = new SoulContainer ();
		container.souls.Add (new Soul ("slime", Elements.one));
		container.Save (soulPath);*/
		// init eDic
		var container = SoulContainer.Load (soulPath);
		soulDict = container.ToDict ();
	}

	public static DataAdapter GetInstance(){
		if(instance == null){
			instance = new DataAdapter();
		}
		return instance;
	}

	public Elements GetElementsByName(string name){
		if (name == null||name.Equals(""))
			return Elements.one;
		return soulDict [name];
	}
}
