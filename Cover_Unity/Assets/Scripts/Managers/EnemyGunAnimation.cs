using UnityEngine;
using System.Collections;

public class EnemyGunAnimation : MonoBehaviour {

    [Header("Animation Controllers")]
    [SerializeField] private Animation gunController;
    
    [Header("StandingAim")]
    [SerializeField] private AnimationClip standAndAim;
    [SerializeField] private Vector3 standingAimPos;
    [SerializeField] private Vector3 standingAimRot;

    [Header("Crouching")]
    [SerializeField] private AnimationClip crouchAndAim;
    [SerializeField] private Vector3 crouchingPos;
    [SerializeField] private Vector3 crouchingRot;

    [Header("RunToCover")]
    [SerializeField] private AnimationClip runToCover;
    [SerializeField] private Vector3 runToCoverPos;
    [SerializeField] private Vector3 runToCoverRot;

    [Header("ChangeToSniper")]
    [SerializeField] private AnimationClip changeToSniper;
    [SerializeField] private Vector3 changeToSniperPos;
    [SerializeField] private Vector3 changeToSniperRot;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StandingAim()
    {
        gunController.Play(standAndAim.name);
        transform.localPosition = standingAimPos;
        transform.localRotation = Quaternion.Euler(standingAimRot);

    
        
    }
    public void Crouching()
    {
        gunController.Play(crouchAndAim.name);
        transform.localPosition = crouchingPos;
        transform.localRotation = Quaternion.Euler(crouchingRot);

       
    }
    public void RunToCover()
    {
        gunController.Play(runToCover.name);
        transform.localPosition = runToCoverPos;
        transform.localRotation = Quaternion.Euler(runToCoverRot);
    }

    public void ChangeToSniper()
    {
        gunController.Play(changeToSniper.name);
        transform.localPosition = changeToSniperPos;
        transform.localRotation = Quaternion.Euler(changeToSniperRot);
    }

    public void Dead()
    {
        gunController.Stop();
        gunController.gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }
}
