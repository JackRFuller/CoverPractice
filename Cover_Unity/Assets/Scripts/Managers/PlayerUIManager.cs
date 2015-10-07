using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour {

	[Header("Health Objects")]
	[SerializeField] private Text healthText;

	[Header("Instructions")]
	[SerializeField] private Text instructionText;

    [Header("Bomb Items")]
    public Image defuseTimer;
    private float bombTimer = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DefuseIndicator(float _defuseRate)
    {
        defuseTimer.enabled = true;            

        defuseTimer.fillAmount -= _defuseRate * Time.deltaTime;
    }

    public void ResetDefuseID()
    {
        defuseTimer.enabled = false;
        defuseTimer.fillAmount = 1;
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
