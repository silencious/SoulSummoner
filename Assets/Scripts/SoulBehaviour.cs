using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoulBehaviour : MonoBehaviour {
	public string soulName;		// name of the soul, should be set when the soul is instantiated
	public Elements elements;
	public float hp;

	protected DataAdapter data;
	protected float moveSpeed = 0.0f;	// by default cannot move
	protected float minRouteDis = 1.0f;
	protected float maxRouteDis = 20.0f;

	public LinkedList<Vector3> waypoints;

	public DMBehaviour dm;

	// Use this for initialization
	protected virtual void Start () {
		// init parameters
		data = DataAdapter.GetInstance ();
		elements = data.GetElementsByName (soulName);
		Debug.Log (soulName+": "+elements);
		hp = elements.sigma ();
	}

	protected virtual void FixedUpdate(){
		if(waypoints==null || waypoints.Count==0){
			dm.ReRoute (this);
			return;
		}
		var target = waypoints.First.Value;
		while (CloseEnough (target) && waypoints.Count>1) {
			waypoints.RemoveFirst();
			target = waypoints.First.Value;
		}
		MoveTowards (target, Time.fixedDeltaTime);
	}

	// Update is called once per frame
	protected virtual void Update () {
		if(hp<=0){
			Debug.Log (soulName + ":hp <= 0, destroy");
			Destroy (gameObject);
		}
	}

	protected virtual void OnCollisionEnter(Collision other){
		//Debug.Log ("collision enter");
		// do nothing
	}

	protected virtual void Fight(SoulBehaviour soul){
		float factor = elements.factor (soul.elements);
		if(hp*factor<soul.hp){
			soul.hp -= hp * factor;
			hp = 0;
		}else{
			hp -= soul.hp / factor;
			soul.hp = 0;
		}
	}

	public virtual void MoveTowards(Vector3 dst, float time){
		transform.position += (dst - transform.position) * moveSpeed * time;
	}

	bool CloseEnough(Vector3 v){
		return (v - transform.position).magnitude < minRouteDis;
	}
}
