using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaTrapSc : MonoBehaviour {

    // Use this for initialization
    private Material material;
    public Renderer rend;
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
        material = rend.material;
        Vector4 color = material.GetColor("_Color");
        color.x = calcSin;
        material.SetColor("_Color", color);
        if (color.x >= 0.9) lc.move();
	}
}
