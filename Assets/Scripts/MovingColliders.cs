using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingColliders : MonoBehaviour {

    private bool blocked;
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {

	}

    public bool isBlocked()
    {
        return blocked;
    }
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "obstacle")
        {            
            blocked = true;
        }        
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "obstacle")
        {
            blocked = false;
        }
    }
}
