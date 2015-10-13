using UnityEngine;
using System.Collections;

public class PlayerShootBehaviour : MonoBehaviour {

    [Header("Managers")]
	[SerializeField] private PlayerUIManager PUM_Script;
    [SerializeField] private GunAnimation GA_Script;  

	[Header("Ammo")]
	private bool isReloading;
	[SerializeField] private int currentClipAmount;
	[SerializeField] private int maxClipSize;
	[SerializeField] private int currentAmmo;
	[SerializeField] private int maxAmmo;
	[SerializeField] private float reloadCooldownTime;
      

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.R))
        {
            CheckAmmo("reload");
        }

		if(Input.GetMouseButton(0))
		{
			CheckAmmo("shoot");
		}	

		if((Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0))
		{
			if(!isReloading)
			{
				if(Input.GetKey(KeyCode.LeftShift))
				{
					GA_Script.Run();
				}
				else
				{
					GA_Script.Walk();
				}

			}
		}
		else
		{
			if(!isReloading)
			{
				GA_Script.Idle();
			}
		}
	}

	void CheckAmmo(string _phase)
	{
		if(_phase == "shoot")
		{
			if(currentClipAmount > 0)
			{
				Shoot();
			}
			else if(currentAmmo > 0)
			{
				Reload();
			}
		}
		if(_phase == "reload")
		{
			if(currentClipAmount < maxClipSize && currentAmmo > 0)
			{
				Reload();
			}
		}
	}

	void Shoot()
	{
		GA_Script.Shoot();

		currentClipAmount--;
		PUM_Script.Shooting(currentClipAmount);
	}   

    void Reload()
    {
		if(!isReloading)
		{
			isReloading = true;

			StartCoroutine(ReloadCooldown());

			for(int i = 0; i < maxClipSize; i++)
			{
				if(currentAmmo > 0 && currentClipAmount < maxClipSize)
				{
					currentClipAmount++;
					currentAmmo--;
					PUM_Script.Reload(currentClipAmount, currentAmmo);
				}
				else
				{
					break;
				}
			}
		}
    }

	IEnumerator ReloadCooldown()
	{
		GA_Script.Reload();

		yield return new WaitForSeconds(reloadCooldownTime);
		isReloading = false;

		GA_Script.Idle();
	}

}
