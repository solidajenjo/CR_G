using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roca : MonoBehaviour {

    public int speed;
    private float direction;
    public float timeToLive, amplitude, atenuator, bounceAmount;
    private float timer, bouncingCount, timePassed;
    private bool bouncing;
    private float sinus;
    private float y;
    // Use this for initialization
    void Start()
    {
        timer = timeToLive;
        sinus = 0.0f;
        bouncing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!bouncing)
        {
            transform.Translate(speed * Time.deltaTime, 0.0f, 0.0f);
        }
        else
        {
            bouncingCount -= 1.0f;
            transform.Translate(speed * Time.deltaTime, Mathf.Sin(bouncingCount) * atenuator, 0.0f);
            if (bouncingCount <= 0.0f)
            {
                bouncing = false;
                transform.position = new Vector3(transform.position.x,
                    y, transform.position.z);
            }
        }
        timer -= Time.deltaTime;
        if (timer <= 0) Destroy(gameObject);
    }

    public void setDirection(float dir)
    {
        direction = dir;
    }
    public float getSpeed()
    {
        return speed * direction;
    }
    public void setBouncing()
    {
        bouncingCount = bounceAmount;
        bouncing = true;
        timePassed = 0;
        y = transform.position.y;
    }
}
