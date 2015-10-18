using UnityEngine;
using System.Collections;

public class EnemyCombatBehaviour : MonoBehaviour {

    [Header("Managers")]
    [SerializeField] private EnemyAnimationManager EAM_Script;
    [SerializeField] private EnemyUIManager EUM_Script;
    [SerializeField] private EnemyGunAnimation EGA_Script;
    [SerializeField] private EnemyShootBehaviour ESB_Script;

    [Header("Typing")]
    [SerializeField] private string EnemyName;
    public enum EnemyType
    {
        Idle,
        Patrol
    }
    public EnemyType currentEnemyType;
    public enum CombatMode
    {
        Passive,
        Aggressive,
        Searching,
    }
    public CombatMode currentCombatMode;
    public enum AggressiveType
    {
        InPosition,
        FindCover,
    }
    public AggressiveType currentAggroType;

    [Header("Movement")]
    [SerializeField] private float patrollingSpeed;
    [SerializeField] private float runToCoverSpeed;

    [Header("Patrolling Items")]
    [SerializeField] private Transform[] patrolWaypoints;
    private int waypointTarget;

    [Header("Navigation")]
    [SerializeField] private NavMeshAgent navMeshAgent;

    [Header("Cover")]
    [SerializeField] private Transform designatedCover;
    private float originalColliderHeight;
    private Vector3 originalColliderPos;
    [SerializeField] private float coverColliderHeight;
    [SerializeField] private Vector3 coverColliderPos;


    [Header("Life")]
    [SerializeField] private float health;

    [Header("Misc")]
    [SerializeField] private Transform targetPC;
    [SerializeField] private Transform soldierMesh;
    [SerializeField] private CapsuleCollider mainCollider;
    private bool isDead;

    // Use this for initialization
    void Start () {

        IntialValues();
	
	}

    void IntialValues()
    {
        //Managers
        ESB_Script = this.GetComponent<EnemyShootBehaviour>();

        mainCollider.enabled = true;
        originalColliderHeight = mainCollider.height;
        originalColliderPos = mainCollider.center;        

        if(currentEnemyType == EnemyType.Patrol)
        {
            navMeshAgent.speed = patrollingSpeed;
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (!isDead)
        {
            DetermineEnemyType();

            if (currentCombatMode == CombatMode.Aggressive)
            {
                if (currentAggroType == AggressiveType.InPosition)
                {
                    LocateEnemy();
                }
                if (currentAggroType == AggressiveType.FindCover)
                {
                    MoveToCover();
                }

            }
        } 

        
	}

    void LocateEnemy()
    {
        Vector3 _targetPostition = new Vector3(targetPC.position.x, transform.position.y, targetPC.position.z);       
        transform.LookAt(_targetPostition);        
    }

    void MoveToCover()
    {
        navMeshAgent.SetDestination(designatedCover.position);

        float _distance = Vector3.Distance(designatedCover.position, transform.position);

        if(_distance < 0.5)
        {
            currentAggroType = AggressiveType.InPosition;
            EAM_Script.HideBehindCover();

            Crouching();
        }
    }

    void DetermineEnemyType()
    {
        if(currentEnemyType == EnemyType.Patrol)
        {
            if(currentCombatMode == CombatMode.Passive)
            {
                Patrol();
            }
        }
    }

    void Patrol()
    {
        navMeshAgent.SetDestination(patrolWaypoints[waypointTarget].position);
        EAM_Script.Patrol();

        float _distance = Vector3.Distance(patrolWaypoints[waypointTarget].position, transform.position);

        if(_distance < 0.5)
        {
            waypointTarget++;

            if(waypointTarget == patrolWaypoints.Length)
            {
                waypointTarget = 0;
            }
        }
    }

     

    void Hit(float _damage)
    {
        if(currentCombatMode != CombatMode.Aggressive)
        {
            SetAggressive("shot");
        }

        health -= _damage;

        //Update Health Bar
        EUM_Script.HealthUpdate(health);

        if(health <= 0)
        {            
            Dead();
        }
    }

    public void SetAggressive(string _hitType)
    {
        currentCombatMode = CombatMode.Aggressive;       

        if(_hitType == "shot")
        {
            SetStandingAim();
        }
        if(_hitType == "detected")
        {
            currentAggroType = AggressiveType.FindCover;
            navMeshAgent.speed = runToCoverSpeed;

            //--- Run to Cover Animations
            EAM_Script.RunToCover();
            EGA_Script.RunToCover();
        }
    }

    void SetStandingAim()
    {
        navMeshAgent.speed = 0;
        int _random = Random.Range(1, 3);
        if (_random == 1)
        {
            EAM_Script.StandingAim();
            EGA_Script.StandingAim();
        }
        else
        {
            EAM_Script.HideBehindCover();
            
            Crouching();
        }
    }

    void Crouching()
    {
        EGA_Script.Crouching();
        ESB_Script.StartShooting();

        mainCollider.height = coverColliderHeight;
        mainCollider.center = coverColliderPos;
       
    }

    void Standing()
    {
        mainCollider.height = originalColliderHeight;
        mainCollider.center = originalColliderPos;

        ESB_Script.StartShooting();
    }

    void Dead()
    {
        isDead = true;
        navMeshAgent.speed = 0;
        mainCollider.enabled = false;

        EAM_Script.Dead();
        EGA_Script.Dead();
    }
}
