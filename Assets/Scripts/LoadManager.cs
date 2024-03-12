using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public Animator transition;
    private float loadTime = 1.8f;

    public bool reload = false;
    public bool loadNext = false;
    public bool loadStart = false;
    public bool loadPrev = false;
    public bool loadStored = false;
    public bool loadTitle = false;
    public bool loadOptions = false;
    public bool isLoading = true;

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
        buffer = true;
        yield return new WaitForSeconds(waitForTitle);
        transition.SetTrigger("In");

        yield return new WaitForSeconds(1.5f);
        buffer = false;
    }

    void Update()
    {
        if (Input.GetButton("Reset") && SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1)
        {
            reload = true;
        }
        if (Input.GetButton("Options Menu") && SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1)
        {
            loadOptions = true;
        }
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
            StartCoroutine(LoadLevel(2));
        }
        if (loadPrev && !isLoading && !buffer)
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 1));
        }
        if (loadTitle && !isLoading && !buffer)
        {
            StartCoroutine(LoadLevel(0));
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
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Out");
        isLoading = true;
        yield return new WaitForSeconds(loadTime);
        SceneManager.LoadScene(levelIndex);
    }

    IEnumerator LoadBuffer()
    {
        buffer = true;
        yield return new WaitForSeconds(1.5f);
        isLoading = false;
        buffer = false;
    }
}
