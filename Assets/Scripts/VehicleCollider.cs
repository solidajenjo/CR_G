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
            Debug.Log("CHOQUE");
            Player player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
            player.setPlainChicken();
            this.gameObject.SetActive(false);
        }
    }
}
