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

	[Header("Ammo Items")]
	[SerializeField] private Image ammoDiagram;  
	[SerializeField] private Text clipText;
	[SerializeField] private Text ammoText;
    [SerializeField] private float ammoFillRate;

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

	public void Shooting(int _currentClipSize)
	{
		clipText.text = _currentClipSize.ToString();
		ammoDiagram.fillAmount -= 0.024F;
	}

    public void AmmoMode()
    {
        defuseTimer.fillAmount = 0;
    }

    public void AcquireAmmo()
    {
        defuseTimer.enabled = true;
        defuseTimer.fillAmount += ammoFillRate * Time.deltaTime;
    }

    public void AmmoUpdate(int _ammoCount)
    {
        ammoText.text = _ammoCount.ToString();
    }

	public void Reload(int _currentClipSize, int _currentAmmoSize)
	{
		clipText.text = _currentClipSize.ToString();
		ammoText.text = _currentAmmoSize.ToString();
		ammoDiagram.fillAmount += 0.025F;
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
