using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour {

    public float bloodEffectDuration;
    private float timer;
	// Use this for initialization
	void Start () {
        timer = bloodEffectDuration;
	}
	
	// Update is called once per frame
	void Update () {
        if (timer > 0) timer -= Time.deltaTime;
        else this.tag = "kk";
	}
}
