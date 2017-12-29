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
    public float durationOfMovement;
    public float timeBetweenSwearing;
    private float swearingTimer;
    private bool recieveSwear;
    public Animator anim;
    private float movementTimer;
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
    public Text scoreText, hiscoreText, tutorialText, tipSwim, tipRoad, tipCam;
    private int score, hiscore;
    public GameObject gameOver;
    private float lastZ;
    public NukeExplosion nukeExplosion;
    public CameraScript camera;
    private int frameCounter;
    public int skipRate;
    void Start () {
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
        chickenClucking.Play();
        chickenClucking.Pause();
    }

    public string getFloorMaterial()
    {
        return scenarioSpawn.getFloorMaterial((int)transform.position.z);
    }
    void Update () {
        if (dead) return;
        frameCounter++;
        if ((scenarioSpawn.getFloorMaterial((int)transform.position.z) == "water"
            || scenarioSpawn.getFloorMaterial((int)transform.position.z) == "Lava")
            && moving == (int)Movements.STILL)
        {
            if (troncoTranslator == null && !dead)
            {
                setDrowned();
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
            translator.transform.position = new Vector3(translator.position.x, troncoTranslator.transform.position.y, translator.position.z);
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
        if (moving == (int)Movements.STILL)
        {
            if (Input.GetKey("up") && directions[0])
            {
                chickenClucking.Play();
                moving = (int)Movements.MOVING_FORWARD;
                originRot = transform.rotation;
                finalRot = forwardRot;
                movementTimer = durationOfMovement;
                anim.SetBool("moving", true);
                origin = translator.position;
                destination = translator.position + new Vector3(0.0f, 0.0f, 10.0f);
                if (destination.x % 10 != 0 
                    && scenarioSpawn.getFloorMaterial((int)destination.z) != "water"
                    && scenarioSpawn.getFloorMaterial((int)destination.z) != "Lava") //Corrección al saltar desde un tronco fuera del agua
                   
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
                movementTimer = durationOfMovement;
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
                movementTimer = durationOfMovement;
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
                movementTimer = durationOfMovement;
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
            movementTimer -= Time.deltaTime;
            if (Quaternion.Angle(transform.rotation, finalRot) < stopAnimAngle)
            {
                anim.SetBool("moving", false);
            }
            if (movementTimer <= 0)
            {
                if (moving == (int)Movements.MOVING_FORWARD &&
                    transform.position.z > lastZ)
                {
                    if (score > 8) tutorialText.gameObject.SetActive(false);
                    score++;
                    scoreText.text = score.ToString();
                    lastZ = transform.position.z;
                }
                moving = (int)Movements.STILL;
                chickenClucking.Pause();                
            }
        }
    }
    public void setDrowned()
    {
        Debug.Log("SLOW MO");
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
        this.GetComponent<Rigidbody>().useGravity = true;
        scenarioDestroyerWhenDead.gameObject.SetActive(true);
        anim.SetBool("moving", true);
        moving += 1;
        Instantiate(waterSplash, transform.position, transform.rotation);
        dead = true;
        gameOver.SetActive(true);
        if (!tipCam.IsActive()) tipSwim.gameObject.SetActive(true);
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
            Vector3 nukePos = transform.position;
            nukePos.y = 0.0f;
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

    public void setPlainChicken()
    {
        Debug.Log("SLOW MO");
        Time.timeScale = 0.3f;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
        plainChicken.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        if (!tipCam.IsActive()) tipRoad.gameObject.SetActive(true);
        scenarioDestroyerWhenDead.gameObject.SetActive(true);
        translator.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
        Vector3 feathersPos = new Vector3(transform.position.x, 5.0f, transform.position.z);
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
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "troncoFlotante")
        {
            troncoTranslator = null;
        }
    }

}
