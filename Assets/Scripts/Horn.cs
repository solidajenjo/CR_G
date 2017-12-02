using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horn : MonoBehaviour {

    public AudioSource[] swearingWords;
    public AudioSource horn;
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            horn.Play();            
            Player player = other.GetComponent<Player>();
            if (player.canBeSweared())
            {
                player.getSweared();
                int swear = Random.Range(0, swearingWords.Length);
                swearingWords[swear].Play();
            }            
        }
    }
}
