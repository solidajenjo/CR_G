using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ScenarioSpawner : MonoBehaviour {

    public Rigidbody[] lanes;
    public Rigidbody[] obstacles;
    public Rigidbody trainSpawner, troncoSpawner;
    public Rigidbody carSpawner;
    private float lastZ;
    private int lastLane;
    public int sizeOfLane, obstacleSpawnPossibility;
    private int[] materialOfTheLane;
    public int minSpeed, maxSpeed;
    private int scenario;
    enum LaneTypes
    {
        GRASS, WATER, ROAD, RAILROAD, DIRT, LAVA
    };
    // Use this for initialization
    void Start () {
        scenario = 0;
        lastZ = transform.position.z;
        lastLane = -1;
        materialOfTheLane = new int[1000];
        for (int i = 0; i < 1000; ++i) materialOfTheLane[i] = (int)LaneTypes.GRASS;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene("mainMenu", LoadSceneMode.Single);
    }
    public int getScenario()
    {
        return scenario;
    }
    public string getFloorMaterial(int zPos)
    {
        //INTENTA MANTENER EN MATERIAL OF THE LANE EL TIPO EXACTO
        //PARA EFECTOS Y MUERTES DISTINTAS
        //NO ES LO MISMO CAER EN LA LAVA QUE EN EL AGUA Y HAY QUE PODER DIFERENCIARLOS
        if (zPos < 0) return "grass";
        int type = materialOfTheLane[(zPos % 1000) / 10];
        if (type == (int)LaneTypes.GRASS) return "grass";            
        else if (type == (int)LaneTypes.WATER) return "water";
        else if (type == (int)LaneTypes.LAVA) return "Lava";                
        else if (type == (int)LaneTypes.DIRT) return "dirt";
        else return "railroad";
    }
    void OnTriggerExit(Collider other)
    {
        scenario = (int) (lastZ / 500) % 30; //Cada 50 pasos cambio de escenario. Escenario de cueva empezado.
        Debug.Log("Last Z = " + lastZ.ToString());
        if (other.tag == "asphalt" || other.tag == "grass" || other.tag == "water" || other.tag == "dirt" || other.tag == "Lava")
        {
            Vector3 newPos = transform.position;
            newPos.x = 0.0f;
            if (newPos.z % 10 != 0)
            {
                newPos.z = newPos.z + 10 - (newPos.z % 10);
            }
            if (lastZ < newPos.z)
            {
                int type = Random.Range(0, 4);
                int lastLaneMem = lastLane;
                while (type == lastLane) type = Random.Range(0, 4); //Force alternance between lanes
                lastLane = type;
                int amount = Random.Range(3, 6);
                if (type == (int)LaneTypes.GRASS || type == (int)LaneTypes.WATER)
                {
                    if (type == (int)LaneTypes.GRASS)
                    {
                        newPos.y = 0.5f;
                        --amount;
                    }
                    else newPos.y = -0.5f;
                    Vector3 increment = new Vector3(0.0f, 0.0f, 10.0f);
                    for (int i = 0; i < amount; ++i)
                    {
                        if (type == (int)LaneTypes.WATER && i == 0)
                        {
                            newPos.y = 0.5f;
                            if (scenario == 1)
                            {
                                Instantiate(lanes[(int)LaneTypes.GRASS + 6], newPos + increment * i, transform.rotation);
                                materialOfTheLane[((int)(newPos + increment * i).z % 1000) / 10] = (int)LaneTypes.DIRT;
                            }
                            else
                            {
                                Instantiate(lanes[(int)LaneTypes.GRASS], newPos + increment * i, transform.rotation);
                                materialOfTheLane[((int)(newPos + increment * i).z % 1000) / 10] = (int)LaneTypes.GRASS;
                            }
                            newPos.y = -0.5f;
                        }
                        else
                        {
                            if (scenario == 1)
                            {
                                Instantiate(lanes[type + 6], newPos + increment * i, transform.rotation);
                                if (type == (int)LaneTypes.GRASS)
                                    materialOfTheLane[((int)(newPos + increment * i).z % 1000) / 10] = (int)LaneTypes.DIRT;
                                if (type == (int)LaneTypes.WATER)
                                    materialOfTheLane[((int)(newPos + increment * i).z % 1000) / 10] = (int)LaneTypes.LAVA;
                            }
                            else
                            {
                                Instantiate(lanes[type], newPos + increment * i, transform.rotation);
                                materialOfTheLane[((int)(newPos + increment * i).z % 1000) / 10] = type;
                            }
                        }                        
                        int leftMargin = (int)(-sizeOfLane / 2);
                        while (leftMargin % 10 != 0) leftMargin += 1;
                        int rightMargin = (int)(sizeOfLane / 2);
                        if (type == (int)LaneTypes.GRASS) //Evitar obstaculos a la orilla del rio, da problemas
                        {
                            newPos.y = 0.5f;                           
                            for (int j = leftMargin + 20; j < rightMargin - 20; j += 10)
                            {
                                int spawnPossibility = Random.Range(0, 100);
                                if (spawnPossibility < obstacleSpawnPossibility)
                                {
                                    int which = 1;
                                    if (scenario != 1) which = Random.Range(0, obstacles.Length);
                                    Instantiate(obstacles[which], new Vector3((float)j, 0.0f, (newPos + increment * i).z), transform.rotation);
                                }
                            }
                        }
                        else if (type == (int)LaneTypes.WATER)
                        {
                            if (i % 2 == 0 && i > 0)
                            {
                                Vector3 troncoSpawnPos = newPos;
                                Rigidbody ts;
                                ts = (Rigidbody)Instantiate(troncoSpawner, new Vector3(leftMargin, 2.0f, (troncoSpawnPos + increment * i).z), transform.rotation);
                                ts.GetComponent<TroncoSpawn>().setDirection(1.0f);
                                ts.GetComponent<TroncoSpawn>().speed = Random.Range(minSpeed, maxSpeed);
                            }
                            else if (i > 0)
                            {
                                Vector3 troncoSpawnPos = newPos;
                                Quaternion rot = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                                Rigidbody ts;
                                ts = (Rigidbody) Instantiate(troncoSpawner, new Vector3(rightMargin, 2.0f, (troncoSpawnPos + increment * i).z), rot);
                                ts.GetComponent<TroncoSpawn>().setDirection(-1.0f);
                                ts.GetComponent<TroncoSpawn>().speed = Random.Range(minSpeed, maxSpeed);
                            }
                        }
                    }
                    if (type == (int)LaneTypes.WATER)
                    {
                        newPos.y = 0.5f;
                        if (scenario == 1) Instantiate(lanes[(int)LaneTypes.GRASS + 6], newPos + increment * amount, transform.rotation);
                        else Instantiate(lanes[(int)LaneTypes.GRASS], newPos + increment * amount, transform.rotation);
                        amount++;
                    }
                    lastZ = newPos.z + increment.z * (amount - 1);
                }
                else if (type == (int)LaneTypes.ROAD)
                {
                    Quaternion rot;
                    Vector3 increment = new Vector3(0.0f, 0.0f, 10.0f);
                    newPos.y = 0.0f;
                    int leftMargin = (int)(-sizeOfLane / 2);
                    int rightMargin = (int)(sizeOfLane / 2);
                    Instantiate(lanes[2], newPos, transform.rotation);
                    materialOfTheLane[((int)(newPos).z % 1000) / 10] = type;
                    Rigidbody cs;
                    cs = (Rigidbody) Instantiate(carSpawner, new Vector3(leftMargin, 2.0f, newPos.z), transform.rotation);
                    CarSpawn carSpawn = cs.GetComponent<CarSpawn>();
                    carSpawn.speed = (int)Random.Range(minSpeed, maxSpeed);
                    for (int i = 1; i < amount - 1; ++i)
                    {
                        int x = Random.Range(0, 100);
                        Instantiate(lanes[3], newPos + increment * i, transform.rotation);
                        materialOfTheLane[((int)(newPos + increment * i).z % 1000) / 10] = type;
                        if (x % 2 == 0)
                        {
                            Vector3 carSpawnPos = newPos;
                            cs = (Rigidbody)Instantiate(carSpawner, new Vector3(leftMargin, 2.0f, (carSpawnPos + increment * i).z), transform.rotation);
                            carSpawn = cs.GetComponent<CarSpawn>();
                            carSpawn.speed = (int) Random.Range(minSpeed, maxSpeed);
                        }
                        else
                        {
                            Vector3 carSpawnPos = newPos;
                            rot = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                            cs = (Rigidbody)Instantiate(carSpawner, new Vector3(rightMargin, 2.0f, (carSpawnPos + increment * i).z), rot);
                            carSpawn = cs.GetComponent<CarSpawn>();
                            carSpawn.speed = (int)Random.Range(minSpeed, maxSpeed);
                        }
                    }
                    Instantiate(lanes[4], newPos + increment * (amount - 1), transform.rotation);
                    rot = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                    cs = (Rigidbody)Instantiate(carSpawner, new Vector3(rightMargin, 2.0f, (newPos + increment * (amount-1)).z), rot);
                    carSpawn = cs.GetComponent<CarSpawn>();
                    carSpawn.speed = Random.Range(15, 30);
                    materialOfTheLane[((int)(newPos + increment * (amount - 1)).z % 1000) / 10] = type;
                    lastZ = newPos.z + increment.z * (amount - 1);                   
                }

                else if (type == (int)LaneTypes.RAILROAD)
                {
                    newPos.y = -3.0f;
                    Quaternion rot = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                    int leftMargin = (int)(-sizeOfLane / 2);
                    int rightMargin = (int)(sizeOfLane / 2);
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
