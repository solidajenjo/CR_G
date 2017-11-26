using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humo : MonoBehaviour {

    public float duration;
    public float speed;
    private float timer;
	// Use this for initialization
	void Start () {
        timer = duration;
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(0.0f, speed * Time.deltaTime, 0.0f);
        transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime * 6, transform.localScale.y + Time.deltaTime * 6, transform.localScale.z + Time.deltaTime);
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
	}
}
