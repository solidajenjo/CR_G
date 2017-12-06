using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    // Use this for initialization
    public Transform chickenTransform;
    public float zOffset;
    public float xOffset;
    public float speed;
    public float zInc;
    public float zOffsetMin, zOffsetMax;
    private float zMover;
    public Player player;
    private float scoreWatchDog;
	void Start () {
        scoreWatchDog = 10;
	}

    // Update is called once per frame
    void Update() {
        if (player.getScore() > 10 && zMover <= 0)
        {
            zOffset += Time.deltaTime * speed;
        }
        if (zMover > 0)
        {
            zOffset -= Time.deltaTime * speed * 4;
            zMover -= Time.deltaTime * 4;
        }
        if (player.getScore() > scoreWatchDog)
        {
            zMover = zInc;
            scoreWatchDog = player.getScore();
        }
        zOffset = Mathf.Clamp(zOffset, zOffsetMin, zOffsetMax);
        if (player.isDead()) return;
        transform.position = new Vector3(chickenTransform.position.x + xOffset, transform.position.y, 
            chickenTransform.position.z + zOffset);        
	}
}
