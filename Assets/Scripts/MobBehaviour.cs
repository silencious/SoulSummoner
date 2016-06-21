using UnityEngine;
using System.Collections;

public class MobBehaviour : LiveBehaviour
{
	public float searchingTurnSpeed = 120f;
	public float searchingDuration = 4f;
	public float sightRange = 20f;
	public Vector3[] patrolPoints;
	public Vector3 offset = new Vector3 (0,.5f,0);
	[HideInInspector] public Transform chaseTarget;
	[HideInInspector] public IEnemyState currentState;
	[HideInInspector] public ChaseState chaseState;
	[HideInInspector] public AlertState alertState;
	[HideInInspector] public PatrolState patrolState;

	private void Awake ()
	{
		chaseState = new ChaseState (this);
		alertState = new AlertState (this);
		patrolState = new PatrolState (this);

		patrolPoints = new Vector3[4];
		Vector3 pos = transform.position;
		pos.y += 5;
		pos.x += 10;
		pos.z += 10;
		patrolPoints [0] = pos;
		pos.x -= 20;
		patrolPoints [1] = pos;
		pos.z -= 20;
		patrolPoints [2] = pos;
		pos.x += 20;
		patrolPoints [3] = pos;
	}

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
		currentState = patrolState;
	}

	protected override void FixedUpdate ()
	{
		//HandleMove ();
		//base.FixedUpdate ();
		currentState.UpdateState ();
	}

	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();
	}

	protected override void OnCollisionEnter (Collision other)
	{
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

	private void OnTriggerEnter (Collider other)
	{
		currentState.OnTriggerEnter (other);
	}

	public void RouteTo(Vector3 pos){
		if(dm!=null){
			dm.RouteTo (this, pos);			
		}
	}
}
