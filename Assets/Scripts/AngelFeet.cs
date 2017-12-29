using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelFeet : MonoBehaviour {

    public Animator anim;
        
	// Use this for initialization
	void Start () {
        anim.SetBool("prepare", false);
        anim.SetBool("kick", false);
        //anim.SetTrigger("toIdle");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("other.tag"+"ANGEL");
        if (other.tag == "Player")
        {
            anim.SetBool("prepare", true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            anim.SetBool("prepare", false);
        }
    }
}
