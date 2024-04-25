using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepHandler : MonoBehaviour
{
    private PlayerController pCon;
    private AudioSource audSource;

    // Start is called before the first frame update
    void Start()
    {
        pCon = GameObject.Find("Player").GetComponent<PlayerController>();
        audSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Checks if the player is on the ground and inputting a direction, while the audio source isn't already playing
        if (pCon.isOnGround && !pCon.noInput && !audSource.isPlaying)
        {
            audSource.Play();
        }
        else if (!pCon.isOnGround || pCon.noInput)
        {
            audSource.Stop();
        }
        // Pauses the audio source while paused
        if (pCon.isPaused)
        {
            audSource.Pause();
        }
        else
        {
            audSource.UnPause();
        }
    }
}
