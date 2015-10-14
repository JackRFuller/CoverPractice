using UnityEngine;
using System.Collections;

public class PlayerShootBehaviour : MonoBehaviour {

    [Header("Managers")]
	[SerializeField] private PlayerUIManager PUM_Script;
    [SerializeField] private GunAnimation GA_Script;  

    [Header("Shooting")]
    [SerializeField] private float shotRange;
    [SerializeField] private float cooldownTime;
	[SerializeField] private int burstRate;
    private float timeStamp;
	private bool isShooting;
    [SerializeField] private float xAccuracy;
    [SerializeField] private float yAccuracy;
    [SerializeField] private float damage;
    [SerializeField] private Camera mainCamera;

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

        ShootingControls();

        
	}

    void ShootingControls()
    {
        if (Input.GetKey(KeyCode.R))
        {
            CheckAmmo("reload");
        }

        if (Input.GetMouseButtonDown(0))
        {
            if((timeStamp <= Time.time) && !isReloading && !isShooting)
            {
				isShooting = false;

                CheckAmmo("shoot");
            }            
        }

		if(Input.GetMouseButtonUp(0))
		{
			isShooting = false;
		}

        if ((Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0))
        {
            if (!isReloading)
            {
                if (Input.GetKey(KeyCode.LeftShift))
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
            if (!isReloading)
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
		if(!isShooting)
		{
			isShooting = true;

			GA_Script.Shoot();

			for(int i = 0; i < burstRate; i++)
			{
				Vector3 _camerTransform = new Vector3(xAccuracy, yAccuracy, 0);
				
				Ray ray = mainCamera.ViewportPointToRay(_camerTransform);
				//Debug.DrawRay(ray.origin, ray.direction, Color.red, 1);
				
				RaycastHit hit;
				
				if(Physics.Raycast(ray, out hit, shotRange))
				{
					Debug.Log(hit.collider.name);
				}
				
				currentClipAmount--;
				PUM_Script.Shooting(currentClipAmount);
			}
			
			timeStamp += cooldownTime;
		}


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
