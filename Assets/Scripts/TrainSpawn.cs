using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSpawn : MonoBehaviour {

    public Rigidbody train;
    public float timeBetween;
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
            timer = timeBetween;
            Instantiate(train, transform.position, transform.rotation);
        }
	}
}
