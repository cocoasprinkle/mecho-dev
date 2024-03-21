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
        if (pCon.isOnGround && !pCon.noInput && !audSource.isPlaying)
        {
            audSource.Play();
        }
        else if (!pCon.isOnGround || pCon.noInput)
        {
            audSource.Stop();
        }

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
