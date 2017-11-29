using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawn : MonoBehaviour
{

    public Rigidbody car;
    public float timeBetween;
    private float timer;


    // Use this for initialization
    void Start()
    {
        timer = Random.Range(0.0f, timeBetween);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = Random.Range(0.0f, timeBetween);
            Instantiate(car, transform.position, transform.rotation);
        }
    }
}
