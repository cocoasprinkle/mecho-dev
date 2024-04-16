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

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        getCount = GameObject.FindGameObjectsWithTag("Item");
        totalItemCount = getCount.Length;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = ("Item Count: " + player.itemCount + "/" + totalItemCount);
    }
}
