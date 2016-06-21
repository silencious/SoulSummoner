using UnityEngine;
using System.Collections;


public interface IEnemyState
{
	void UpdateState ();
	void OnTriggerEnter (Collider other);
	void ToPatrolState ();
	void ToAlertState ();
	void ToChaseState ();
}

public class PatrolState : IEnemyState
{
	private readonly MobBehaviour enemy;
	private int nextWayPoint=0;

	public PatrolState (MobBehaviour statePatternEnemy)
	{
		enemy = statePatternEnemy;
	}

	public void UpdateState ()
	{
		Debug.Log ("Patrol Update");
		Look ();
		Patrol ();
	}

	public void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.CompareTag ("Player"))
			ToAlertState ();
	}

	public void ToPatrolState ()
	{
	}

	public void ToAlertState ()
	{
		Debug.Log ("Patrol -> Alert");
		enemy.currentState = enemy.alertState;
	}

	public void ToChaseState ()
	{
		Debug.Log ("Patrol -> Chase");
		enemy.currentState = enemy.chaseState;
	}

	private void Look ()
	{
		RaycastHit hit;
		if (Physics.Raycast (enemy.transform.position, enemy.transform.forward, out hit, enemy.sightRange) && hit.collider.CompareTag ("Player")) {
			enemy.chaseTarget = hit.transform;
			ToChaseState ();
		}
	}

	void Patrol ()
	{
		enemy.MoveTowards(enemy.patrolPoints [nextWayPoint], Time.deltaTime);
		if(enemy.waypoints == null || enemy.waypoints.Count==0){
			nextWayPoint = (nextWayPoint + 1) % enemy.patrolPoints.Length;			
		}
	}
}

public class AlertState : IEnemyState
{
	private readonly MobBehaviour enemy;
	private float searchTimer;

	public AlertState (MobBehaviour statePatternEnemy)
	{
		enemy = statePatternEnemy;
	}

	public void UpdateState ()
	{
		Debug.Log ("Alert Update");
		Look ();
		Search ();
	}

	public void OnTriggerEnter (Collider other)
	{

	}

	public void ToPatrolState ()
	{
		Debug.Log ("Alert -> Patrol");
		enemy.currentState = enemy.patrolState;
		searchTimer = 0f;
	}

	public void ToAlertState ()
	{
		
	}

	public void ToChaseState ()
	{
		Debug.Log ("Alert -> Chase");
		enemy.currentState = enemy.chaseState;
		searchTimer = 0f;
	}

	private void Look ()
	{
		RaycastHit hit;
		if (Physics.Raycast (enemy.transform.position, enemy.transform.forward, out hit, enemy.sightRange) && hit.collider.CompareTag ("Player")) {
			enemy.chaseTarget = hit.transform;
			ToChaseState ();
		}
	}

	private void Search ()
	{
		enemy.waypoints.Clear ();
		enemy.transform.Rotate (0, enemy.searchingTurnSpeed * Time.deltaTime, 0);
		searchTimer += Time.deltaTime;

		if (searchTimer >= enemy.searchingDuration)
			ToPatrolState ();
	}


}

public class ChaseState : IEnemyState
{

	private readonly MobBehaviour enemy;


	public ChaseState (MobBehaviour statePatternEnemy)
	{
		enemy = statePatternEnemy;
	}

	public void UpdateState ()
	{
		Debug.Log ("Chase Update");
		Look ();
		Chase ();
	}

	public void OnTriggerEnter (Collider other)
	{

	}

	public void ToPatrolState ()
	{

	}

	public void ToAlertState ()
	{
		Debug.Log ("Chase -> Alert");
		enemy.currentState = enemy.alertState;
	}

	public void ToChaseState ()
	{

	}

	private void Look ()
	{
		RaycastHit hit;
		Vector3 enemyToTarget = (enemy.chaseTarget.position + enemy.offset) - enemy.transform.position;
		if (Physics.Raycast (enemy.transform.position, enemyToTarget, out hit, enemy.sightRange) && hit.collider.CompareTag ("Player")) {
			enemy.chaseTarget = hit.transform;

		} else {
			ToAlertState ();
		}

	}

	private void Chase ()
	{
		enemy.RouteTo(enemy.chaseTarget.position);
	}
}
