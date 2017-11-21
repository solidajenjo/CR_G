using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioDestroyer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.tag == "floor")
        {
            Destroy(other.gameObject);
        }
    }
}
