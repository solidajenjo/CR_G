using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrop : MonoBehaviour {

    public Rigidbody rigidBody;
    public float initForce;
	// Use this for initialization
	void Start () {
        //Vector3 direction = new Vector3(Random.Range(-2.0f, 2.0f), 10.0f, Random.Range(-2.0f, 2.0f));
        Vector3 direction = transform.up * Random.Range(initForce - 2, initForce + 2);
        direction.x = Random.Range(-1.6f, 1.6f);
        direction.z = Random.Range(-1.6f, 1.6f);
        rigidBody.velocity = direction;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
