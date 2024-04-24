using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHandler : MonoBehaviour
{
    public GameObject[] objMesh;
    public AudioClip collectSound;
    private AudioSource audSource;
    private BoxCollider col;
    private PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        audSource = GetComponent<AudioSource>();
        col = GetComponent<BoxCollider>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate (0, 25 * Time.deltaTime, 0);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < objMesh.Length; i++)
            {
                objMesh[i].gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
            col.enabled = false;
            audSource.PlayOneShot(collectSound);
            player.itemCount = player.itemCount + 1;
        }
    }
}
