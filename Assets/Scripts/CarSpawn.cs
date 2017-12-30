using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawn : MonoBehaviour
{

    public Car[] car;
    public int type;
    public float timeBetween;
    private float timer;
    public int speed;
    public bool isRight;

    // Use this for initialization
    void Start()
    {
        timer = Random.Range(1.0f, timeBetween);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = Random.Range(2.0f, timeBetween);            
            Vector3 newPos = transform.position;
            //newPos.y = 12.0f;
            Car c = null;
            int which = Random.Range(0, car.Length);
            c = (Car)Instantiate(car[which], newPos, transform.rotation);
            c.speed = this.speed;
            if (type == 2 && which == 1 && !isRight) c.flip();            
        }
    }
}
