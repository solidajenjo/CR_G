using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeExplosion : MonoBehaviour {

    public NukeParticle nukeParticlePrefab;
    public GameObject friedChicken;
    private float timer; 
	// Use this for initialization
	void Start () {
        friedChicken.SetActive(false);
        timer = 2.0f;
        for (int i = 0; i < 90; ++i)
        {
            NukeParticle part = Instantiate(nukeParticlePrefab, transform.position, transform.rotation);
            part.setDegree(i * 4);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (timer > 0) timer -= Time.deltaTime;
        else
        {
            friedChicken.SetActive(true);
        }
	}
}
