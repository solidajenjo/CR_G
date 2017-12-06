using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAnimator : MonoBehaviour {

    public float amplitude;
    private float sinus;
    private float randomOffset;
	// Use this for initialization
	void Start () {
        randomOffset = Random.Range(0.0f, 10.0f);
	}
	
	// Update is called once per frame
	void Update () {
        sinus = Mathf.Sin(Time.time + randomOffset) * amplitude + 1.0f;
        Debug.Log("sin "+sinus.ToString());
        transform.localScale = new Vector3(transform.localScale.x * (sinus), 
            transform.localScale.y * (sinus), transform.localScale.z);
        transform.Rotate(new Vector3(0.0f, 0.0f, (1.0f - sinus) * 100.0f));
	}
}
