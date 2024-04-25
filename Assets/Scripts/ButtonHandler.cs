using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    [Header("Button Types")]
    public bool nextButton;
    public bool startButton;
    public bool optionsButton;
    public bool backButton;
    public bool titleButton;
    public bool creditsButton;
    public bool reloadButton = false;

    private LoadManager loader;
    private PlayerController pCon;

    void Start()
    {
        Button b = gameObject.GetComponent<Button>();
        loader = GameObject.Find("LevelLoader").GetComponent<LoadManager>();
        if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1 && SceneManager.GetActiveScene().buildIndex != 2)
        {
            pCon = GameObject.Find("Player").GetComponent<PlayerController>();
        }
        // Adds listeners to buttons corresponding to load manager variables, so clicking on them will cause specific loading transitions to be triggered
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
        if (creditsButton)
        {
            b.onClick.AddListener(delegate () { loader.loadCredits = true; });
        }
        if (backButton)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 1)
            {
                b.onClick.AddListener(delegate () { loader.loadStored = false; });
            }
            else
            {
                b.onClick.AddListener(delegate () { pCon.isPaused = false; });
            }
        }
        if (titleButton)
        {
            b.onClick.AddListener(delegate () { loader.loadTitle = true; });
        }
        if (reloadButton)
        {
            b.onClick.AddListener(delegate () { loader.reload = true; });
        }
    }
    void OnClick()
    {
        // On click, the bools are set to true
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
            pCon.isPaused = false;
            Debug.Log("Clicked back!");
        }
        else if (titleButton)
        {
            loader.loadTitle = true;
            Debug.Log("Clicked title!");
        }
        else if (creditsButton)
        {
            loader.loadCredits = true;
            Debug.Log("Clicked credits!");
        }
        else if (reloadButton)
        {
            loader.reload = true;
            Debug.Log("Clicked reload!");
        }
    }
}
