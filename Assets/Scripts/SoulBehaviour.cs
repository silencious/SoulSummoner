using UnityEngine;
using System.Collections;

public class SoulBehaviour : MonoBehaviour {
	public string soulName;		// name of the soul, should be set when the soul is instantiated
	public Elements elements;

	protected DataAdapter data;
	public DMBehaviour dm;

	// Use this for initialization
	protected virtual void Start () {
		// init parameters
		data = DataAdapter.GetInstance ();
		elements = data.GetElementsByName (soulName);
		Debug.Log (soulName+": "+elements);
	}

	// Update is called once per frame
	protected virtual void Update () {
		
	}

	protected virtual void OnCollisionEnter(Collision other){
		//Debug.Log (soulName+" collides with "+other.gameObject.name);
		// do nothing
	}

}
