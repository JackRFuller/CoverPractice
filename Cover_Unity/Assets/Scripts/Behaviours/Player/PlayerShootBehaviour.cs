using UnityEngine;
using System.Collections;

public class PlayerShootBehaviour : MonoBehaviour {

    [Header("Managers")]
	[SerializeField] private PlayerUIManager PUM_Script;
    [SerializeField] private GunAnimation GA_Script;  

    [Header("Shooting")]
    [SerializeField] private float shotRange; //-- Determines how far the bullet can travel
    [SerializeField] private float cooldownTime; //-- Determines the time between shots
	[SerializeField] private float burstRate; //-- The amount of shots fire with each click
	[SerializeField] private float hipRecoilRate; //-- Determines the amount of recoil between each individual shot when firing from the hip
	[SerializeField] private float aimingRecoilRate; //-- Determines the amount of recoil between each individual shot when aiming
    private float timeStamp;
	private bool isShooting;
    [SerializeField] private float xAccuracy; //-- Determines how close to the centre of the screen on the X Axis the shot is
    [SerializeField] private float yAccuracy; //-- Determines how close to the centre of the screen on the Y Axis the shot is
	private float startingYAccuracy;
	[SerializeField] private float recoilCooldownTime; //-- Determines the amount of time between individual shots - relates more for SMG mode    
    [SerializeField] private Camera mainCamera;
	[SerializeField] private Camera gunCamera;
	[SerializeField] private GameObject bulletHoleDecal;

	[Header("Shooting Mode")]
	[SerializeField] private float sniperFireRate;
	[SerializeField] private float smgFireRate;

	[SerializeField] private float sniperCooldownRate;
	[SerializeField] private float smgCooldownRate;

	[SerializeField] private float sniperRecoilRate;
	[SerializeField] private float smgRecoilRate;

	[SerializeField] private float sniperXAccuracy;
	[SerializeField] private float sniperYAccuracy;

	[SerializeField] private float smgXAccuracy;
	[SerializeField] private float smgYAccuracy;

    [Header("Damage Modifiers")]
    [SerializeField] private float smgDamage; //-- Determines how much damage the SMG gun does
    [SerializeField] private float smgDamageDistance; //-- Max Distance that SMG can do 100% damage
    [SerializeField] private float smgDistanceModifier; //-- Modifier applied to lower damage done based on distance
    [SerializeField] private float sniperDamage; //-- Determines how much damage the Sniper does
    [SerializeField] private float sniperDamageDistance; //-- Determines the miniumum distance at which the damage modifier is applied
    [SerializeField] private float sniperDamageModifier; //Damage modifier applied if the sniper rifle is very close
    [SerializeField] private float headshotDamage; //-- Damage modifier applied for a headshot;


    public enum ShootingMode
	{
		SMG,
		Sniper,
	}
	public ShootingMode currentShootingMode;

    [Header("Aiming")]
    [SerializeField] private Transform gunModel;
    public enum aimingMode
    {
        Aiming,
        HipShooting,
    }
    public aimingMode currentAimingMode;
       
    [SerializeField] private float smgZoomInFOV;
	[SerializeField] private float sniperZoomInFOV;
    [SerializeField] private float zoomOutFOV;
    [SerializeField] private Vector3 aimingPosition;
    [SerializeField] private Vector3 hipPosition;

	[Header("Ammo")]
    [SerializeField] private int currentClipAmount;
    private bool isReloading;
    private bool isLoadingAmmo;	
	[SerializeField] private int maxClipSize;
	[SerializeField] private int currentAmmo;
	[SerializeField] private int maxAmmo;
	[SerializeField] private float reloadCooldownTime;
      

	// Use this for initialization
	void Start () {
        
		startingYAccuracy = yAccuracy;


	}
	
	// Update is called once per frame
	void Update () {

        ShootingControls();

        AimingControls();

        ShootingControls();

        ReloadingControls();     

		MovementControls();

		ChangeShootingMode();
	}

    #region Controls

    void ShootingControls()
    { 
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
    }

	void ChangeShootingMode()
	{
		if(Input.GetKeyUp(KeyCode.Q))
		{
			if(!isReloading && !isShooting)
			{
				switch(currentShootingMode)
				{
				case(ShootingMode.SMG):
					ChangeToSniper();
					break;
				case(ShootingMode.Sniper):
					ChangeToSMG();
					break;
				}
			}

		}
	}

    void MovementControls()
    {
        if ((Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0))
        {
            if (!isReloading)
            {

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if (currentAimingMode == aimingMode.HipShooting)
                    {
                        GA_Script.Run();
                    }

                }
                else
                {
                    if (currentAimingMode == aimingMode.HipShooting)
                    {
                        GA_Script.Walk();
                    }
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

    void AimingControls()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (!isReloading)
            {
                ZoomIn();
            }

        }

        if (Input.GetMouseButtonUp(1))
        {
            ZoomOut();
        }
    }

    void ReloadingControls()
    {
        if (Input.GetKey(KeyCode.R))
        {
            CheckAmmo("reload");
        }
    }

    #endregion

	void ChangeToSniper()
	{
		burstRate = sniperFireRate;
		cooldownTime = sniperCooldownRate;


		GA_Script.SwitchToSniper();

		currentShootingMode = ShootingMode.Sniper;
	}

	void ChangeToSMG()
	{
		burstRate = smgFireRate;
		cooldownTime = smgCooldownRate;


		GA_Script.SwitchToSMG();

		currentShootingMode = ShootingMode.SMG;
	}

    void ZoomIn()
    {
        currentAimingMode = aimingMode.Aiming;

		if(currentShootingMode == ShootingMode.SMG)
		{
			gunModel.localPosition = aimingPosition;
			mainCamera.fieldOfView = smgZoomInFOV;
		}
		if(currentShootingMode ==ShootingMode.Sniper)
		{
			gunCamera.enabled = false;
			PUM_Script.ActivateSniper();
			mainCamera.fieldOfView = sniperZoomInFOV;
		}
    }

    void ZoomOut()
    {
		currentAimingMode = aimingMode.HipShooting;

		mainCamera.fieldOfView = zoomOutFOV;

		if(currentShootingMode == ShootingMode.SMG)
		{
			gunModel.localPosition = hipPosition;
		}

		if(currentShootingMode == ShootingMode.Sniper)
		{
			gunCamera.enabled = true;
			PUM_Script.DeactivateSniper();

		}
    }

	void CheckAmmo(string _phase)
	{
		if(_phase == "shoot")
		{
			if(currentClipAmount > 0)
			{
				StartCoroutine(ShootBullet());
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

	IEnumerator ShootBullet()
	{
		if(!isShooting)
		{
			isShooting = true;
			
			GA_Script.Shoot();
			
			for(int i = 0; i < burstRate; i++)
			{
				yield return new WaitForSeconds(recoilCooldownTime);

				if(i == 0)
				{
					yAccuracy = startingYAccuracy;
				}
				
				Vector3 _camerTransform = new Vector3(xAccuracy, yAccuracy, 0);
				Ray ray = mainCamera.ViewportPointToRay(_camerTransform);
				//Debug.DrawRay(ray.origin, ray.direction, Color.red, 1);

				if(currentShootingMode == ShootingMode.SMG)
				{
					if(currentAimingMode == aimingMode.HipShooting)
					{
						yAccuracy += hipRecoilRate;
					}
					
					if(currentAimingMode == aimingMode.Aiming)
					{
						yAccuracy += aimingRecoilRate;
					}
				}

				if(currentShootingMode == ShootingMode.Sniper)
				{
					if(currentAimingMode == aimingMode.Aiming)
					{
						mainCamera.transform.localRotation = Quaternion.Euler(new Vector3(mainCamera.transform.localRotation.x + 20,mainCamera.transform.localRotation.y,mainCamera.transform.localRotation.y));

						ZoomOut();
					}
				}
				
				RaycastHit hit;
				
				if(Physics.Raycast(ray, out hit, shotRange))
				{
					if(hit.collider.tag == "Environment")
					{
						Vector3 _spawnPosition = hit.point +(hit.normal * 0.01F);
						Quaternion _hitRotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
						GameObject decal;
						decal = Instantiate(bulletHoleDecal, _spawnPosition, _hitRotation) as GameObject;
					}

					Vector3 _distTraveled = hit.point - ray.origin; 
					float _distToPC = _distTraveled.z;

					if(hit.collider.tag == "Enemy")
					{
						CalculateDamage(false, hit.collider.gameObject, _distToPC);
					}
					if(hit.collider.tag == "Head")
					{
						CalculateDamage(true, hit.transform.parent.root.gameObject, _distToPC);
					}
				}
				
				currentClipAmount--;
				PUM_Script.Shooting(currentClipAmount);
			}
			
			timeStamp += cooldownTime;
		}
	}

	void CalculateDamage(bool _headShot, GameObject _target, float _DistToPC)
	{
		float _damage = 0;

		if(currentShootingMode == ShootingMode.SMG)
		{
			_damage = smgDamage;

            if(_DistToPC > smgDamageDistance)
            {
                _damage = (_damage / 100) * smgDistanceModifier;
            }
		}
		if(currentShootingMode == ShootingMode.Sniper)
		{
			_damage = sniperDamage;

			if(_DistToPC < sniperDamageDistance)
			{
				_damage *= sniperDamageModifier;
			}
		}

		if(_headShot)
		{
			_damage *= headshotDamage;
		}

		_target.SendMessage("Hit", _damage, SendMessageOptions.DontRequireReceiver);
	}

	void Reload()
	{
		if(!isReloading)
		{
			isReloading = true;

            ZoomOut();

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

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "AmmoCrate")
        {
            PUM_Script.AmmoMode();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "AmmoCrate")
        {
            if(currentAmmo < maxAmmo)
            {
                PUM_Script.ShowInstructions("Press E To Acquire Ammo");

                if (Input.GetKey(KeyCode.E))
                {
                    StartCoroutine(AcquireAmmo());
                    PUM_Script.AcquireAmmo();
                }
                if (Input.GetKeyUp(KeyCode.E))
                {
                    StopCoroutine(AcquireAmmo());
                    PUM_Script.HideInstruction();
                    PUM_Script.ResetDefuseID();
                }
            }
            else
            {
                PUM_Script.HideInstruction();
                StopCoroutine(AcquireAmmo());
                PUM_Script.ResetDefuseID();
            }
        }
    } 
    
    IEnumerator AcquireAmmo()
    {
        if (!isLoadingAmmo)
        {
            isLoadingAmmo = true;
            yield return new WaitForSeconds(1F);
            currentAmmo = maxAmmo;
            PUM_Script.AmmoUpdate(currentAmmo);
            isLoadingAmmo = false;
        }
        
    }   

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "AmmoCrate")
        {
            PUM_Script.HideInstruction();
            StopCoroutine(AcquireAmmo());
            PUM_Script.ResetDefuseID();
        }
    }

}
