using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBlood : MonoBehaviour {

    public float duration;
    public float speed;
    private float timer;
	// Use this for initialization
	void Start () {
        timer = duration + Random.Range(0.0f, 2.0f);
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0) Destroy(gameObject);
        transform.Translate(speed * Time.deltaTime, 0.0f, 0.0f);
	}
}
