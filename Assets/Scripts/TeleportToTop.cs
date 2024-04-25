using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToTop : MonoBehaviour
{
    public GameObject teleportToObject;
    private PlayerController pCon;
    // Start is called before the first frame update
    void Start()
    {
        pCon = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void OnCollisionEnter(Collision other)
    {
        // If the player collides with the teleporter object, they will be teleported to the defined destination object
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.position = new Vector3 (teleportToObject.transform.position.x, teleportToObject.transform.position.y, teleportToObject.transform.position.z);
            pCon.jumpCount = 0;
        }
    }
}
