using UnityEngine;
using System.Collections;

public class EnemyShootBehaviour : MonoBehaviour {

    [Header("Managers")]
    [SerializeField] private EnemyGunAnimation EGA_Script;

    public enum ShooterType
    {
        SMG,
        Sniper,
    }

    public ShooterType currentShooterType;


	// Use this for initialization
	void Start () {

        InitialValues();
	
	}

    void InitialValues()
    {
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartShooting()
    {
        if(currentShooterType == ShooterType.Sniper)
        {
            ChangeToSniper();
        }
        
    }

    void ChangeToSniper()
    {
        EGA_Script.ChangeToSniper();
    }
}
