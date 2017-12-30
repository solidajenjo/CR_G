using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickTrigger : MonoBehaviour {

    public Animator anim;
    public lavaColumn lc;
    public AudioSource audio;
    private bool onlyOneKick;
    // Use this for initialization
    void Start()
    {
        onlyOneKick = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !onlyOneKick)
        {
            onlyOneKick = true;
            anim.SetBool("kick", true);
            lc.move();
            lc.isAngel = true;
            audio.Play();
            Player player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
            player.recieveKick();
        }
    }
    
}
