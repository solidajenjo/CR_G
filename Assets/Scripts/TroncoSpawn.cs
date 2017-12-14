using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroncoSpawn : MonoBehaviour {

    public Rigidbody tronco;
    public float minTime, maxTime;
    private float direction;
    private float timer;
    public float speed;
	// Use this for initialization
	void Start () {
        timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
		if (timer <= 0)
        {
            Rigidbody t;
            t = (Rigidbody)Instantiate(tronco, transform.position, transform.rotation);
            t.GetComponent<Tronco>().setDirection(direction);
            t.GetComponent<Tronco>().speed = this.speed;
            timer = Random.Range(minTime, maxTime);
        }
	}

    public void setDirection(float dir)
    {
        direction = dir;
    }
}
