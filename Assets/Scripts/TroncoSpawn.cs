using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroncoSpawn : MonoBehaviour {

    public Rigidbody tronco;
    public float minTime, maxTime;
    private float timer;
	// Use this for initialization
	void Start () {
        timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
		if (timer <= 0)
        {
            Instantiate(tronco, transform.position, transform.rotation);
            timer = Random.Range(minTime, maxTime);
        }
	}
}
