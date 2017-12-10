using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CameraScript : MonoBehaviour {

    // Use this for initialization
    public Transform chickenTransform;
    public float zOffset;
    public float xOffset;
    public float speed;
    public float zInc;
    public float zOffsetMin, zOffsetMax;
    public float xMin, xMax, xExtra;
    private float zMover;
    public Player player;
    private float scoreWatchDog;
    public Text tip;
    void Start () {
        scoreWatchDog = 3;
        tip.gameObject.SetActive(false);
	}

    // Update is called once per frame
    void Update() {
        if (player.getScore() > 3 && zMover <= 0 && !player.isDead())
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
        //if (player.isDead()) return;
        float xCam = Mathf.Clamp(chickenTransform.position.x, xMin, xMax);
        transform.position = new Vector3(xCam + xOffset, transform.position.y, 
            chickenTransform.position.z + zOffset);
        if ((zOffset >= zOffsetMax || chickenTransform.position.x < xMin - xExtra || 
            chickenTransform.position.x > xMax + xExtra + 10.0f) && !player.isDead())
        {
            player.setNuked();
            zMover = zInc * 2;
        }
    }

    public void rewind()
    {
        zMover = zInc * 2;
    }
}
