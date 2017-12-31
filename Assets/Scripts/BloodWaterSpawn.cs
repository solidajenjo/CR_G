using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodWaterSpawn : MonoBehaviour {

    public float timeBetween;
    public WaterBlood waterBlood;
    private float timer;
	// Use this for initialization
	void Start () {
        timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
		if (timer <= 0)
        {
            Vector3 pos = transform.position;
            pos.y += 1.0f;
            Instantiate(waterBlood, pos, transform.rotation);
            timer = timeBetween;
        }
	}
}
