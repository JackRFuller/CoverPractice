using UnityEngine;
using System.Collections;

public class PlayerShootBehaviour : MonoBehaviour {

    [Header("Managers")]
    [SerializeField] private GunAnimation GA_Script;
    
    [SerializeField] private Transform gunTransform;    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.R))
        {
            CheckAmmo();
        }
	
	}

    void CheckAmmo()
    {
        Reload();
    }

    void Reload()
    {
        GA_Script.Reload();
    }

    void SetGunPosition()
    {
      
        
    }
}
