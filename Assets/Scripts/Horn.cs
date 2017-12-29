using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horn : MonoBehaviour {

    public AudioSource[] swearingWords;
    public AudioSource horn;
    public bool isWagon;
    public Animator anim;
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
            Debug.Log(swearingWords);
            if (player.canBeSweared() && !isWagon)
            {
                player.getSweared();
                Debug.Log("SWEAR");
                int swear = Random.Range(0, swearingWords.Length);
                swearingWords[swear].Play();
            }else if (isWagon)
            {
                anim.Play("laugh");
            }
        }
    }
}
