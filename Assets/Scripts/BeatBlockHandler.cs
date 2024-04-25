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
        // Keeps track of when the player is colliding with the beat block
        if (collision.gameObject.CompareTag("Player"))
        {
            collidingWithPlayer = true;
        }
    }
    void OnCollisionExit()
    {
        // Stops the player from keeping the grounded state when a beat block disables below
        collidingWithPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        // When the tempo handler changes which set of blocks is visible, this checks which set the corresponding block fits into and then changes the state of the mesh renderer
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
