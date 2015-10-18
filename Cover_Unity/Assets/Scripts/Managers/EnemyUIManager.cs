using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyUIManager : MonoBehaviour {

    [Header("Core UI Items")]
    [SerializeField] private Canvas uiCanvas;

    [Header("Health Items")]
    [SerializeField] private Transform healthBar;
    private bool isHBActive;

    [Header("Target")]
    [SerializeField] private Transform target;

    private bool isDead;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (isHBActive && !isDead)
        {
            Vector3 _targetPostition = new Vector3(target.position.x,transform.position.y,target.position.z);
            uiCanvas.transform.LookAt(_targetPostition);
        }
	
	}

    public void HealthUpdate(float _currentHealth)
    {
        if (!isHBActive)
        {
            healthBar.GetComponent<Image>().enabled = true;
            isHBActive = true;
        }
        
        float _healhBarWidth = _currentHealth / 100;
        healthBar.localScale = new Vector3(_healhBarWidth, 1, 1);

        if(_currentHealth <= 0)
        {
            healthBar.localScale = new Vector3(0, 1, 1);
            isDead = true;
        }
    }
}
