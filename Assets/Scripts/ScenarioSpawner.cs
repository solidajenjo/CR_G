using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioSpawner : MonoBehaviour {

    public Rigidbody[] lanes;
    public Rigidbody[] obstacles;
    private float lastZ;
    private int lastLane;
    public int sizeOfLane, obstacleSpawnPossibility;
    private int[] materialOfTheLane;
    enum LaneTypes
    {
        GRASS, WATER, ROAD
    };
    // Use this for initialization
    void Start () {
        lastZ = transform.position.z;
        lastLane = -1;
        materialOfTheLane = new int[100];
    }
	
	// Update is called once per frame
	void Update () {
    }

    public string getFloorMaterial(int zPos)
    {
        int type = materialOfTheLane[(zPos % 100) / 10];
        if (type == (int)LaneTypes.GRASS) return "grass";
        else if (type == (int)LaneTypes.WATER) return "water";
        else return "asphalt";
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "floor")
        {
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
                    if (type == (int)LaneTypes.GRASS) newPos.y = 0.5f;                        
                    else newPos.y = -0.5f;
                    Vector3 increment = new Vector3(0.0f, 0.0f, 10.0f);
                    for (int i = 0; i < amount; ++i)
                    {
                        Instantiate(lanes[type], newPos + increment * i, transform.rotation);
                        materialOfTheLane[((int)(newPos + increment * i).z % 100) / 10] = type;
                        if (type == (int)LaneTypes.GRASS)
                        {
                            newPos.y = 0.5f;
                            int leftMargin = (int)(transform.position.x - sizeOfLane / 2);
                            int rightMargin = (int)(transform.position.x + sizeOfLane / 2);                    
                            for (int j = leftMargin; j < rightMargin; j += 10)
                            {
                                int spawnPossibility = Random.Range(0, 100);
                                if (spawnPossibility < obstacleSpawnPossibility)
                                {
                                    int which = Random.Range(0, obstacles.Length - 1);
                                    Debug.Log(which+" "+j);
                                    Instantiate(obstacles[which], new Vector3((float)j, 0.0f, (newPos + increment * i).z), transform.rotation);
                                }
                            }
                        }
                    }
                    lastZ = newPos.z + increment.z * (amount - 1);
                }
                else if (type == (int)LaneTypes.ROAD)
                {
                    Vector3 increment = new Vector3(0.0f, 0.0f, 10.0f);
                    newPos.y = 0.0f;
                    Instantiate(lanes[2], newPos, transform.rotation);
                    materialOfTheLane[((int)(newPos).z % 100) / 10] = type;                    
                    for (int i = 1; i < amount - 1; ++i)
                    {
                        Instantiate(lanes[3], newPos + increment * i, transform.rotation);
                        materialOfTheLane[((int)(newPos + increment * i).z % 100) / 10] = type;                        
                    }
                    Instantiate(lanes[4], newPos + increment * (amount - 1), transform.rotation);
                    materialOfTheLane[((int)(newPos + increment * (amount - 1)).z % 100) / 10] = type;                    
                    lastZ = newPos.z + increment.z * (amount - 1);
                }                
            }
        }
    }
}
