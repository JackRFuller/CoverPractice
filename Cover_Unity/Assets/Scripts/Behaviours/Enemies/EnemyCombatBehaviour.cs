using UnityEngine;
using System.Collections;
[RequireComponent (typeof(NavMeshAgent))]
public class EnemyCombatBehaviour : MonoBehaviour {

	[Header("Combat Manager")]
	[SerializeField] private CombatZoneBehaviour CZB_Script;

	[Header("Status")]
	public bool inCombat = false;

	[System.Serializable]
	public class EnemyType
	{
		public enum SoldierWave
		{
			Wave1,
			Wave2,
		}
		public SoldierWave Wave;

		public enum Weapon
		{
			SMG,
			Sniper,
		}

		public Weapon WeaponType;

		public enum CoreAction
		{
			HideInCover,
			StandAndShoot,
			RunAndGun,
		}

		public CoreAction Action;
	}

	public EnemyType EnemySettings;

	[Header("Cover")]
	[SerializeField] private bool isInCover;
	public Transform allocatedCover;
	[SerializeField] private float inCoverLimit;

	[Header("Shooting")]
	[SerializeField] private bool hasLocatedTarget = false;
	public Transform shootingTarget;

	[Header("Movement")]
	[SerializeField] private NavMeshAgent navMeshAgent;
	[SerializeField] private float runningSpeed;


	// Use this for initialization
	void Start () {

		InitialValues();
	
	}

	void InitialValues()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		shootingTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {

		if(inCombat)
		{
			EngageInCombat();
		}
	
	}

	public void EngageInCombat()
	{
		switch(EnemySettings.Wave)
		{

		case(EnemyType.SoldierWave.Wave1):

			if(EnemySettings.Action == EnemyType.CoreAction.HideInCover || EnemySettings.WeaponType == EnemyType.Weapon.Sniper)
			{
				if(!isInCover)
				{
					MoveInToCover();
				}
				else
				{

				}

			}

			break;


		case(EnemyType.SoldierWave.Wave2):

			if(!isInCover)
			{
				MoveInToCover();
			}
			break;
		}
	}

	public void MoveInToCover()
	{
		navMeshAgent.SetDestination(allocatedCover.position);

		float _distToCover = Vector3.Distance(allocatedCover.position, transform.position);

		if(_distToCover <= inCoverLimit)
		{
			isInCover = true;
		}
	}

	void Hit(float _damage)
	{
		if(!inCombat)
		{
			CZB_Script.SetSoldiersIntoCombat();
		}
	}


}
