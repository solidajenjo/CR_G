using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

    public int speed;
    public float timeToLive;
    public float timer;
    public Material[] materials;
    public GameObject[] carParts;

	// Use this for initialization
	void Start () {
        timer = timeToLive;
        //Randomize materials
        int amount = materials.Length;
        int parts = carParts.Length;
        for (int i = 0; i < parts; ++i)
        {
            Renderer[] renderer = carParts[i].GetComponents<Renderer>();
            renderer[0].material = materials[Random.Range(0, amount - 1)];
        }
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(speed * Time.deltaTime, 0.0f, 0.0f);
        timer -= Time.deltaTime;
        if (timer <= 0) Destroy(gameObject);
	}

    public void flip()
    {
        transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
        speed = -speed;
    }

}
