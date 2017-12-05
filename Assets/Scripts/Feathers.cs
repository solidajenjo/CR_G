using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feathers : MonoBehaviour {

    // Use this for initialization
    public float speedDown;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(new Vector3(0.0f, speedDown * Time.deltaTime, 0.0f));
	}
}
