using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UITimer : MonoBehaviour
{
    [SerializeField] bool timerActive;
    [SerializeField] float curTime;
    [SerializeField] TMP_Text text;

    void Awake()
    {
        curTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive)
        {
            StartCoroutine(TimerTick());
        }
    }
    IEnumerator TimerTick()
    {
        yield return new WaitForSeconds(0.016f);
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
