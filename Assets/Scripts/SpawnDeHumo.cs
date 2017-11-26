using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDeHumo : MonoBehaviour {

    // Use this for initialization
    public float timeBetween;
    public Rigidbody humo;
    private float timer;
	void Start () {
        timer = timeBetween;
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
		if (timer <= 0)
        {
            timer = timeBetween;
            Instantiate(humo, transform.position, transform.rotation);
        }
	}
}
