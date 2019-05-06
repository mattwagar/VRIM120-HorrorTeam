using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    private bool sceneLoaded;
    private bool sceneUnloaded;

    private IEnumerator LoadSceneRoutine(string currentScene, string sceneToLoad, LoadSceneMode mode)
    {
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadSceneAsync(sceneToLoad, mode);
        yield return new WaitUntil(() => sceneLoaded);
        sceneLoaded = false;

        SceneManager.UnloadSceneAsync(currentScene);
        yield return new WaitUntil(() => sceneUnloaded);
        sceneUnloaded = false;
        currentScene = sceneToLoad;
    }

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sceneLoaded = true;
        Debug.Log("SceneLoaded: " + scene.name);
    }

    void OnSceneUnloaded(Scene scene)
    {
        sceneUnloaded = true;
        Debug.Log("SceneUnloaded: " + scene.name);
    }

    public void LoadScene(string currentScene, string sceneToLoad, LoadSceneMode mode = LoadSceneMode.Additive)
    {
        StartCoroutine(LoadSceneRoutine(currentScene, sceneToLoad, mode));
    }

    private void SetPlayerSpawn(string sceneName)
    {

    }

    private void SetPlayerPosition(string newScene)
    {

    }

}
