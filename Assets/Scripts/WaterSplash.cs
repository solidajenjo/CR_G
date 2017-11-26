using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSplash : MonoBehaviour {

    public int amount;
    public Rigidbody waterDrop;
    public float delay;
    private float timer;
    private int howManyLaunched;
	// Use this for initialization
	void Start () {
        howManyLaunched = 0;
        timer = 0;
    }
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Instantiate(waterDrop, transform.position, transform.rotation);
            timer = delay;
            howManyLaunched++;
        }
        if (howManyLaunched > amount) Destroy(gameObject);
	}
}
