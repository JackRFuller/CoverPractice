using UnityEngine;
using System.Collections;

public class BaseEnemyClass : MonoBehaviour {

	public enum Mode
	{
		Patrol,
		Attack
	}

	public Mode CurrentMode;

	[Header("Movement Items")]
	public NavMeshAgent navMeshAgent;
	public float walkSpeed;
	public float runSpeed;

	[Header("Targets")]
	public Transform[] patrollingWaypoints;
	public int waypointID = 0;

	[Header("Detecting Player")]
	public float fieldofViewAngle = 110F;
	public bool playerInSight;
	public Vector3 personalLastSighting;
	public Vector3 previousSighting;
	 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
