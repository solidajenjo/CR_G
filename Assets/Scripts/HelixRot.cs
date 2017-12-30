using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixRot : MonoBehaviour {

    public float speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(speed * Time.deltaTime, 0.0f, 0.0f));
	}
}
