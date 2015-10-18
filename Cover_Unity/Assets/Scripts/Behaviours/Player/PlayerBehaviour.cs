using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour {

	[Header("Managers")]
	[SerializeField] private PlayerUIManager PUM_Script;

	[SerializeField] private float totalHealth;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Hit(float _damageTaken)
	{
		totalHealth -= _damageTaken;
		PUM_Script.HealthUpdate(totalHealth);
	}
}
