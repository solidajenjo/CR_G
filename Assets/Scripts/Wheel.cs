using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour {

    // Use this for initialization
    public float timeBetweenDrops;
    public float duration;
    private float timer;
    private float timerBetween;

    public Rigidbody bloodDrop;

	void Start () {       
        timerBetween = timeBetweenDrops;
        timer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (timer > 0.0f) timer -= Time.deltaTime;
        timerBetween -= Time.deltaTime;
        if (timerBetween <= 0 && timer > 0)
        {
            Instantiate(bloodDrop, new Vector3(transform.position.x,
            transform.position.y, transform.position.z), transform.rotation);
            timerBetween = timeBetweenDrops;
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "blood")
        {           
            timer = duration;
        }
    }
}
