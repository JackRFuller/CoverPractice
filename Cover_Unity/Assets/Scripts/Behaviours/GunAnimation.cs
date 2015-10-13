using UnityEngine;
using System.Collections;

public class GunAnimation : MonoBehaviour {

	[SerializeField] private Animation gunAnimation;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


	
	}

	public void Idle()
	{
		gunAnimation.Play("Idle");
	}

	public void Walk()
	{
		gunAnimation.CrossFade("Run");
	}

	public void Shoot()
	{
		gunAnimation.Play("Fire1shot");
	}

	public void Run()
	{
		gunAnimation.CrossFade("Run2");
	}

    public void Reload()
    {
		gunAnimation.CrossFade("SMG_reload");
    }


}
