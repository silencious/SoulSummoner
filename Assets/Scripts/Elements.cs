﻿using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public class Elements{
	public enum ELEMENTS
	{
		METAL=0, PLANT=1, EARTH=2, WATER=3, FIRE=4
	};

	[XmlElement("metal")]
	public float metal{ get; set;}
	[XmlElement("plant")]
	public float plant{ get; set;}
	[XmlElement("earth")]
	public float earth{ get; set;}
	[XmlElement("water")]
	public float water{ get; set;}
	[XmlElement("fire")]
	public float fire{ get; set;}

	public Elements():this(zero){}

	public Elements(float metal, float plant, float earth, float water, float fire){
		this.metal = metal;
		this.plant = plant;
		this.earth = earth;
		this.water = water;
		this.fire = fire;
	}

	public Elements(Elements e){
		this.metal = e.metal;
		this.plant = e.plant;
		this.earth = e.earth;
		this.water = e.water;
		this.fire = e.fire;
	}

	public static Elements one = new Elements (1, 1, 1, 1, 1);
	public static Elements zero = new Elements (0, 0, 0, 0, 0);

	public float sigma(){
		return metal + plant + earth + water + fire;
	}

	public float magnitude(){
		return Mathf.Sqrt (metal * metal + plant * plant + earth * earth + water * water + fire * fire);
	}

	public Elements normalized(){
		Elements e = new Elements(this);
		e.normalize ();
		return e;
	}

	public void normalize(){
		float mag = magnitude ();
		if (mag <= 0)
			return;
		metal /= mag;
		plant /= mag;
		earth /= mag;
		water /= mag;
		fire /= mag;
	}

	public static float factor(Elements element1, Elements element2){
		if(element1==null||element2==null){
			return 1;
		}
		Elements e1 = element1.normalized (), e2 = element2.normalized ();
		float index = e1.metal*(e2.plant-e2.fire)+
			e1.plant*(e2.earth-e2.metal)+
			e1.earth*(e2.water-e2.plant)+
			e1.water*(e2.fire-e2.earth)+
			e1.fire*(e2.metal-e2.water);
		return Mathf.Pow (2, index);
	}

	public float factor(Elements other){
		return factor (this, other);
	}

	public static Elements operator +(Elements e1, Elements e2){
		return new Elements (e1.metal + e2.metal, e1.plant + e2.plant, e1.earth + e2.earth, e1.water + e2.water, e1.fire + e2.fire);
	}

	public static Elements operator -(Elements e1, Elements e2){
		return new Elements (e1.metal - e2.metal, e1.plant - e2.plant, e1.earth - e2.earth, e1.water - e2.water, e1.fire - e2.fire);
	}

	public static Elements operator *(Elements e, float f){
		return new Elements (e.metal * f, e.plant * f, e.earth * f, e.water * f, e.fire * f);
	}

	/* if this Elements' five values are all over e */
	public bool over(Elements e){
		return metal >= e.metal && plant >= e.plant && earth >= e.earth && water >= e.water && fire >= e.fire;
	}

	public bool nonneg(){
		return over (zero);
	}

	public static Elements max(Elements e1, Elements e2){
		return new Elements (Mathf.Max (e1.metal, e2.metal),
			Mathf.Max (e1.plant, e2.plant),
			Mathf.Max (e1.earth, e2.earth),
			Mathf.Max (e1.water, e2.water),
			Mathf.Max (e1.fire, e2.fire));
	}

	public static Elements min(Elements e1, Elements e2){
		return new Elements (Mathf.Min (e1.metal, e2.metal),
			Mathf.Min (e1.plant, e2.plant),
			Mathf.Min (e1.earth, e2.earth),
			Mathf.Min (e1.water, e2.water),
			Mathf.Min (e1.fire, e2.fire));
	}

	public override string ToString(){
		return "{metal=" + metal + 
			", plant=" + plant + 
			", earth=" + earth + 
			", water=" + water + 
			", fire=" + fire + "}";
	}
}
