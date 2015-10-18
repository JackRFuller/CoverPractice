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
	[SerializeField] private AnimationClip switchGun;

	[Header("SMG Animation Clips")]
	[SerializeField] private AnimationClip smgIdleClip;
	[SerializeField] private AnimationClip smgRunClip;
	[SerializeField] private AnimationClip smgSprintClip;
	[SerializeField] private AnimationClip smgReloadClip;
	[SerializeField] private AnimationClip smgFireClip;
	[SerializeField] private AnimationClip smgZoomInClip;
	[SerializeField] private AnimationClip smgToSniper;

	[Header("Sniper Animation Clips")]
	[SerializeField] private AnimationClip sniperIdleClip;
	[SerializeField] private AnimationClip sniperRunClip;
	[SerializeField] private AnimationClip sniperSprintClip;
	[SerializeField] private AnimationClip sniperReloadClip;
	[SerializeField] private AnimationClip sniperFireClip;
	[SerializeField] private AnimationClip sniperZoomInClip;
	[SerializeField] private AnimationClip sniperToSMG;

	[Header("Misc")]
	[SerializeField] private Animation gunAnimation;
	[SerializeField] private float shootingSpeed;
	private bool isShooting;
	private bool isSwitching;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		//Debug.Log(gunAnimation.isPlaying);
	
	}

	public void SwitchToSniper()
	{
		isSwitching = true;

		gunAnimation.Play(switchGun.name);

		idleClip = sniperIdleClip;
		runClip = sniperRunClip;
		sprintClip = sniperSprintClip;
		reloadClip = sniperReloadClip;
		fireClip = sniperFireClip;
		zoomInClip = sniperZoomInClip;
		switchGun = sniperToSMG;

		StartCoroutine(SwitchWeaponCoolDown());

	}

	public void SwitchToSMG()
	{
		isSwitching = true;

		gunAnimation.Play(switchGun.name);

		idleClip = smgIdleClip;
		runClip = smgRunClip;
		sprintClip = smgSprintClip;
		reloadClip = smgReloadClip;
		fireClip = smgFireClip;
		zoomInClip = smgZoomInClip;
		switchGun = smgToSniper;

		StartCoroutine(SwitchWeaponCoolDown());
	}

	IEnumerator SwitchWeaponCoolDown()
	{
		yield return new WaitForSeconds(1.0F);
		isSwitching = false;
	}

	public void Idle()
	{
		if(!gunAnimation.IsPlaying(fireClip.name) && !gunAnimation.IsPlaying(switchGun.name))
		{
			if(!isSwitching)
			{
				gunAnimation.CrossFade(idleClip.name);
			}

		}

	}

	public void Walk()
	{
		if(!gunAnimation.IsPlaying(fireClip.name) && !gunAnimation.IsPlaying(switchGun.name))
		{
			if(!isSwitching)
			{
				gunAnimation.CrossFade(runClip.name);
			}
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
		Debug.Log("Hit");
	}

    public void Reload()
    {
		gunAnimation.CrossFade(reloadClip.name);
    }


}
