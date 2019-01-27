using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnyKeyToContinue : MonoBehaviour
{
    public static AnyKeyToContinue Instance;
    bool doOnce = true;

    void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    void Start()
    {
        FadeManager.Instance.FadeIn();
    }
    void Update()
    {
        if (doOnce)
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                FadeManager.Instance.FadeOut(LoadNextScene);
            }
        }
    }

    public void ChangeLevel(int? id = null)
    {
        FadeManager.Instance.FadeOut(LoadNextScene);
    }

    void LoadNextScene()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        SceneManager.LoadScene(
            (SceneManager.GetActiveScene().buildIndex + 1 > SceneManager.sceneCountInBuildSettings - 1)
                ? 0
                : SceneManager.GetActiveScene().buildIndex + 1
        );

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        doOnce = false;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        FadeManager.Instance.FadeIn();
        doOnce = true;
    }
}