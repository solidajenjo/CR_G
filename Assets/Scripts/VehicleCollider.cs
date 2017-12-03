using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleCollider : MonoBehaviour {

    private bool dead;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public bool isBlocked()
    {
        return dead;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "vehicle")
        {
            Debug.Log("CHOQUE");
            dead = true;
        }
    }
}
