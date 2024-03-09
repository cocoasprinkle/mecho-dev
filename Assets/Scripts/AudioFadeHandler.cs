using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFadeHandler : MonoBehaviour
{
    private AudioSource audioSource;
    private float fadeTime = 1.5f;
    private LoadManager loader;

    public bool fading = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        loader = GameObject.Find("LevelLoader").GetComponent<LoadManager>();
        loader.isLoading = false;
    }
    void FixedUpdate()
    {
        if (loader.isLoading && !fading)
        {
            StartCoroutine(AudioFade());
            Debug.Log(("Fading!"));
        }
    }
    private IEnumerator AudioFade()
    {
        fading = true;
        float startVolume = audioSource.volume;
 
        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
 
            yield return null;
        }
 
        audioSource.Stop ();
        audioSource.volume = startVolume;
    }
}
