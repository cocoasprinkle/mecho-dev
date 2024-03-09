using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    [Header("Button Types")]
    public bool NextButton;
    
    private LoadManager loader;

    void Start()
    {
        Button b = gameObject.GetComponent<Button>();
        loader = GameObject.Find("LevelLoader").GetComponent<LoadManager>();
        if (NextButton)
        {
            b.onClick.AddListener(delegate() { loader.loadNext = true; });
        }
    }
    void OnClick()
    {
        if (NextButton)
        {
            loader.loadNext = true;
            Debug.Log("Clicked start!");
        }
    }
}
