using UnityEngine;
using System.Collections;

public class EnemyDetectionZone : MonoBehaviour {
   
    private EnemyCombatBehaviour ECB_Script;

	// Use this for initialization
	void Start () {

        ECB_Script = transform.root.GetComponent<EnemyCombatBehaviour>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            ECB_Script.SetAggressive("detected");
        }
    }
}
