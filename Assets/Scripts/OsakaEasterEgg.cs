using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OsakaEasterEgg : MonoBehaviour
{
    private AudioSource audSource;
    public GameObject timer;

    private UITimer timerScript;
    // Start is called before the first frame update
    void Start()
    {
        audSource = GetComponent<AudioSource>();
        timerScript = timer.GetComponent<UITimer>();
    }

    // Update is called once per frame
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided!");
        if (collision.gameObject.CompareTag("Player") && !audSource.isPlaying)
        {
            audSource.Play();
            timerScript.timerActive = false;
        }
    }
}
