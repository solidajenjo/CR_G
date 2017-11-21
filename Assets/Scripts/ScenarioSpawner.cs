using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioSpawner : MonoBehaviour {

    public Rigidbody[] lanes;
    private float lastZ;
    private int lastLane;

    enum LaneTypes
    {
        GRASS, WATER, ROAD
    };
    // Use this for initialization
    void Start () {
        lastZ = transform.position.z;
        lastLane = -1;
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
                int type = Random.Range(0, sizeof(LaneTypes) - 1);
                while (type == lastLane) type = Random.Range(0, sizeof(LaneTypes) - 1); //Force alternance between lanes
                lastLane = type;
                int amount = Random.Range(3, 6);
                if (type == (int)LaneTypes.GRASS || type == (int)LaneTypes.WATER)
                {
                    Vector3 increment = new Vector3(0.0f, 0.0f, 10.0f);
                    for (int i = 0; i < amount; ++i)
                    {
                        newLane = Instantiate(lanes[type], newPos + increment * i, transform.rotation);
                    }
                    lastZ = newPos.z + increment.z * (amount - 1);
                }
                else if (type == (int)LaneTypes.ROAD)
                {
                    Vector3 increment = new Vector3(0.0f, 0.0f, 10.0f);
                    newLane = Instantiate(lanes[2], newPos, transform.rotation);
                    for (int i = 1; i < amount - 1; ++i)
                    {
                        newLane = Instantiate(lanes[3], newPos + increment * i, transform.rotation);
                    }
                    newLane = Instantiate(lanes[4], newPos + increment * (amount - 1), transform.rotation);
                    lastZ = newPos.z + increment.z * (amount - 1);
                }                
            }
        }
    }
}
