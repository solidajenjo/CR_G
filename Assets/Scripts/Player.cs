using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class Player : MonoBehaviour {


    enum Movements
    {
        STILL, MOVING_FORWARD, MOVING_BACK, MOVING_LEFT, MOVING_RIGHT
    };

    public float speed, rotSpeed, stopAnimAngle;
    public float timeBetweenSwearing;
    private float swearingTimer;
    private bool recieveSwear;
    public Animator anim;
    private int moving;
    private Quaternion originRot, finalRot, leftRot, forwardRot, rightRot, backRot, previousRot;
    private Vector3 origin, destination;
    public Transform translator;
    private Transform troncoTranslator;
    private float lateralSpeed;
    private float journeyLength, startTime;
    public MovingColliders[] colliders;
    private bool[] directions = { false, false, false, false };
    public ScenarioSpawner scenarioSpawn;
    public Rigidbody waterSplash, feathers, scenarioDestroyerWhenDead;
    public int feathersAmount;
    public AmbientSounds ambientSoundController;
    private bool dead;
    public VehicleCollider vehicleCollider;
    public GameObject plainChicken;
    public AudioSource chickenClucking;
    public Text scoreText, hiscoreText, tutorialText, tipSwim, tipRoad, tipCam, tipWings, tipKFC, godModeText;
    private int score, hiscore;
    public GameObject gameOver;
    private float lastZ;
    public NukeExplosion nukeExplosion;
    public CameraScript camera;
    private int frameCounter;
    public int skipRate;
    private bool splash, falling;
    public bool goingToHeaven;
    private bool inHeaven;
    public Light goingToHeavenLight;
    public float heavenY, ascensionSpeed;
    public int heavenStart, hellStart;
    public bool godMode;
    void Start () {
        Cursor.visible = false;
        moving = (int)Movements.STILL;
        troncoTranslator = null;
        forwardRot = transform.rotation;
        transform.Rotate(new Vector3(0.0f, 90.0f, 0.0f));
        rightRot = transform.rotation;
        transform.Rotate(new Vector3(0.0f, 90.0f, 0.0f));
        backRot = transform.rotation;
        transform.Rotate(new Vector3(0.0f, 90.0f, 0.0f));
        leftRot = transform.rotation;
        transform.rotation = forwardRot;
        swearingTimer = 0.0f;
        recieveSwear = false;
        dead = false;
        plainChicken.gameObject.SetActive(false);
        score = 0;
        hiscore = PlayerPrefs.GetInt("high score", 0);
        hiscoreText.text = hiscoreText.text + hiscore.ToString();
        gameOver.SetActive(false);
        lastZ = 0;
        frameCounter = 0;
        scenarioDestroyerWhenDead.gameObject.SetActive(false);
        tipSwim.gameObject.SetActive(false);
        tipRoad.gameObject.SetActive(false);
        tipCam.gameObject.SetActive(false);
        tipKFC.gameObject.SetActive(false);
        tipWings.gameObject.SetActive(false);
        chickenClucking.Play();
        chickenClucking.Pause();
        splash = true;
        goingToHeavenLight.enabled = false;
        heavenStart = 100000;
        inHeaven = false;
        godMode = false;
        godModeText.gameObject.SetActive(false);
    }

    public bool isGod()
    {
        return godMode;
    }
    public string getFloorMaterial()
    {
        return scenarioSpawn.getFloorMaterial((int)transform.position.z);
    }
    void Update () {
        if (falling) anim.SetBool("moving", true);
        if (dead) return;
        frameCounter++;
        for (int i = 0; i < 4; ++i) Debug.Log("COLL " + i + " " + colliders[i].isBlocked());        
        if (transform.position.z >= heavenStart
            && transform.position.y < heavenY)
        {
            goingToHeaven = true;            
        }
        if (goingToHeaven)
        {
            if (translator.position.y < heavenY)
            {                
                goingToHeavenLight.enabled = true;
                translator.Translate(new Vector3(0.0f, ascensionSpeed * Time.deltaTime, 0.0f));
                return;
            }
            else
            {            
                goingToHeavenLight.enabled = false;
                goingToHeaven = false;
                translator.position = new Vector3(translator.position.x,
                    heavenY, translator.position.z);
                heavenStart = 1000000;
                inHeaven = true;             
            }
        }
        if ((scenarioSpawn.getFloorMaterial((int)transform.position.z) == "water"
            || scenarioSpawn.getFloorMaterial((int)transform.position.z) == "Lava"
            || scenarioSpawn.getFloorMaterial((int)transform.position.z) == "void")
            && moving == (int)Movements.STILL)
        {
            if (troncoTranslator == null && !dead)
            {
                if (!godMode)
                {
                    Debug.Log("DROWN " + scenarioSpawn.getFloorMaterial((int)transform.position.z));
                    if (scenarioSpawn.getFloorMaterial((int)transform.position.z) == "Lava"
                        || scenarioSpawn.getFloorMaterial((int)transform.position.z) == "void") splash = false;
                    falling = true;
                    if (scenarioSpawn.getFloorMaterial((int)transform.position.z) == "Lava") setFriedChicken();
                    if (scenarioSpawn.getFloorMaterial((int)transform.position.z) == "void") tipWings.gameObject.SetActive(true);
                    setDrowned();
                }
            }
        }
        if (frameCounter % skipRate == 0) ambientSoundController.updateEnvironment(transform.position.z); //OPTIMIZATION
        if (recieveSwear)
        {
            swearingTimer -= Time.deltaTime;
            if (swearingTimer <= 0)
            {
                recieveSwear = false;
            }
        }
        if (troncoTranslator != null)
        {
            translator.Translate(lateralSpeed * Time.deltaTime, 0.0f, 0.0f);
            transform.position = new Vector3(translator.position.x, transform.position.y, translator.position.z);
        }
        for (int i = 0; i < 4; i++)
        {
            if (colliders[i].isBlocked())
            {                
                directions[i] = false;
            }
            else
            {
                directions[i] = true;
            }
        }
        if (moving == (int)Movements.STILL && !goingToHeaven)
        {
            if (Input.GetKeyUp(KeyCode.G))
            {
                godMode = !godMode;
                godModeText.gameObject.SetActive(!godModeText.IsActive());
            }
            if (Input.GetKey("up") && directions[0])
            {
                chickenClucking.Play();
                moving = (int)Movements.MOVING_FORWARD;
                originRot = transform.rotation;
                finalRot = forwardRot;
                anim.SetBool("moving", true);
                origin = translator.position;
                destination = translator.position + new Vector3(0.0f, 0.0f, 10.0f);
                if (destination.x % 10 != 0 
                    && scenarioSpawn.getFloorMaterial((int)destination.z) != "water"
                    && scenarioSpawn.getFloorMaterial((int)destination.z) != "Lava"
                    && scenarioSpawn.getFloorMaterial((int)destination.z) != "void") //Corrección al saltar desde un tronco, roca o nube fuera del agua, lava o vacío
                   
                {
                    float xDisp = destination.x % 10;
                    if (xDisp > 5)
                    {
                        destination.x += 10 - xDisp;
                    }
                    else
                    {
                        destination.x -= xDisp;
                    }
                }
                journeyLength = Vector3.Distance(origin, destination);
                startTime = Time.time;
                return;
            }
            if (Input.GetKey("left") && directions[3])
            {
                chickenClucking.Play();
                moving = (int)Movements.MOVING_LEFT;
                originRot = transform.rotation;
                finalRot = leftRot;
                anim.SetBool("moving", true);
                origin = translator.position;
                destination = translator.position + new Vector3(-10.0f, 0.0f, 0.0f);
                journeyLength = Vector3.Distance(origin, destination);
                startTime = Time.time;
                return;
            }
            if (Input.GetKey("right") && directions[1])
            {
                chickenClucking.Play();
                moving = (int)Movements.MOVING_RIGHT;
                originRot = transform.rotation;
                finalRot = rightRot;
                anim.SetBool("moving", true);
                origin = translator.position;
                destination = translator.position + new Vector3(10.0f, 0.0f, 0.0f);
                journeyLength = Vector3.Distance(origin, destination);
                startTime = Time.time;
                return;
            }
            if (Input.GetKey("down") && directions[2])
            {
                chickenClucking.UnPause();
                moving = (int)Movements.MOVING_BACK;
                originRot = transform.rotation;
                finalRot = backRot;
                anim.SetBool("moving", true);
                origin = translator.position;
                destination = translator.position + new Vector3(0.0f, 0.0f, -10.0f);
                journeyLength = Vector3.Distance(origin, destination);
                startTime = Time.time;
                return;
            }
        }
        else
        {
            transform.rotation = Quaternion.Slerp(originRot, finalRot, (Time.time - startTime) * rotSpeed);
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            translator.position = Vector3.Lerp(origin, destination, fracJourney);
            
            if (Vector3.Distance(translator.position, destination) == 0.0f)
            {
                translator.position = destination;
                if (moving == (int)Movements.MOVING_FORWARD &&
                    transform.position.z > lastZ)
                {                    
                    score++;
                    if (score > 3) tutorialText.gameObject.SetActive(false);
                    scoreText.text = score.ToString();
                    lastZ = transform.position.z;
                }
                anim.SetBool("moving", false);
                moving = (int)Movements.STILL;
                chickenClucking.Pause();                
            }
        }
    }
    public void setDrowned()
    {
        Debug.Log("DROWNED DEATH");
        Time.timeScale = 0.3f;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
        this.GetComponent<Rigidbody>().useGravity = true;
        scenarioDestroyerWhenDead.gameObject.SetActive(true);
        anim.SetBool("moving", true);
        moving += 1;
        if (splash) Instantiate(waterSplash, transform.position, transform.rotation);
        this.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, -2000.0f, 0.0f));
        dead = true;
        gameOver.SetActive(true);
        if (!tipCam.IsActive() && !tipKFC.IsActive() && !tipWings.IsActive()) tipSwim.gameObject.SetActive(true);
        camera.rewind();
        hiscore = PlayerPrefs.GetInt("high score", 0);
        if (dead && hiscore < score)
        {
            PlayerPrefs.SetInt("high score", score);
            hiscoreText.text = "hi-score " + score.ToString();
        }
    }
    public void setNuked()
    {
        Debug.Log("NUKED DEATH");
        Time.timeScale = 0.3f;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
        tipCam.gameObject.SetActive(true);        
        if (scenarioSpawn.getFloorMaterial((int)transform.position.z) == "water")
        {
            setDrowned();
            return;
        }
        else if (scenarioSpawn.getFloorMaterial((int)transform.position.z) == "road")
        {
            setPlainChicken();
        }
        else{
            tipKFC.gameObject.SetActive(true);
            Vector3 nukePos = transform.position;
            nukePos.y = translator.position.y;
            Instantiate(nukeExplosion, nukePos, transform.rotation);
            scenarioDestroyerWhenDead.gameObject.SetActive(true);
            this.gameObject.SetActive(false);            
            dead = true;
            gameOver.SetActive(true);
            hiscore = PlayerPrefs.GetInt("high score", 0);
            if (dead && hiscore < score)
            {
                PlayerPrefs.SetInt("high score", score);
                hiscoreText.text = "hi-score " + score.ToString();
            }
            Vector3 feathersPos = new Vector3(translator.position.x, translator.position.y + 5.0f, translator.position.z);
            for (int i = 0; i < feathersAmount; ++i) Instantiate(feathers, feathersPos, transform.rotation);
        }
    }
    public bool canBeSweared()
    {
        return !recieveSwear;
    }

    public void getSweared()
    {
        swearingTimer = timeBetweenSwearing;
        recieveSwear = true;
    }
    public void setFriedChicken()
    {
        if (!godMode)
        {
            Time.timeScale = 0.3f;
            tipKFC.gameObject.SetActive(true);
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
            this.gameObject.SetActive(false);
            dead = true;
            gameOver.SetActive(true);
            hiscore = PlayerPrefs.GetInt("high score", 0);
            if (dead && hiscore < score)
            {
                PlayerPrefs.SetInt("high score", score);
                hiscoreText.text = "hi-score " + score.ToString();
            }
            Vector3 feathersPos = new Vector3(transform.position.x, transform.position.y + 5.0f, transform.position.z);
            for (int i = 0; i < feathersAmount * 3; ++i) Instantiate(feathers, feathersPos, transform.rotation);
        }
    }
    public void setPlainChicken()
    {
        Debug.Log("PLAIN DEATH");
        Time.timeScale = 0.3f;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
        plainChicken.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        if (!tipCam.IsActive()) tipRoad.gameObject.SetActive(true);
        scenarioDestroyerWhenDead.gameObject.SetActive(true);
        float offset = 1.0f;
        if (!inHeaven) offset = -1.0f;
        translator.position = new Vector3(translator.position.x, translator.position.y + 0.8f + offset,
            translator.position.z);
        Vector3 feathersPos = new Vector3(translator.position.x, translator.position.y + 5.0f, translator.position.z);
        for (int i = 0; i < feathersAmount; ++i) Instantiate(feathers, feathersPos, transform.rotation);
        dead = true;
        gameOver.SetActive(true);
        camera.rewind();
        hiscore = PlayerPrefs.GetInt("high score", 0);
        if (dead && hiscore < score)
        {
            PlayerPrefs.SetInt("high score", score);
            hiscoreText.text = "hi-score " + score.ToString();
        }
    }

    public int getScore()
    {
        return score;
    }
    public void recieveKick()
    {
        Debug.Log("OUCH");
        chickenClucking.UnPause();
        moving = (int)Movements.MOVING_BACK;
        originRot = transform.rotation;
        finalRot = originRot;
        anim.SetBool("moving", true);
        origin = translator.position;
        destination = translator.position + new Vector3(0.0f, 0.0f, -10.0f);
        float zDisp = destination.z % 10;
        if (zDisp > 5)
        {
            destination.z += 10 - zDisp;
        }
        else
        {
            destination.z -= zDisp;
        }
        journeyLength = Vector3.Distance(origin, destination);
        startTime = Time.time;
    }
    public bool isDead()
    {
        return dead;
    }
    void OnTriggerEnter(Collider other)
    {        
        if (other.tag == "troncoFlotante")
        {
            troncoTranslator = other.transform;
            lateralSpeed = other.GetComponent<Tronco>().getSpeed();
            other.GetComponent<Tronco>().setBouncing();
        }
        else if (other.tag == "rocaFlotante")
        {
            troncoTranslator = other.transform;
            lateralSpeed = other.GetComponent<Roca>().getSpeed();
            other.GetComponent<Roca>().setBouncing();
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "troncoFlotante" || other.tag == "rocaFlotante")
        {
            troncoTranslator = null;
        }
    }
    
}
