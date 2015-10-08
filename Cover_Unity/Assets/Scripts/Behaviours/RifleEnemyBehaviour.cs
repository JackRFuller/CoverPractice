using UnityEngine;
using System.Collections;

public class RifleEnemyBehaviour : BaseEnemyClass {

	// Use this for initialization
	void Start () {

		navMeshAgent.speed = walkSpeed;
	
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

		if(_distance < 1)
		{
			waypointID++;
			if(waypointID == patrollingWaypoints.Length)
			{
				waypointID = 0;
			}
		}

	}
}
