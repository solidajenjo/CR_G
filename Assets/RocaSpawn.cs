using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocaSpawn : MonoBehaviour {

    public Rigidbody roca, nube;
    public float minTime, maxTime;
    private float direction;
    private float timer;
    public int speed;
    public bool isCloud;
    // Use this for initialization
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Rigidbody t;
            if (!isCloud) t = (Rigidbody)Instantiate(roca, transform.position, transform.rotation);
            else t = (Rigidbody)Instantiate(nube, transform.position, transform.rotation);
            t.GetComponent<Roca>().setDirection(direction);
            t.GetComponent<Roca>().speed = this.speed;
            timer = Random.Range(minTime, maxTime);
        }
    }

    public void setDirection(float dir)
    {
        direction = dir;
    }
}
