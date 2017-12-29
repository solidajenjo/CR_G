using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaTrapSc : MonoBehaviour {

    // Use this for initialization
    public Material material;
    private float timer;
    public float amplitude;
    private float sinus;
    public lavaColumn lc;

	void Start () {
        sinus = Random.Range(0.0f, 10.0f);
	}
	
	// Update is called once per frame
	void Update () {
        sinus += Time.deltaTime;
        float calcSin = Mathf.Sin(sinus);
        Vector4 color = material.GetColor("_Color");
        color.x = calcSin;
        material.SetColor("_Color", color);
        if (calcSin >= 0.9) lc.move();
	}
}
