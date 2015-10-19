using UnityEngine;
using System.Collections;

public class CombatZoneBehaviour : MonoBehaviour {


	[Header("Status")]
	[SerializeField] private bool isActive = false;
	[SerializeField] private bool isWave1Over = false;
	[SerializeField] private bool isWave2Over = false;

	[Header("Soldiers")]
	[SerializeField] private EnemyCombatBehaviour[] ECB_Scripts;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			if(!isActive)
			{
				SetSoldiersIntoCombat();
			}
		}

	}

	public void SetSoldiersIntoCombat()
	{
		foreach(EnemyCombatBehaviour ECB_Script in ECB_Scripts)
		{
			ECB_Script.inCombat = true;
			isActive = true;
		}
	}
}
