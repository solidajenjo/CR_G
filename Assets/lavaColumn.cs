using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lavaColumn : MonoBehaviour {
    private float offset;
	// Use this for initialization
	void Start () {
        offset = Random.Range(0.0f, 1.0f);

    }
	
	// Update is called once per frame
	void Update () {
        
        transform.position = new Vector3(transform.position.x, Mathf.SmoothStep(-10.0f, 10.0f, Mathf.PingPong(Time.time/2 + offset, 1.0f)), transform.position.z);
    }
}
