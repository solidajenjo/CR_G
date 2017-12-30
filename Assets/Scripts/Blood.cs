using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{

    public float bloodEffectDuration;
    private float timer;
    public BloodWaterSpawn bloodWaterSpawn;
    private Dictionary<float, bool> pointSpawn;
    // Use this for initialization
    void Start()
    {
        pointSpawn = new Dictionary<float, bool>();
        timer = bloodEffectDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0) timer -= Time.deltaTime;
        else this.tag = "kk";
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Vector3 contactPoint = contact.point;
            contactPoint.z = Mathf.Round(contactPoint.z);
            if (collision.gameObject.name == "Water(Clone)" && this.tag != "kk"
                && !pointSpawn.ContainsKey(contactPoint.z))
            {
                contactPoint.y += 1.0f;               
                pointSpawn.Add(contactPoint.z, true);
                Instantiate(bloodWaterSpawn, contactPoint, Quaternion.Euler(0, 0, 0));
                Debug.Log(Time.time + " " + contactPoint);
            }
        }
    }
}