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
        // Makes the object slowly spin in relation to the in-game time
        transform.Rotate (0, 25 * Time.deltaTime, 0);
    }

    void OnCollisionEnter(Collision collision)
    {
        // When the player collides with the object, all meshes in the array are disabled, the collect sound is played and the player's item count increases by 1
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
