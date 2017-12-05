using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plainChicken : MonoBehaviour {

    public GameObject blood;
    public float speed, duration;
    public Transform transf;
    private float timer;
	// Use this for initialization
	void Start () {
        timer = duration;
        transf.rotation = Quaternion.Euler(new Vector3(0.0f, Random.Range(0.0f, 180.0f), 0.0f));
	}
	
	// Update is called once per frame
	void Update () {
        if (timer > 0)
        {
            blood.transform.localScale = new Vector3(blood.transform.localScale.x + speed * Time.deltaTime,
                blood.transform.localScale.y, blood.transform.localScale.z + speed * Time.deltaTime);
            timer -= Time.deltaTime;
        }
	}
}
