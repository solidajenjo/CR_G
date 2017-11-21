using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioSpawner : MonoBehaviour {

    public Rigidbody[] lanes;
    private float lastZ;
	// Use this for initialization
	void Start () {
        lastZ = transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "floor")
        {
            Rigidbody newLane;
            Vector3 newPos = transform.position;
            if (newPos.z % 10 != 0)
            {
                newPos.z = newPos.z + 10 - (newPos.z % 10);
            }
            if (lastZ < newPos.z)
            {
                lastZ = newPos.z;
                newLane = Instantiate(lanes[Random.Range(0, lanes.Length)], newPos, transform.rotation);
            }
        }
    }
}
