using UnityEngine;
using System.Collections;

public class TurretBehaviour : MonoBehaviour {

	[Header("Rotation Variables")]
	[SerializeField] private Transform turretHead;
	[SerializeField] private float turningRate;
	[SerializeField] private Vector3[] targetRotations; 
	private Quaternion targetRotation;
	private bool isSearching = true;
	private bool isStopping = false;
	private int rotationID = 0;

	[Header("Shooting")]
	[SerializeField] private Transform raycastPoint;
	[SerializeField] private LineRenderer laserSight;
	[SerializeField] private float returnToSearchTime;
	private Transform target;
	private bool lostTarget;
	[SerializeField] private float cooldownTime;
	private float timeStamp = 0;
	[SerializeField] private float damageDone;


	// Use this for initialization
	void Start () {

		targetRotation = Quaternion.Euler(targetRotations[rotationID]);
	
	}
	
	// Update is called once per frame
	void Update () {

		SearchForTarget();

		if(isSearching)
		{
			RotateTurretHead();
		}	
		else
		{
			SnapToTarget();
		}
	}

	void SnapToTarget()
	{
		turretHead.LookAt(new Vector3(target.position.x, turretHead.localPosition.y, target.position.z ));

		if(timeStamp <= Time.time)
		{
			Shoot();
		}

	}

	void Shoot()
	{
		Ray ray = new Ray(raycastPoint.position, raycastPoint.forward);
		Debug.DrawRay(ray.origin, ray.direction, Color.green);

		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, 100))
		{
			if(hit.collider.tag == "Player")
			{
				//laserSight.SetPosition(1,hit.collider.transform.localPosition);

				hit.collider.gameObject.SendMessage("Hit", damageDone, SendMessageOptions.DontRequireReceiver);
				timeStamp = Time.time + cooldownTime;
			}
		}
	}

	void SearchForTarget()
	{
		Ray ray = new Ray(raycastPoint.position, raycastPoint.forward);
		Debug.DrawRay(ray.origin, ray.direction, Color.green);

		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, 200))
		{
			if(hit.collider.tag == "Player")
			{
				isSearching = false;
				target = hit.collider.GetComponent<Transform>();
				StopCoroutine(StopSearch());
				lostTarget = false;
				
			}
			if(hit.collider.tag != "Player")
			{
				lostTarget = true;
				StartCoroutine(StopSearch());
			}
		}
		else
		{
			lostTarget = true;
			StartCoroutine(StopSearch());
		}
	}

	IEnumerator StopSearch()
	{
		if(lostTarget && !isStopping)
		{
			isStopping = true;
			yield return new WaitForSeconds(returnToSearchTime);
			isSearching = true;
			isStopping = false;
		}

	}

	void RotateTurretHead()
	{
		turretHead.rotation = Quaternion.RotateTowards(turretHead.rotation, targetRotation, turningRate * Time.deltaTime);
		
		if(turretHead.rotation == targetRotation)
		{
			switch(rotationID)
			{
			case(0):
				rotationID = 1;
				break;
			case(1):
				rotationID = 0;
				break;
			}
			targetRotation = Quaternion.Euler(targetRotations[rotationID]);
		}
	}
}
