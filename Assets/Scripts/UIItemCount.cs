using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIItemCount : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] TMP_Text text;
    public int totalItemCount;
    private GameObject[] getCount;
    public bool finished;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        getCount = GameObject.FindGameObjectsWithTag("Item");
        totalItemCount = getCount.Length;
    }

    // Update is called once per frame
    void Update()
    {
        // Sets a TMP text item to show the item count prefixed by a string detailing the variable and suffixed by the total amount of items in the scene
        text.text = ("Item Count: " + player.itemCount + "/" + totalItemCount);
        if (player.itemCount == totalItemCount && !finished)
        {
            finished = true;
            player.timer.timerActive = false;
            player.raceAudSource.PlayOneShot(player.raceEnd);
        }
    }
}
