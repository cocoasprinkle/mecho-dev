using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempoHandler : MonoBehaviour
{
    public float songBPM;
    private AudioSource audSource;
    public AudioClip beepSound;
    public AudioClip switchSound;
    public float fullBar;
    public float beepInterval;
    public int blockType;
    public bool beepInProgress;
    public bool barInProgress;
    // Start is called before the first frame update
    void Start()
    {
        // 240 divided by songBPM, for the full beat/bar (when the blocks switch)
        fullBar = 240 / songBPM;
        // 60 divided by songBPM, for every quarter section of a bar (every beat)
        beepInterval = 60 / songBPM;
        audSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!beepInProgress)
        {
            StartCoroutine(BeepHandler());
        }
        if (!barInProgress)
        {
            StartCoroutine(ChangeBlocks());
        }
    }

    IEnumerator BeepHandler()
    {
        beepInProgress = true;
        Debug.Log("Beep Interval = " + beepInterval);
        yield return new WaitForSeconds(beepInterval);
        audSource.PlayOneShot(beepSound, 0.5f);
        beepInProgress = false;
    }
    IEnumerator ChangeBlocks()
    {
        barInProgress = true;
        yield return new WaitForSeconds(fullBar);
        if (blockType == 0)
        {
            blockType = 1;
        }
        else if (blockType == 1)
        {
            blockType = 0;
        }
        Debug.Log("Block Type = " + blockType);
        audSource.PlayOneShot(switchSound, 0.5f);
        barInProgress = false;
    }
}
