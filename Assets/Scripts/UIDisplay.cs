using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIDisplay : MonoBehaviour {
	public Transform elementChart;
	public Elements pcElements;
	public Elements pcReserve;
	public Transform healthPie;
	public float pcHPCeil;
	public float pcHP;

	// Use this for initialization
	void Start () {
		elementChart = transform.Find ("ElementChart");
		pcElements = Elements.one;
		pcReserve = Elements.zero;
		UpdateElementChart ();

		healthPie = transform.Find ("HealthPie");
		pcHPCeil = 1;
		pcHP = 1;
		UpdateHealthPie ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdatePCElements(Elements elements){
		pcElements = elements;
		UpdateElementChart ();
	}

	public void UpdatePCReserve(Elements reserve){
		pcReserve = reserve;
		UpdateElementChart ();
	}

	void UpdateElementChart(){
		elementChart.Find ("Metal").transform.localScale = Vector3.one * pcReserve.metal / pcElements.metal;
		elementChart.Find ("MetalValue").GetComponent<Text> ().text = Mathf.FloorToInt (pcReserve.metal).ToString();
		elementChart.Find ("Plant").transform.localScale = Vector3.one * pcReserve.plant / pcElements.plant;
		elementChart.Find ("PlantValue").GetComponent<Text> ().text = Mathf.FloorToInt (pcReserve.plant).ToString();
		elementChart.Find ("Earth").transform.localScale = Vector3.one * pcReserve.earth / pcElements.earth;
		elementChart.Find ("EarthValue").GetComponent<Text> ().text = Mathf.FloorToInt (pcReserve.earth).ToString();
		elementChart.Find ("Water").transform.localScale = Vector3.one * pcReserve.water / pcElements.water;
		elementChart.Find ("WaterValue").GetComponent<Text> ().text = Mathf.FloorToInt (pcReserve.water).ToString();
		elementChart.Find ("Fire").transform.localScale = Vector3.one * pcReserve.fire / pcElements.fire;
		elementChart.Find ("FireValue").GetComponent<Text> ().text = Mathf.FloorToInt (pcReserve.fire).ToString();
	}

	public void UpdatePCHPCeil(float hp){
		pcHPCeil = hp;
		UpdateHealthPie ();
	}

	public void UpdatePCHP(float hp){
		pcHP = Mathf.Min(hp, pcHPCeil);
		UpdateHealthPie ();	
	}

	void UpdateHealthPie(){
		healthPie.Find ("HP").transform.localScale = Vector3.one * pcHP / pcHPCeil;
		healthPie.Find ("HPValue").GetComponent<Text> ().text = Mathf.FloorToInt (pcHP).ToString ();
	}
}
