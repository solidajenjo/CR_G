﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour {


    enum Movements
    {
        STILL, MOVING_FORWARD, MOVING_BACK, MOVING_LEFT, MOVING_RIGHT
    };

    public float speed, rotSpeed, stopAnimAngle;
    public float durationOfMovement;
    public Animator anim;
    private float movementTimer;
    private int moving;
    private Quaternion originRot, finalRot, leftRot, forwardRot, rightRot, backRot, previousRot;
    private Vector3 origin, destination;
    public Transform translator;
    private Transform troncoTranslator;
    private float journeyLength, startTime;
    public MovingColliders[] colliders;
    private bool[] directions = { false, false, false, false };
    public ScenarioSpawner scenarioSpawn;
    public Rigidbody waterSplash;

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

    }
	
	void Update () {
        if (troncoTranslator != null)
        {
            translator.position = troncoTranslator.position;
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
                    if (troncoTranslator == null)
                    {
                        Debug.Log("AHOGADO");
                        Destroy(gameObject);
                        Instantiate(waterSplash, transform.position, transform.rotation);
                    }
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "troncoFlotante")
        {
            Debug.Log("TRONCO");
            troncoTranslator = other.transform;
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
