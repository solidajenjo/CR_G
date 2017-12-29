using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lavaColumn : MonoBehaviour {
    private float offset;
    private float timer;
    public float duration;
    public float speed;
    private bool bajando;
    private bool stoped;
    private float z;
	// Use this for initialization
	void Start () {
        bajando = false;
        stoped = true;
        z = transform.localPosition.z;
    }
	
	// Update is called once per frame
	void Update () {
        if (stoped) return;
        else
        {
            if (!bajando) transform.Translate(new Vector3(0.0f, 0.0f, speed * Time.deltaTime));
            else transform.Translate(new Vector3(0.0f, 0.0f, -speed * Time.deltaTime));
        }
        timer -= Time.deltaTime;
        if (timer <= 0 && !bajando)
        {
            bajando = true;
            Debug.Log("BAJ "+ transform.localPosition.z+" "+z);
        }
        else if (timer <= 0 && transform.localPosition.z >= z)
        {
            bajando = false;
            stoped = true;
        }
    }                


    public void move()
    {
        timer = duration;
        stoped = false;
    }
}
