using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LiveBehaviour : SoulBehaviour {
	public float hp;
	protected float moveSpeed = 1.0f;	// by default
	protected float minRouteDis = 1.0f;
	protected float maxRouteDis = 20.0f;

	public LinkedList<Vector3> waypoints;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		hp = elements.sigma ();
	}

	protected virtual void FixedUpdate(){
		if(waypoints==null || waypoints.Count==0){
			dm.ReRoute (this);
		}
		DoRoute();
	}
	
	// Update is called once per frame
	protected override void Update () {
		if(hp<=0){
			Destroy (gameObject);
		}
	}

	void OnDestroy(){
		Debug.Log (soulName + ": hp <= 0, destroy");
		dm.Remove (gameObject);
	}

	protected void DoRoute(){
		var target = waypoints.First.Value;
		//Debug.Log (target);
		while (CloseEnough (target)) {
			if(waypoints.Count==1){
				waypoints.Clear ();
				return;
			}
			waypoints.RemoveFirst();
			target = waypoints.First.Value;
		}
		MoveTowards (target, Time.fixedDeltaTime);
	}

	public void MoveTowards(Vector3 dst, float time){
		var v = dst - transform.position;
		v.y = 0;
		transform.position += v * moveSpeed * time;
	}

	protected void Fight(LiveBehaviour live){
		float factor = elements.factor (live.elements);
		if(hp*factor<live.hp){
			live.hp -= hp * factor;
			hp = 0;
			Debug.Log (soulName+" vs "+live.soulName+", "+soulName+" died");
		}else{
			hp -= live.hp / factor;
			live.hp = 0;
			Debug.Log (soulName+" vs "+live.soulName+", "+live.soulName+" died");
		}
	}

	protected bool CloseEnough(Vector3 dst){
		var v = dst - transform.position;
		v.y = 0;
		return v.magnitude < minRouteDis;
	}
}
