using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSounds : MonoBehaviour {

    public AudioSource[] environmentAudios;
    public ScenarioSpawner scenarioSpawn;
    public Player player;
    public Light dayLight, hellLight;
    private int waterPlaying, roadPlaying, grassPlaying;
    private bool hellPlaying;
    // Use this for initialization
    void Start()
    {
        waterPlaying = 0;
        roadPlaying = 0;
        grassPlaying = 0;
        environmentAudios[0].Play();
        environmentAudios[1].Play();
        environmentAudios[2].Play();
        hellPlaying = false;        
        hellLight.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if ((player.getFloorMaterial() == "dirt" || player.getFloorMaterial() == "Lava")
            && !hellPlaying)
        {
            environmentAudios[3].Play();
            hellPlaying = true;
            dayLight.enabled = false;
            hellLight.enabled = true;
        }
    }

    public void updateEnvironment(float zPos)
    {

        waterPlaying = 0;
        roadPlaying = 0;
        grassPlaying = 0;

        for (int i = -30; i < 30; i += 2)
        {
            if ((zPos - i) >= 0)
            {
                if (scenarioSpawn.getFloorMaterial((int)zPos + i) == "water") waterPlaying++;
                else if (scenarioSpawn.getFloorMaterial((int)zPos + i) == "road") roadPlaying++;
                else if (scenarioSpawn.getFloorMaterial((int)zPos + i) == "grass") grassPlaying++;
            }
        }
        //Debug.Log(grassPlaying + " "+ (float)grassPlaying / 10);
        environmentAudios[0].volume = (float)grassPlaying / 5;
        environmentAudios[1].volume = (float)waterPlaying / 60;
        environmentAudios[2].volume = (float)roadPlaying / 30;

    }
}
