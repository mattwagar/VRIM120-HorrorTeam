using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public string environmentScene = "Environment";
    public string playerScene = "PlayerController";

    public bool reload = false;
    public Vector3 startingPosition = new Vector3(0f, 2.5f, 0f);

    private bool sceneLoaded;
    private bool sceneUnloaded;
    private BikeMovement bikeMovement;

    private IEnumerator ReloadGameRoutine()
    {
        yield return new WaitUntil(() => reload);
        // bikeMovement.enabled = false;
        // bikeMovement.bikeRigidBody.velocity = Vector3.zero;
        sceneUnloaded = false;
        SceneManager.UnloadSceneAsync(environmentScene);
        yield return new WaitUntil(() => sceneUnloaded);
        Debug.Log("Unloaded Env");
        sceneUnloaded = false;
        SceneManager.UnloadSceneAsync(playerScene);
        yield return new WaitUntil(() => sceneUnloaded);
        sceneUnloaded = false;
        Debug.Log("Unloaded Player");
        SceneManager.LoadSceneAsync(environmentScene);
        yield return new WaitUntil(() => sceneLoaded);
        sceneLoaded = false;
        Debug.Log("Loaded Env");
        Destroy(this);
        // bikeMovement.gameObject.transform.position = startingPosition;
    }

    private IEnumerator LoadSceneRoutine(string sceneToLoad, LoadSceneMode mode)
    {
        SceneManager.LoadSceneAsync(sceneToLoad, mode);
        yield return new WaitUntil(() => sceneLoaded);
        sceneLoaded = false;
        bikeMovement = FindObjectOfType<BikeMovement>();
        StartCoroutine(ReloadGameRoutine());
    }

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        LoadScene("PlayerController");
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

    public void LoadScene(string sceneToLoad, LoadSceneMode mode = LoadSceneMode.Additive)
    {
        StartCoroutine(LoadSceneRoutine(sceneToLoad, mode));
    }

    public void ReloadGame()
    {
        StartCoroutine(ReloadGameRoutine());
    }

}
