using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatBlockHandler : MonoBehaviour
{
    private MeshRenderer meshMat;
    private BoxCollider col;
    public TempoHandler tempo;
    private PlayerController player;
    public bool collidingWithPlayer;
    // Start is called before the first frame update
    void Start()
    {
        meshMat = GetComponent<MeshRenderer>();
        tempo = GameObject.Find("Beat Blocks").GetComponent<TempoHandler>();
        col = GetComponent<BoxCollider>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collidingWithPlayer = true;
        }
    }
    void OnCollisionExit()
    {
        collidingWithPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (tempo.blockType == 0)
        {
            if (gameObject.tag == "Blue Block")
            {
                meshMat.enabled = true;
                col.enabled = true;

            }
            else if (gameObject.tag == "Pink Block")
            {
                meshMat.enabled = false;
                col.enabled = false;
                if (collidingWithPlayer)
                {
                    player.isOnGround = false;
                    collidingWithPlayer = false;
                }
            }
        }
        else if (tempo.blockType == 1)
        {
            if (gameObject.tag == "Blue Block")
            {
                meshMat.enabled = false;
                col.enabled = false;
                if (collidingWithPlayer)
                {
                    player.isOnGround = false;
                    collidingWithPlayer = false;
                }
            }
            else if (gameObject.tag == "Pink Block")
            {
                meshMat.enabled = true;
                col.enabled = true;
            }
        }
    }
}
