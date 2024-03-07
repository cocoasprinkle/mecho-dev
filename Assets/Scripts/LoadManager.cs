using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public Animator transition;
    private float loadTime = 1.8f;

    // Start is called before the first frame update
    void Awake()
    {
        transition.SetTrigger("In");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
        }
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Out");
        yield return new WaitForSeconds(loadTime);
        SceneManager.LoadScene(levelIndex);
    }
}
