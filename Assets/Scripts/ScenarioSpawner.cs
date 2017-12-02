using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioSpawner : MonoBehaviour {

    public Rigidbody[] lanes;
    public Rigidbody[] obstacles;
    public Rigidbody trainSpawner, troncoSpawner;
    public Rigidbody carSpawner;
    private float lastZ;
    private int lastLane;
    public int sizeOfLane, obstacleSpawnPossibility;
    private int[] materialOfTheLane;
    enum LaneTypes
    {
        GRASS, WATER, ROAD, RAILROAD
    };
    // Use this for initialization
    void Start () {
        lastZ = transform.position.z;
        lastLane = -1;
        materialOfTheLane = new int[1000];
    }
	
	// Update is called once per frame
	void Update () {
    }

    public string getFloorMaterial(int zPos)
    {
        int type = materialOfTheLane[(zPos % 1000) / 10];
        if (type == (int)LaneTypes.GRASS) return "grass";
        else if (type == (int)LaneTypes.WATER) return "water";
        else if (type == (int)LaneTypes.ROAD) return "road";
        else return "railroad";
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
                int type = Random.Range(0, 4);
                while (type == lastLane) type = Random.Range(0, 4); //Force alternance between lanes
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
                        materialOfTheLane[((int)(newPos + increment * i).z % 1000) / 10] = type;
                        int leftMargin = (int)(transform.position.x - sizeOfLane / 2);
                        int rightMargin = (int)(transform.position.x + sizeOfLane / 2);
                        if (type == (int)LaneTypes.GRASS)
                        {
                            newPos.y = 0.5f;                           
                            for (int j = leftMargin; j < rightMargin; j += 10)
                            {
                                int spawnPossibility = Random.Range(0, 100);
                                if (spawnPossibility < obstacleSpawnPossibility)
                                {
                                    int which = Random.Range(0, obstacles.Length - 1);        
                                    Instantiate(obstacles[which], new Vector3((float)j, 0.0f, (newPos + increment * i).z), transform.rotation);
                                }
                            }
                        }
                        else
                        {
                            if (i % 2 == 0)
                            {
                                Vector3 troncoSpawnPos = newPos;
                                Instantiate(troncoSpawner, new Vector3(leftMargin, 2.0f, (troncoSpawnPos + increment * i).z), transform.rotation)
                                    .GetComponent<TroncoSpawn>().setDirection(1.0f);
                            }
                            else
                            {
                                Vector3 troncoSpawnPos = newPos;
                                Quaternion rot = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                                Instantiate(troncoSpawner, new Vector3(rightMargin, 2.0f, (troncoSpawnPos + increment * i).z), rot)
                                    .GetComponent<TroncoSpawn>().setDirection(-1.0f);
                            }
                        }
                    }
                    lastZ = newPos.z + increment.z * (amount - 1);
                }
                else if (type == (int)LaneTypes.ROAD)
                {
                    Vector3 increment = new Vector3(0.0f, 0.0f, 10.0f);
                    newPos.y = 0.0f;
                    int leftMargin = (int)(transform.position.x - sizeOfLane / 2);
                    int rightMargin = (int)(transform.position.x + sizeOfLane / 2);
                    Instantiate(lanes[2], newPos, transform.rotation);
                    materialOfTheLane[((int)(newPos).z % 1000) / 10] = type;
                    Instantiate(carSpawner, new Vector3(leftMargin * 0.7f, 2.0f, newPos.z), transform.rotation);
                    for (int i = 1; i < amount - 1; ++i)
                    {
                        int x = Random.Range(0, 100);
                        Instantiate(lanes[3], newPos + increment * i, transform.rotation);
                        materialOfTheLane[((int)(newPos + increment * i).z % 1000) / 10] = type;
                        if (x % 2 == 0)
                        {
                            Vector3 carSpawnPos = newPos;
                            Instantiate(carSpawner, new Vector3(leftMargin * 0.7f, 2.0f, (carSpawnPos + increment * i).z), transform.rotation);
                        }
                        else
                        {
                            Vector3 carSpawnPos = newPos;
                            Quaternion rot = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                            Instantiate(carSpawner, new Vector3(rightMargin * 0.7f, 2.0f, (carSpawnPos + increment * i).z), rot);
                        }
                    }
                    Instantiate(lanes[4], newPos + increment * (amount - 1), transform.rotation);
                    materialOfTheLane[((int)(newPos + increment * (amount - 1)).z % 1000) / 10] = type;
                    lastZ = newPos.z + increment.z * (amount - 1);                   
                }

                else if (type == (int)LaneTypes.RAILROAD)
                {
                    newPos.y = -3.0f;
                    Quaternion rot = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                    int leftMargin = (int)(transform.position.x - sizeOfLane / 2);
                    int rightMargin = (int)(transform.position.x + sizeOfLane / 2);
                    Vector3 trainSpawnPos = newPos;
                    trainSpawnPos.x = leftMargin;
                    trainSpawnPos.y = 8.5f;
                    Instantiate(trainSpawner, trainSpawnPos, rot);
                    for (int j = leftMargin; j < rightMargin; j += 40)
                    {
                        newPos.x = j;
                        Instantiate(lanes[5], newPos, rot);
                    }                    
                    materialOfTheLane[((int)(newPos.z) % 1000) / 10] = type;                    
                    lastZ = newPos.z;
                }
            }
        }
    }
}
