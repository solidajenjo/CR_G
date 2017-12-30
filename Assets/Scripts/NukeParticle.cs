using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeParticle : MonoBehaviour {

    public float height;
    public int degree;
    public float speed;
    public float speed2;
    public float expansionMultiplier;
    public float duration;
    private float timer;
    private bool expansion;
	// Use this for initialization
	void Start () {
        expansion = false;
        timer = duration;
        height += transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0) Destroy(gameObject);
        if (transform.position.y < height)
        {
            transform.Translate(new Vector3(0.0f, speed * Time.deltaTime, 0.0f));
        }
        else {
            expansion = true;
            transform.rotation = Quaternion.Euler(new Vector3(0.0f, degree, 0.0f));
        }
        if (expansion)
        {
            //speed2 *= expansionMultiplier;
            transform.Translate(transform.forward * speed2 * Time.deltaTime);
            if (timer < 3.0)
            {
                Vector3 expansionVector = new Vector3(transform.localScale.x * expansionMultiplier * Time.deltaTime,
                    transform.localScale.y * expansionMultiplier * Time.deltaTime,
                    transform.localScale.z * expansionMultiplier * Time.deltaTime);
                transform.localScale = transform.localScale + expansionVector;
            }
        }
	}

    public void setDegree(int degree)
    {
        this.degree = degree;
    }
}
