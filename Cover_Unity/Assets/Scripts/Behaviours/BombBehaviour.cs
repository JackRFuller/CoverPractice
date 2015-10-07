using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BombBehaviour : MonoBehaviour {

	[Header("Managers")]
	[SerializeField] private PlayerUIManager PUM_Script;

	[Header("Countdown Items")]
	[SerializeField] private float minutes;
	[SerializeField] private float seconds;
	private float miliseconds;
	[SerializeField] private Text countdownText;
	private bool timerRunning = true;

	[Header("Defuse Items")]	
	private float startingdefuseTime;
	[SerializeField] private float defuseRate;
	private bool defusePossible;
	private bool defused = false;
	[SerializeField] private string instructionText;


	// Use this for initialization
	void Start () {

		SetValues();
	
	}

	void SetValues()
	{
		string formattedTime = string.Format("{0}:{1}:{2}", minutes, seconds, (int)miliseconds);
		countdownText.text = formattedTime;
	}
	
	// Update is called once per frame
	void Update () {

		if(!defused)
		{
			RunTimer();
			
			ActivateDefuse();
		}
	}

	void ActivateDefuse()
	{
		if(defusePossible)
		{
			if(Input.GetKey(KeyCode.E))
			{
				Defuse();
			}
			if(Input.GetKeyUp(KeyCode.E))
			{
				ResetDefuse();
			}
		}
	}

	void Defuse()
	{
        PUM_Script.DefuseIndicator(defuseRate);

		if(PUM_Script.defuseTimer.fillAmount <= 0)
        {
            defused = true;
            PUM_Script.HideInstruction();
        }
	}

	void ResetDefuse()
	{
        PUM_Script.ResetDefuseID();
    }

	void RunTimer()
	{
		if(timerRunning)
		{
			if(miliseconds <= 0)
			{
				if(seconds <= 0)
				{
					minutes--;
					seconds = 59;
				}
				else if (seconds >= 0)
				{
					seconds--;
				}
				
				miliseconds = 100;
			}
			
			miliseconds -= Time.deltaTime * 100;
			
			string formattedTime = string.Format("{0}:{1}:{2}", minutes, seconds, (int)miliseconds);
			countdownText.text = formattedTime;

			CheckTimer();
		}

	}

	void CheckTimer()
	{
		if(minutes <=0 && seconds <= 0 && miliseconds <=0)
		{
			timerRunning = false;
			countdownText.text = "--:--:--";
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player" && !defused)
		{
			defusePossible = true;
			PUM_Script.ShowInstructions(instructionText);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Player")
		{
			defusePossible = false;
			PUM_Script.HideInstruction();
			ResetDefuse();
		}
	}
}
