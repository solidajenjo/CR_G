using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ScenarioSpawner : MonoBehaviour {

    public Rigidbody[] lanes;
    public Rigidbody[] obstacles;
    public Rigidbody trainSpawner, troncoSpawner, rocaSpawner;
    public Rigidbody carSpawner, wagonSpawner, avionSpawner;
    public Rigidbody hellEnd;
    public Player player;
    public Text correction;
    private float lastZ;
    private int lastLane;
    private int frameCounter;
    public int sizeOfLane, obstacleSpawnPossibility;
    private int[] materialOfTheLane;
    public int minSpeed, maxSpeed;
    private int scenario;
    public int hellStart, heavenStart;
    private bool hellEndDeployed;
    private int firstHell, firstHeaven;
    public float heavenHeight, hellHeight;
    enum LaneTypes
    {
        GRASS, WATER, ROAD, ROAD2, ROAD3, RAILROAD, DIRT, LAVA, WAGON, CLOUD, VOID
    };
    // Use this for initialization
    void Start () {
        scenario = 0;
        correction.gameObject.SetActive(false);
        frameCounter = 0;
        hellEndDeployed = false;
        lastZ = transform.position.z;
        lastLane = 4;
        materialOfTheLane = new int[3000];
        firstHeaven = -1;
        firstHell = -1;
        for (int i = 0; i < 3000; ++i) materialOfTheLane[i] = -1;
    }
	
	// Update is called once per frame
	void Update () {
        frameCounter++;
 
        if (Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene("mainMenu", LoadSceneMode.Single);
        if (player.getScore() == hellStart)
        {
            scenario = 1;            
        }
        if (player.getScore() == heavenStart && !hellEndDeployed)
        {
            scenario = 2;
            hellEndDeployed = true;
            Vector3 p = player.transform.position;
            p.y = player.transform.position.y - 0.5f;
            p.z = 200 + lastZ;
            Instantiate(hellEnd, p, transform.rotation);
        }
    }
    public int getScenario()
    {
        return scenario;
    }
    public string getFloorMaterial(int zPos)
    {
        if (zPos < 0) return "grass";
        int type = materialOfTheLane[(zPos % 1000) / 10];
        if (type == (int)LaneTypes.GRASS) return "grass";            
        else if (type == (int)LaneTypes.WATER) return "water";
        else if (type == (int)LaneTypes.ROAD) return "road";
        else if (type == (int)LaneTypes.LAVA) return "Lava";                
        else if (type == (int)LaneTypes.DIRT) return "dirt";
        else if (type == (int)LaneTypes.WAGON) return "wagon";
        else if (type == (int)LaneTypes.CLOUD) return "cloud";
        else if (type == (int)LaneTypes.VOID) return "void";
        else return "railroad";
    }
    void OnTriggerExit(Collider other)
    {
        if (scenario == 0)
        {
            if (other.tag == "floor" || other.tag == "asphalt" || other.tag == "grass"
                    || other.tag == "water" || other.tag == "dirt" || other.tag == "Lava")
            {
                Vector3 newPos = transform.position;
                newPos.x = 0.0f;
                if (newPos.z % 10 != 0)
                {
                    newPos.z = newPos.z + 10 - (newPos.z % 10);
                }
                if (lastZ < newPos.z)
                {
                    int type = Random.Range(0, 6);
                    if (type == (int)LaneTypes.ROAD2 || type == (int)LaneTypes.ROAD3) type = (int)LaneTypes.ROAD;
                    int lastLaneMem = lastLane;
                    while (type == lastLane
                        || (type == (int)LaneTypes.RAILROAD && lastLane == (int)LaneTypes.WATER))
                    {
                        type = Random.Range(0, 6); //Force alternance between lanes
                        if (type == (int)LaneTypes.ROAD2 || type == (int)LaneTypes.ROAD3) type = (int)LaneTypes.ROAD;
                    }                    
                    lastLane = type;
                    Debug.Log("LANE " + type);
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
                            Instantiate(lanes[type], newPos + increment * i, transform.rotation);
                            materialOfTheLane[((int)(newPos + increment * i).z % 1000) / 10] = type;
                            int leftMargin = (int)(-sizeOfLane / 2);
                            while (leftMargin % 10 != 0) leftMargin += 1;
                            int rightMargin = (int)(sizeOfLane / 2);
                            if (type == (int)LaneTypes.GRASS && i > 0) //Evitar obstaculos a la orilla del rio, da problemas
                            {
                                newPos.y = 0.5f;
                                for (int j = leftMargin + 20; j < rightMargin - 20; j += 10)
                                {
                                    int spawnPossibility = Random.Range(0, 100);
                                    if (spawnPossibility < obstacleSpawnPossibility)
                                    {
                                        int which = 1;
                                        if (scenario != 1) which = Random.Range(0, 2);
                                        Instantiate(obstacles[which], new Vector3((float)j, 0.0f, (newPos + increment * i).z), transform.rotation);
                                    }
                                }
                            }
                            else if (type == (int)LaneTypes.WATER)
                            {
                                if (i % 2 == 0)
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
                                    ts = (Rigidbody)Instantiate(troncoSpawner, new Vector3(rightMargin, 2.0f, (troncoSpawnPos + increment * i).z), rot);
                                    ts.GetComponent<TroncoSpawn>().setDirection(-1.0f);
                                    ts.GetComponent<TroncoSpawn>().speed = Random.Range(minSpeed, maxSpeed);
                                }
                            }
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
                        cs = (Rigidbody)Instantiate(carSpawner, new Vector3(leftMargin, 2.0f, newPos.z), transform.rotation);
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
                                carSpawn.speed = (int)Random.Range(minSpeed, maxSpeed);
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
                        cs = (Rigidbody)Instantiate(carSpawner, new Vector3(rightMargin, 2.0f, (newPos + increment * (amount - 1)).z), rot);
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
        else if (scenario == 1)
        {
            if (other.tag == "floor" || other.tag == "asphalt" || other.tag == "grass" 
                    || other.tag == "water" || other.tag == "dirt" || other.tag == "Lava")
            {
                Vector3 newPos = transform.position;
                newPos.x = 0.0f;
                if (newPos.z % 10 != 0)
                {
                    newPos.z = newPos.z + 10 - (newPos.z % 10);
                }
                if (lastZ < newPos.z)
                {
                    int type = Random.Range(6, 9);
                    int lastLaneMem = lastLane;
                    while (type == lastLane) type = Random.Range(6, 9); //Force alternance between lanes                    
                    lastLane = type;
                    int amount = Random.Range(3, 6);
                    if (type == (int)LaneTypes.DIRT || type == (int)LaneTypes.LAVA)
                    {
                        if (type == (int)LaneTypes.DIRT)
                        {
                            newPos.y = 0.5f;
                            --amount;
                        }
                        else newPos.y = -0.5f;
                        Vector3 increment = new Vector3(0.0f, 0.0f, 10.0f);
                        for (int i = 0; i < amount; ++i)
                        {
                            if (type == (int)LaneTypes.LAVA && i == 0)
                            {
                                newPos.y = 0.5f;
                                Instantiate(lanes[(int)LaneTypes.DIRT], newPos + increment * i, transform.rotation);
                                materialOfTheLane[((int)(newPos + increment * i).z % 1000) / 10] = (int)LaneTypes.DIRT;
                                newPos.y = -0.5f;
                            }
                            else
                            {
                                Instantiate(lanes[type], newPos + increment * i, transform.rotation);
                                materialOfTheLane[((int)(newPos + increment * i).z % 1000) / 10] = type;
                            }
                            int leftMargin = (int)(-sizeOfLane / 2);
                            while (leftMargin % 10 != 0) leftMargin += 1;
                            int rightMargin = (int)(sizeOfLane / 2);
                            if (type == (int)LaneTypes.DIRT) //Evitar obstaculos a la orilla del rio, da problemas
                            {
                                newPos.y = 0.5f;
                                for (int j = leftMargin + 20; j < rightMargin - 20; j += 10)
                                {
                                    int spawnPossibility = Random.Range(0, 100);
                                    if (spawnPossibility < obstacleSpawnPossibility + 20)
                                    {
                                        Instantiate(obstacles[2], new Vector3((float)j, 0.0f, (newPos + increment * i).z), Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f)));
                                    }
                                }
                            }
                            else if (type == (int)LaneTypes.LAVA)
                            {
                                if (i % 2 == 0 && i > 0)
                                {
                                    Vector3 rocaSpawnPos = newPos;
                                    Rigidbody ts;
                                    ts = (Rigidbody)Instantiate(rocaSpawner, new Vector3(leftMargin, 2.0f, (rocaSpawnPos + increment * i).z), transform.rotation);
                                    ts.GetComponent<RocaSpawn>().setDirection(1.0f);
                                    ts.GetComponent<RocaSpawn>().speed = Random.Range(minSpeed, maxSpeed);
                                }
                                else if (i > 0)
                                {
                                    Vector3 rocaSpawnPos = newPos;
                                    Quaternion rot = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                                    Rigidbody ts;
                                    ts = (Rigidbody)Instantiate(rocaSpawner, new Vector3(rightMargin, 2.0f, (rocaSpawnPos + increment * i).z), rot);
                                    ts.GetComponent<RocaSpawn>().setDirection(-1.0f);
                                    ts.GetComponent<RocaSpawn>().speed = Random.Range(minSpeed, maxSpeed);
                                }
                            }
                        }
                        if (type == (int)LaneTypes.LAVA)
                        {
                            newPos.y = 0.5f;
                            Instantiate(lanes[(int)LaneTypes.DIRT], newPos + increment * amount, transform.rotation);
                            amount++;
                        }
                        lastZ = newPos.z + increment.z * (amount - 1);
                    }
                    else if (type == (int)LaneTypes.WAGON)
                    {
                        Quaternion rot;
                        Vector3 increment = new Vector3(0.0f, 0.0f, 10.0f);
                        newPos.y = -1.5f;
                        int leftMargin = (int)(-sizeOfLane / 2);
                        int rightMargin = (int)(sizeOfLane / 2);
                        Rigidbody cs;                        
                        CarSpawn carSpawn;                        
                        for (int i = 0; i < amount; ++i)
                        {
                            int x = Random.Range(0, 100);
                            for (int j = leftMargin; j < rightMargin; j += 40)
                            {
                                newPos.x = j;
                                Instantiate(lanes[type], newPos + increment * i, transform.rotation);
                            }
                            materialOfTheLane[((int)(newPos + increment * i).z % 1000) / 10] = type;
                            if (x % 2 == 0)
                            {
                                Vector3 carSpawnPos = newPos;
                                cs = (Rigidbody)Instantiate(wagonSpawner, new Vector3(leftMargin, 2.0f, (carSpawnPos + increment * i).z), transform.rotation);
                                carSpawn = cs.GetComponent<CarSpawn>();
                                carSpawn.speed = (int)Random.Range(minSpeed * 1.5f, maxSpeed * 1.5f); //50% faster than cars
                            }
                            else
                            {
                                Vector3 carSpawnPos = newPos;
                                cs = (Rigidbody)Instantiate(wagonSpawner, new Vector3(rightMargin, 2.0f, (carSpawnPos + increment * i).z), transform.rotation);
                                carSpawn = cs.GetComponent<CarSpawn>();
                                carSpawn.speed = -(int)Random.Range(minSpeed * 1.5f, maxSpeed * 1.5f);
                            }
                        }                        
                        lastZ = newPos.z + increment.z * (amount - 1);
                    }
                }
            }

        }else if (scenario == 2)
        {
            if (other.tag == "floor" || other.tag == "asphalt" || other.tag == "grass"
                   || other.tag == "water" || other.tag == "dirt" || other.tag == "Lava")
            {
                Vector3 newPos = transform.position;
                newPos.x = 0.0f;
                if (newPos.z % 10 != 0)
                {
                    newPos.z = newPos.z + 10 - (newPos.z % 10);
                }
               
                if (lastZ < newPos.z)
                {
                    int type = Random.Range(9, 11);
                    int lastLaneMem = lastLane;
                    while (type == lastLane) type = Random.Range(9, 11); //Force alternance between lanes                    

                    lastLane = type;
                    int amount = Random.Range(3, 6);
                    bool spawn = true;
                    if (firstHeaven < 0)
                    {
                        Debug.Log("EEI");
                        firstHeaven = (int)newPos.z;
                        player.heavenStart = firstHeaven;
                        type = (int)LaneTypes.CLOUD;
                        spawn = false;

                    }
                    if (type == (int)LaneTypes.VOID)
                    {
                        newPos.y = heavenHeight - 0.5f;
                        Vector3 increment = new Vector3(0.0f, 0.0f, 10.0f);
                        for (int i = 0; i < amount; ++i)
                        {
                            Instantiate(lanes[type], newPos + increment * i, transform.rotation);
                            materialOfTheLane[((int)(newPos + increment * i).z % 1000) / 10] = type;
                            
                            int leftMargin = (int)(-sizeOfLane / 2);
                            while (leftMargin % 10 != 0) leftMargin += 1;
                            int rightMargin = (int)(sizeOfLane / 2);
                            
                            if (i % 2 == 0)
                            {
                                Vector3 troncoSpawnPos = newPos;
                                Rigidbody ts;
                                ts = (Rigidbody)Instantiate(troncoSpawner, new Vector3(leftMargin, newPos.y + 2.0f, (troncoSpawnPos + increment * i).z), transform.rotation);
                                ts.GetComponent<TroncoSpawn>().setDirection(1.0f);
                                ts.GetComponent<TroncoSpawn>().isCloud = true;
                                ts.GetComponent<TroncoSpawn>().speed = Random.Range(minSpeed, maxSpeed);
                            }
                            else
                            {
                                Vector3 troncoSpawnPos = newPos;
                                Quaternion rot = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                                Rigidbody ts;
                                ts = (Rigidbody)Instantiate(troncoSpawner, new Vector3(rightMargin, newPos.y + 2.0f, (troncoSpawnPos + increment * i).z), rot);
                                ts.GetComponent<TroncoSpawn>().setDirection(-1.0f);
                                ts.GetComponent<TroncoSpawn>().isCloud = true;
                                ts.GetComponent<TroncoSpawn>().speed = Random.Range(minSpeed, maxSpeed);
                            }
                            
                        }

                        lastZ = newPos.z + increment.z * (amount - 1);
                    }
                    else if (type == (int)LaneTypes.CLOUD)
                    {
                        Quaternion rot;
                        Vector3 increment = new Vector3(0.0f, 0.0f, 10.0f);
                        newPos.y = heavenHeight - 0.5f;
                        int leftMargin = (int)(-sizeOfLane / 2);
                        int rightMargin = (int)(sizeOfLane / 2);
                        Rigidbody cs;
                        CarSpawn carSpawn;
                        for (int i = 0; i < amount; ++i)
                        {
                            int x = Random.Range(0, 100);
                            
                            Instantiate(lanes[type], newPos + increment * i, transform.rotation);
                            materialOfTheLane[((int)(newPos + increment * i).z % 1000) / 10] = type;
                            if (spawn)
                            {
                                if (x % 2 == 0)
                                {
                                    Vector3 carSpawnPos = newPos;
                                    cs = (Rigidbody)Instantiate(avionSpawner, new Vector3(leftMargin, newPos.y + 2.0f, (carSpawnPos + increment * i).z), transform.rotation);
                                    carSpawn = cs.GetComponent<CarSpawn>();
                                    carSpawn.speed = (int)Random.Range(minSpeed * 1.5f, maxSpeed * 1.5f); //50% faster than cars
                                }
                                else
                                {
                                    Vector3 carSpawnPos = newPos;
                                    rot = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                                    cs = (Rigidbody)Instantiate(avionSpawner, new Vector3(rightMargin, newPos.y + 2.0f, (carSpawnPos + increment * i).z), rot);
                                    carSpawn = cs.GetComponent<CarSpawn>();
                                    carSpawn.isRight = true;
                                    carSpawn.speed = (int)Random.Range(minSpeed * 1.5f, maxSpeed * 1.5f);
                                }
                            }
                        }
                        lastZ = newPos.z + increment.z * (amount - 1);
                    }
                }
            }
        }
    }
}
