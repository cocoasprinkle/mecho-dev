using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepHandler : MonoBehaviour
{
    private PlayerController pCon;
    [Header("Grass Footstep Sounds")]
    [SerializeField] AudioClip stepGrass;
    [SerializeField] AudioClip runStepGrass;
    [SerializeField] AudioClip walkStepGrass;
    [Header("Metal Footstep Sounds")]
    [SerializeField] AudioClip stepMetal;
    [SerializeField] AudioClip runStepMetal;
    [SerializeField] AudioClip walkStepMetal;

    private AudioSource audSource;

    // Start is called before the first frame update
    void Start()
    {
        pCon = GameObject.Find("Player").GetComponent<PlayerController>();
        audSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grass"))
        {
            Debug.Log("GRASS GRASS GRASS");
            if (pCon.curSpeed > 15 && !audSource.isPlaying)
            {
                audSource.PlayOneShot(runStepGrass);
            }
            else if (pCon.curSpeed > 0.5 && !audSource.isPlaying)
            {
                audSource.PlayOneShot(walkStepGrass);
            }
            else
            {
                audSource.PlayOneShot(stepGrass);
            }
        }
        else if (collision.gameObject.CompareTag("Metal"))
        {
            if (pCon.curSpeed > 15 && !audSource.isPlaying)
            {
                audSource.PlayOneShot(runStepMetal);
            }
            else if (pCon.curSpeed > 0.5 && !audSource.isPlaying)
            {
                audSource.PlayOneShot(walkStepMetal);
            }
            else
            {
                audSource.PlayOneShot(stepMetal);
            }
        }
    }
    void OnCollisionEnter (Collision collision)
    {
        audSource.Play();
        Debug.Log("Audio Source is working fine");
    }
    void OnCollisionExit()
    {
        audSource.Stop();
    }
}
