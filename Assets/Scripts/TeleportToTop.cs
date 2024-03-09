using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToTop : MonoBehaviour
{
    public GameObject teleportToObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.position = new Vector3 (teleportToObject.transform.position.x, teleportToObject.transform.position.y, teleportToObject.transform.position.z);
        }
    }
}
