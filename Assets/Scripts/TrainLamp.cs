using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainLamp : MonoBehaviour {

    public MeshRenderer lightRenderer;
    public float duration;
    private float timer;
	// Use this for initialization
	void Start () {
        lightRenderer.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0) lightRenderer.enabled = false;
	}

    public void setLightOn()
    {
        lightRenderer.enabled = true;
        timer = duration;
    }
}
