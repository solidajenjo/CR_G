using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tronco : MonoBehaviour {

    public int speed;
    private float direction;
    public float timeToLive, amplitude, atenuator, bounceTime;
    private float timer, bouncingTimer, timePassed;
    private bool bouncing;
    private float sinus;
    // Use this for initialization
    void Start () {
        timer = timeToLive;
        sinus = 0.0f;
        bouncing = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (!bouncing)
        {
            transform.Translate(speed * Time.deltaTime, 0.0f, 0.0f);
        }
        else
        {
            timePassed += Time.deltaTime;
            sinus = Mathf.Sin(timePassed * amplitude);
            transform.Translate(speed * Time.deltaTime, sinus * atenuator, 0.0f);
            bouncingTimer -= Time.deltaTime;
            if (bouncingTimer <= 0)
            {
                bouncing = false;
                sinus = 0.0f;
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
        bouncingTimer = bounceTime;
        bouncing = true;
        timePassed = 0;
    }
}
