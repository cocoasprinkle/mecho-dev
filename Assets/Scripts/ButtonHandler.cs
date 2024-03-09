using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    [Header("Button Types")]
    public bool nextButton;
    public bool startButton;
    public bool optionsButton;
    public bool backButton;
    public bool titleButton;
    
    private LoadManager loader;

    void Start()
    {
        Button b = gameObject.GetComponent<Button>();
        loader = GameObject.Find("LevelLoader").GetComponent<LoadManager>();
        if (nextButton)
        {
            b.onClick.AddListener(delegate() { loader.loadNext = true; });
        }
        if (startButton)
        {
            b.onClick.AddListener(delegate() { loader.loadStart = true; });
        }
        if (optionsButton)
        {
            b.onClick.AddListener(delegate() { loader.loadOptions = true; });
        }
        if (backButton)
        {
            b.onClick.AddListener(delegate() { loader.loadStored = true; });
        }
        if (titleButton)
        {
            b.onClick.AddListener(delegate() { loader.loadTitle = true; });
        }
    }
    void OnClick()
    {
        if (nextButton)
        {
            loader.loadNext = true;
            Debug.Log("Clicked next!");
        }
        else if (startButton)
        {
            loader.loadStart = true;
            Debug.Log("Clicked start!");
        }
        else if (optionsButton)
        {
            loader.loadOptions = true;
            Debug.Log("Clicked options!");
        }
        else if (backButton)
        {
            loader.loadStored = true;
            Debug.Log("Clicked back!");
        }
        else if (titleButton)
        {
            loader.loadTitle = true;
            Debug.Log("Clicked back!");
        }
    }
}
