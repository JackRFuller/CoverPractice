using UnityEngine;
using System.Collections;

public class GunAnimation : MonoBehaviour {

	[Header("Animation Clips")]
	[SerializeField] private AnimationClip idleClip;
	[SerializeField] private AnimationClip runClip;
	[SerializeField] private AnimationClip sprintClip;
	[SerializeField] private AnimationClip reloadClip;
	[SerializeField] private AnimationClip fireClip;
    [SerializeField] private AnimationClip zoomInClip;

	[SerializeField] private Animation gunAnimation;
	[SerializeField] private float shootingSpeed;
	private bool isShooting;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		//Debug.Log(gunAnimation.isPlaying);
	
	}

	public void Idle()
	{
		if(!gunAnimation.IsPlaying(fireClip.name))
		{
			gunAnimation.CrossFade(idleClip.name);
		}

	}

	public void Walk()
	{
		if(!gunAnimation.IsPlaying(fireClip.name))
		{
			gunAnimation.CrossFade(runClip.name);
		}

	}

    public void ZoomIn()
    {
        gunAnimation.Play(zoomInClip.name);        
    }

	public void Shoot()
	{
		if(!gunAnimation.IsPlaying(fireClip.name))
		{
			//gunAnimation["SMG_fireburst"].speed = shootingSpeed;
			gunAnimation.CrossFade(fireClip.name);
			Debug.Log("Play");
		}

	}

	public void Run()
	{
		gunAnimation.CrossFade(sprintClip.name);
	}

    public void Reload()
    {
		gunAnimation.CrossFade(reloadClip.name);
    }


}
