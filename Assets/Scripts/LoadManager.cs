using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public Animator transition;
    private float loadTime = 1.8f;

    // List of different loading transitions
    public bool reload = false;
    public bool loadNext = false;
    public bool loadStart = false;
    public bool loadPrev = false;
    public bool loadStored = false;
    public bool loadTitle = false;
    public bool loadOptions = false;
    public bool loadTest = false;
    public bool loadCredits = true;
    public bool isLoading = true;
    public GameObject player;

    public bool buffer = true;

    public float waitForTitle = 1.3f;

    public int storedScene;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            transition.SetTrigger("In");
            StartCoroutine(LoadBuffer());
        }
        else
        {
            // Title screen loading transition
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Application.targetFrameRate = 120;
            StartCoroutine(TitleBuffer());
        }

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        storedScene = (PlayerPrefs.GetInt("StoredScene"));
    }

    IEnumerator TitleBuffer()
    {
        // Buffers the beginning of the opening transition on the title screen
        buffer = true;
        yield return new WaitForSeconds(waitForTitle);
        transition.SetTrigger("In");

        yield return new WaitForSeconds(1.5f);
        buffer = false;
    }

    void Update()
    {
        // If certain buttons are presssed, these bools are set to true
        if (Input.GetButton("Reset") && SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1)
        {
            reload = true;
        }
        if (Input.GetButton("Test Bind") && SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1)
        {
            if (SceneManager.GetActiveScene().buildIndex != 4)
            {
                loadTest = true;
            }
            if (SceneManager.GetActiveScene().buildIndex == 4)
            {
                loadStart = true;
            }
        }
        // Starts a coroutine with different scene arguments corresponding to the transition requested
        if (reload && !isLoading && !buffer)
        {
             StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
        }
        if (loadNext && !isLoading && !buffer)
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }
        if (loadStart && !isLoading && !buffer)
        {
            StartCoroutine(LoadLevel(3));
        }
        if (loadPrev && !isLoading && !buffer)
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 1));
        }
        if (loadTitle && !isLoading && !buffer)
        {
            StartCoroutine(LoadLevel(0));
        }
        if (loadCredits && !isLoading && !buffer)
        {
            StartCoroutine(LoadLevel(2));
        }
        if (loadOptions && !isLoading && !buffer)
        {
            storedScene = SceneManager.GetActiveScene().buildIndex;
            PlayerPrefs.SetInt("StoredScene", storedScene);
            StartCoroutine(LoadLevel(1));
        }
        if (loadStored && !isLoading && !buffer)
        {
            StartCoroutine(LoadLevel(storedScene));
        }
        if (loadTest && !isLoading && !buffer)
        {
            StartCoroutine(LoadLevel(4));
        }
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        // Buffers the scene loading until the wipe out transition has finished
        Time.timeScale = 1;
        transition.SetTrigger("Out");
        isLoading = true;
        yield return new WaitForSeconds(loadTime);
        SceneManager.LoadScene(levelIndex);
    }

    IEnumerator LoadBuffer()
    {
        // Enables a buffer during the initial wipe in transition, to make sure that no transition start until the current one finishes
        buffer = true;
        yield return new WaitForSeconds(1.5f);
        isLoading = false;
        buffer = false;
    }
}
