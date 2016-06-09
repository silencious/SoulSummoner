using UnityEngine;
using System.Collections;

public class MobBehaviour : SoulBehaviour {

	// Use this for initialization
	protected virtual void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		base.Update ();
	}

	protected virtual void OnCollisionEnter(Collision other){
		//Debug.Log ("collision enter");
		var obj = other.gameObject;
		var mob = obj.GetComponent<MobBehaviour> ();
		if(mob!=null){
			Fight (mob);
		}
		
		var pc = obj.GetComponent<PCBehaviour> ();
		if(pc!=null){
			Fight (pc);
		}		
	}
}
