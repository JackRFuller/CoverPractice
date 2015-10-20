using UnityEngine;
using System.Collections;


[RequireComponent (typeof(NavMeshAgent))]
public class EnemyCombatBehaviour : MonoBehaviour {

	[Header("Combat Manager")]
	[SerializeField] private CombatZoneBehaviour CZB_Script;
	[SerializeField] private EnemyUIManager EUM_Script;

    [Header("Animation Manager")]
    [SerializeField] private EnemyAnimationManager EAM_Script;
    [SerializeField] private EnemyGunAnimation EGS_Script;
	[SerializeField] private Animator enemyAnimator;

    

	[Header("Status")]
	public bool inCombat = false;
    [SerializeField] private float health;
	private bool isDead = false;

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

    [Header("Crouching/Standing")]    
    [SerializeField] private float colliderStandingHeight;
    [SerializeField] private float colliderCrouchingHeight;
    [SerializeField] private Vector3 colliderCrouchingOffset;
    [SerializeField] private bool isCrouching = false;
    private CapsuleCollider enemyCollider;

    [Header("Shooting")]	
    [SerializeField] private bool hasLocatedTarget = false;
	public Transform shootingTarget;
    [SerializeField] private float shotRange; //-- Determines how far the bullet can travel
    [SerializeField] private float cooldownTime; //-- Determines the time between shots
    [SerializeField] private float burstRate; //-- The amount of shots fire with each click
    [SerializeField] private float smgRecoilRate; //-- Determines the amount of recoil between each individual shot when firing from the hip
    [SerializeField] private float sniperRecoilRate; //-- Determines the amount of recoil between each individual shot when aiming
    private float timeStamp;   
    [SerializeField] private float xAccuracy; //-- Determines how close to the centre of the screen on the X Axis the shot is
    [SerializeField] private float yAccuracy; //-- Determines how close to the centre of the screen on the Y Axis the shot is
    private float startingYAccuracy;
    [SerializeField] private float recoilCooldownTime; //-- Determines the amount of time between individual shots - relates more for SMG mode   

    [Header("Movement")]
	[SerializeField] private NavMeshAgent navMeshAgent;
	[SerializeField] private float runningSpeed;
    [SerializeField] private float minimumDistanceFromTarget;


	// Use this for initialization
	void Start () {

		InitialValues();
	
	}

	void InitialValues()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		shootingTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        //ColliderValues
        enemyCollider = GetComponent<CapsuleCollider>();
        colliderStandingHeight = enemyCollider.height;

        //Animation
        EAM_Script = GetComponent<EnemyAnimationManager>();
	}
	
	// Update is called once per frame
	void Update () {

		if(!isDead)
		{
			if(inCombat && !hasLocatedTarget)
			{
				EngageInCombat();
			}
			
			if (hasLocatedTarget)
			{
				LocateTarget();
			}
		}
		
		
		
	}
	
	public void EngageInCombat()
	{
		switch(EnemySettings.Wave)
		{
        
        //Wave 1 Enemies
		case(EnemyType.SoldierWave.Wave1):

			if(EnemySettings.Action == EnemyType.CoreAction.HideInCover || EnemySettings.WeaponType == EnemyType.Weapon.Sniper)
			{
				if(!isInCover)
				{
					MoveInToCover();
				}
				if(isCrouching)
				{
                        IdentifiedTarget();
                }
			}
            if(EnemySettings.Action == EnemyType.CoreAction.StandAndShoot)
                {
                    StandUp();
                    IdentifiedTarget();
                }
            if(EnemySettings.Action == EnemyType.CoreAction.RunAndGun)
                {
                    HuntTarget();
                }

			break;

        ///Wave 2 Enemies
		case(EnemyType.SoldierWave.Wave2):

			if(!isInCover)
			{
				MoveInToCover();
			}
            if (isCrouching)
            {
                    IdentifiedTarget();
                }
			break;
        }
	}

    void IdentifiedTarget()
    {        
        hasLocatedTarget = true;
    }

	public void MoveInToCover()
	{
        //Animation
        EAM_Script.RunToCover();
        EGS_Script.RunToCover();

        navMeshAgent.SetDestination(allocatedCover.position);

		float _distToCover = Vector3.Distance(allocatedCover.position, transform.position);

		if(_distToCover <= inCoverLimit)
		{
            CrouchDown();
			isInCover = true;
		}
	}

    void LocateTarget()
    {
        Vector3 _targetPos = new Vector3(shootingTarget.position.x, transform.position.y, shootingTarget.position.z);
        transform.LookAt(_targetPos);

        //Start Shooting
    }

    void Shoot()
    {
        StandUp();

    }

    void StandUp()
    {
        EAM_Script.StandingAim();
        EGS_Script.StandingAim();
    }

    void HuntTarget()
    {
        //Animation
        EAM_Script.RunToCover();
        EGS_Script.RunToCover();

        navMeshAgent.SetDestination(shootingTarget.position);

        float _distToCover = Vector3.Distance(shootingTarget.position, transform.position);

        if (_distToCover <= minimumDistanceFromTarget)
        {
            navMeshAgent.speed = 0;
        }
    }

    void CrouchDown()
    {
        //Animation
        EAM_Script.Crouch();
        EGS_Script.Crouching();

        enemyCollider.height = colliderCrouchingHeight;
        enemyCollider.center = colliderCrouchingOffset;

        isCrouching = true;
    }

	void Hit(float _damage)
	{
		health -= _damage;
		EUM_Script.HealthUpdate(health);

		if(health <= 0)
		{
			Dead();
		}

		if(!inCombat)
		{
            //Set To Most Aggressive Soldier if shot first
            EnemySettings.WeaponType = EnemyType.Weapon.SMG;
            EnemySettings.Wave = EnemyType.SoldierWave.Wave1;
            EnemySettings.Action = EnemyType.CoreAction.StandAndShoot;

            EAM_Script.StandingAim();

            CZB_Script.SetSoldiersIntoCombat();
        }

        //If Run & Gun Stop in Tracks & Shoot
        if(EnemySettings.Action == EnemyType.CoreAction.RunAndGun)
        {
            navMeshAgent.speed = 0;
            hasLocatedTarget = true;
        }
	}

	void Dead()
	{
		isDead = true;

		enemyAnimator.applyRootMotion = true;
		navMeshAgent.enabled = false;

		enemyCollider.enabled = false;

		EAM_Script.Dead();
	}

}
