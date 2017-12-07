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
    public Rigidbody waterSplash, feathers;
    public int feathersAmount;
    public AmbientSounds ambientSoundController;
    private bool dead;
    public VehicleCollider vehicleCollider;
    public GameObject plainChicken;
    public AudioSource chickenClucking;
    public Text scoreText, hiscoreText;
    private int score, hiscore;
    public GameObject gameOver;
    public Light light;
    public NukeExplosion nukeExplosion;
    public CameraScript camera;
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
    }

    void Update () {
        if (dead) return;
        ambientSoundController.updateEnvironment(transform.position.z);
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
                score++;
                scoreText.text = score.ToString();
                chickenClucking.Play();
                moving = (int)Movements.MOVING_FORWARD;
                originRot = transform.rotation;
                finalRot = forwardRot;
                movementTimer = durationOfMovement;
                anim.SetBool("moving", true);
                origin = translator.position;
                destination = translator.position + new Vector3(0.0f, 0.0f, 10.0f);
                if (destination.x % 10 != 0 && 
                    scenarioSpawn.getFloorMaterial((int)destination.z) != "water") //Corrección al saltar desde un tronco fuera del agua
                   
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
                chickenClucking.Play();
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
                moving = (int)Movements.STILL;
                if (scenarioSpawn.getFloorMaterial((int)transform.position.z) == "water")
                {
                    if (troncoTranslator == null && !dead)
                    {
                        setDrowned();
                    }
                }
            }
        }
    }
    public void setDrowned()
    {
        this.GetComponent<Rigidbody>().useGravity = true;
        anim.SetBool("moving", true);
        moving += 1;
        Instantiate(waterSplash, transform.position, transform.rotation);
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
    public void setNuked()
    {
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
            Instantiate(nukeExplosion, transform.position, transform.rotation);
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
        plainChicken.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
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
