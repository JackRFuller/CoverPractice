using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour {

	[Header("Health Objects")]
	[SerializeField] private Text healthText;

	[Header("Instructions")]
	[SerializeField] private Text instructionText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowInstructions(string _instructions)
	{
		instructionText.text = _instructions;
		instructionText.enabled = true;
	}

	public void HideInstruction()
	{
		instructionText.enabled = false;
	}

	public void HealthUpdate(float _remainingHealth)
	{
		healthText.text = _remainingHealth.ToString();
	}

}
