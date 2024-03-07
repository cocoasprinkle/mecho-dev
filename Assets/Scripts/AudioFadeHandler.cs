using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFadeHandler : MonoBehaviour
{
    private AudioSource audioSource;
    private float fadeTime = 1.5f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(AudioFade());
        }
    }
    private IEnumerator AudioFade()
    {
        float startVolume = audioSource.volume;
 
        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
 
            yield return null;
        }
 
        audioSource.Stop ();
        audioSource.volume = startVolume;
    }
}
