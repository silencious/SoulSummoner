using UnityEngine;
using System.Collections;

public class MobBehaviour : LiveBehaviour {

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
	}

	protected override void FixedUpdate(){
		//HandleMove ();
		base.FixedUpdate ();
	}

	protected override void OnCollisionEnter(Collision other){
		//Debug.Log ("collision enter");
		base.OnCollisionEnter (other);
		var obj = other.gameObject;
		var live = obj.GetComponent<LiveBehaviour> ();
		if ((live != null) && !(live is MobBehaviour)) {	// not a mob, fight it
			Fight (live);
			if(live is PCBehaviour){
				(live as PCBehaviour).UpdateHP ();
			}
		}	
	}
}
