using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UITimer : MonoBehaviour
{
    [SerializeField] public bool timerActive;
    [SerializeField] float curTime;
    [SerializeField] TMP_Text text;

    private PlayerController pCon;

    void Awake()
    {
        curTime = 0;
        pCon = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive && pCon.canInput)
        {
            StartCoroutine(TimerTick());
        }
    }
    IEnumerator TimerTick()
    {
        // Handles each tick of the timer, the WaitForSeconds return value is just a hasty way to give a return value every time the tick is ran
        yield return new WaitForSeconds(0f);
        curTime = curTime + Time.deltaTime;
        TimeSpan time = TimeSpan.FromSeconds(curTime);
        if (time.Hours > 0)
        {
            text.text = String.Format(@"{0:hh\:mm\:ss\.ff}", time);
        }
        else
        {
            text.text = String.Format(@"{0:mm\:ss\.ff}", time);
        }
    }
}
