using UnityEngine;
using System.Collections;

public class MobBehaviour : SoulBehaviour {

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
	}

	protected override void OnCollisionEnter(Collision other){
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
