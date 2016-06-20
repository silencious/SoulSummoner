using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class UIDisplay : MonoBehaviour {
	DataAdapter data;
	string mainSceneName = "Main";

	public Transform elementChart;
	public Elements pcElements = Elements.one;
	public Elements pcReserve = Elements.zero;
	public Transform healthPie;
	public float pcHPCeil = 1;
	public float pcHP = 1;

	public GameObject pauseMenu;
	public GameObject pc;

	//public Texture2D cursorTexture;

	// Use this for initialization
	void Start () {
		data = DataAdapter.GetInstance ();
		if(SceneManager.GetActiveScene().name.Equals(mainSceneName)){
			
		}else{
			elementChart = transform.Find ("ElementChart");
			UpdateElementChart ();

			healthPie = transform.Find ("HealthPie");
			UpdateHealthPie ();	

			pauseMenu = transform.Find ("PauseMenu").gameObject;
			pauseMenu.SetActive (false);

			pc = GameObject.Find ("PC");
/*			cursorTexture = Resources.Load ("UI/Cursor") as Texture2D;
			Cursor.SetCursor (cursorTexture, Vector2.zero, CursorMode.Auto);*/			
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.Escape)){
			if(pauseMenu.activeSelf){
				Resume ();
			}else{
				Pause ();
			}
		}
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
		healthPie.Find ("HP").localScale = Vector3.one * pcHP / pcHPCeil;
		healthPie.Find ("HPValue").GetComponent<Text> ().text = Mathf.FloorToInt (pcHP).ToString ();
	}



	public void StartGame(){
		SceneManager.LoadScene ("Stage01");
	}

	public void ContinueGame(){
		var stageName = data.GetSaveInfo().stageName;
		if(stageName==null||stageName.Equals("")){
			StartGame ();
		}
		SceneManager.LoadScene (stageName);
	}

	public void ExitGame(){
		Application.Quit ();
	}

	public void Pause(){
		Debug.Log ("Pause");
		pauseMenu.SetActive (true);
		pc.GetComponent<RigidbodyFirstPersonController> ().enabled = false;
	}

	public void Resume(){
		Debug.Log ("Resume");
		pauseMenu.SetActive (false);
		pc.GetComponent<RigidbodyFirstPersonController> ().enabled = true;
	}

	public void ReturnToMain(){
		SceneManager.LoadScene ("Main");
	}

	public void ClearStage(){
		// show clear stage view

		LoadNextStage ();
	}

	public void LoadNextStage(){
		var currentScene = SceneManager.GetActiveScene ().name;
	}
}
