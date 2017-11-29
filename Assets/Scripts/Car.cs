using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

    public int speed;
    public float timeToLive;
    public float timer;

	// Use this for initialization
	void Start () {
        timer = timeToLive;
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(0.0f, 0.0f, speed * Time.deltaTime);
        timer -= Time.deltaTime;
        if (timer <= 0) Destroy(gameObject);
	}

    void OnTriggerEnter(Collider other)
    {
        //Morir si es el pollo
    }
}
