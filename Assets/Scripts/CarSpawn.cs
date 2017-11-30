using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawn : MonoBehaviour
{

    public Car car;
    public float timeBetween;
    private float timer;
    public int speed;


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
            timer = Random.Range(1.0f, timeBetween);
            Car c;
            c = Instantiate<Car>(car, transform.position, transform.rotation);
            c.speed = this.speed;
        }
    }
}
