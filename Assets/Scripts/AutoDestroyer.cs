using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyer : MonoBehaviour {

    public Transform playerTrans;
    public float distanceToDestroy;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        try {
            playerTrans = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Transform>();
            bool isDead = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>().isDead();
            if (Vector3.Distance(transform.position, playerTrans.transform.position) > distanceToDestroy * 2
                && transform.position.z < playerTrans.transform.position.z
                && !isDead) Destroy(this.gameObject);
        }
        catch
        {

        }
	}
}
