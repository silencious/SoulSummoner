using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIDisplay : MonoBehaviour {
	public Transform elementChart;
	public Elements pcElements = Elements.one;
	public Elements pcReserve = Elements.zero;
	public Transform healthPie;
	public float pcHPCeil = 1;
	public float pcHP = 1;

	public Texture2D cursorTexture;

	// Use this for initialization
	void Start () {
		elementChart = transform.Find ("ElementChart");
		UpdateElementChart ();

		healthPie = transform.Find ("HealthPie");
		UpdateHealthPie ();	

		cursorTexture = Resources.Load ("UI/Cursor") as Texture2D;
		Cursor.SetCursor (cursorTexture, Vector2.zero, CursorMode.Auto);
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
		if(elementChart==null){
			elementChart = transform.Find ("ElementChart");
		}

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
		if(healthPie==null){
			healthPie = transform.Find ("HealthPie");
		}
		healthPie.Find ("HP").transform.localScale = Vector3.one * pcHP / pcHPCeil;
		healthPie.Find ("HPValue").GetComponent<Text> ().text = Mathf.FloorToInt (pcHP).ToString ();
	}
}
