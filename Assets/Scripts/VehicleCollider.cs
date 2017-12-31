using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleCollider : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "vehicle")
        {
            try {
                Player player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
                if (!player.isGod())
                {
                    player.setPlainChicken();
                    this.gameObject.SetActive(false);
                }
            }
            catch
            {

            }                        
        }
    }
}
