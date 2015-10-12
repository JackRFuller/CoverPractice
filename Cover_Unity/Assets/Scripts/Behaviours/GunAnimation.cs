using UnityEngine;
using System.Collections;

public class GunAnimation : MonoBehaviour {

    [SerializeField] private Animator gunAnimator;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Reload()
    {
        gunAnimator.SetBool("Reload", true);
    }
}
