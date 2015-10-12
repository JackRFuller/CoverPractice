using UnityEngine;
using System.Collections;

public class RifleEnemyBehaviour : BaseEnemyClass {

	[Header("Detection Zone")]
	[SerializeField] private BoxCollider detectionZone;
	[SerializeField] private Vector3 detectionZoneSize;


	// Use this for initialization
	void Start () {

		navMeshAgent.speed = walkSpeed;
	
	}

	void InitialValues()
	{
		detectionZone.size = detectionZoneSize;
	}
	
	// Update is called once per frame
	void Update () {

		if(CurrentMode == Mode.Patrol)
		{
			Patrol();
		}


	
	}

	void Patrol()
	{
		navMeshAgent.SetDestination(patrollingWaypoints[waypointID].position);

		float _distance = Vector3.Distance(transform.position, patrollingWaypoints[waypointID].position);

		if(_distance < 2)
		{
			waypointID++;
			if(waypointID == patrollingWaypoints.Length)
			{
				waypointID = 0;
			}
		}

	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			CurrentMode = Mode.Attack;
		}
	}
}
