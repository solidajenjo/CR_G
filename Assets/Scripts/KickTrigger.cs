using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickTrigger : MonoBehaviour {

    public Animator anim;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            anim.SetBool("kick", true);
            Debug.Log("KIKIN");
            Player player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
            player.recieveKick();
        }
    }
    
}
