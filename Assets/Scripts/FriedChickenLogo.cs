using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriedChickenLogo : MonoBehaviour {

    private float sinus;
    public float multiplier;
	// Use this for initialization
	void Start () {
        sinus = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        sinus += Time.deltaTime * 4;
        Vector3 rot = new Vector3(0.0f, 0.0f, Mathf.Sin(sinus));
        this.GetComponent<RectTransform>().Rotate(rot * multiplier);
	}
}
