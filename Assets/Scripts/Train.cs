using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour {

    public float speed;
    public float timeToLive;
    private float timer;
	// Use this for initialization
	void Start () {
        timer = timeToLive;
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(speed * Time.deltaTime, 0.0f, 0.0f);
        timer -= Time.deltaTime;
        if (timer <= 0) Destroy(gameObject);
	}
}
