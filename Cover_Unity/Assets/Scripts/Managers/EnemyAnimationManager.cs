using UnityEngine;
using System.Collections;

public class EnemyAnimationManager : MonoBehaviour {

    [Header("Controllers")]
    [SerializeField] private Animator enemyAnimation;

    [Header("Misc")]
    [SerializeField] private Transform enemyMesh;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Patrol()
    {
        enemyMesh.transform.localPosition = new Vector3(0, -1, 0);
        enemyAnimation.SetBool("Patrolling", true);        
    }

    public void StandingAim()
    {
        enemyAnimation.SetBool("StandingAim", true);
    }

    public void RunToCover()
    {
        enemyAnimation.SetBool("RunToCover", true);
    }

    public void Crouch()
    {
        enemyAnimation.SetBool("CrouchAndAim", true);
        enemyAnimation.SetBool("RunToCover", false);
    }

    public void Dead()
    {
        enemyAnimation.SetBool("Dead", true);
        enemyAnimation.SetBool("Patrolling", false);
    }
}
