using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine;

public class ScreenUIManager : MonoBehaviour
{
    public GameObject calibrationCanvas;
    public Animator blackoutSphere;
    public Slider calibrateSlider;
    public Slider startSlider;
    public GameObject startText;
    public CanvasGroup uiGroup;
    public CanvasGroup goodEndingUI;
    public CanvasGroup badEndingUI;
    public CanvasGroup removeHeadsetMessage;
    public VideoPlayer titleVideo;
    public BikeMovement bikeMovement;
    public AudioManager audioManager;

    private bool gameStarted = false;

    public IEnumerator IntroLoop()
    {
        yield return new WaitUntil(() => titleVideo.time > 2);
        titleVideo.Pause();
        float startTime = 0f;
        while(startTime < 1)
        {
            startTime += Mathf.Clamp01(Time.deltaTime * 0.5f);
            uiGroup.alpha = startTime;
            yield return null;
        }
        yield return new WaitUntil(() => gameStarted);
        // calibrateSlider.gameObject.SetActive(false);
        // startSlider.gameObject.SetActive(false);
        uiGroup.alpha = 0;
        titleVideo.Play();
        audioManager.cycleAudioSource.Play();
        audioManager.VolumeRise();
        yield return new WaitUntil(() => titleVideo.time >= titleVideo.clip.length);
        titleVideo.gameObject.SetActive(false);
        bikeMovement.enabled = true;
        blackoutSphere.Play("FadeIn");
        audioManager.musicAudioSource.Play();
    }

    private IEnumerator GoodEndingRoutine()
    {
        float startTime = 0f;
        while(startTime < 1)
        {
            startTime += Mathf.Clamp01(Time.deltaTime * 0.2f);
            goodEndingUI.alpha = startTime;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        startTime = 0f;
        while(startTime < 1)
        {
            startTime += Mathf.Clamp01(Time.deltaTime * 0.5f);
            removeHeadsetMessage.alpha = startTime;
            yield return null;
        }
    }
    
    private IEnumerator BadEndingRoutine()
    {
        float startTime = 0f;
        while(startTime < 1)
        {
            startTime += Mathf.Clamp01(Time.deltaTime * 0.5f);
            badEndingUI.alpha = startTime;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        startTime = 0f;
        while(startTime < 1)
        {
            startTime += Mathf.Clamp01(Time.deltaTime * 0.5f);
            removeHeadsetMessage.alpha = startTime;
            yield return null;
        }
    }

    void Start()
    {
        StartCoroutine(IntroLoop());
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            StartGame();
        }    
    }

    public void StartGame()
    {
        gameStarted = true;
    }

    public void GoodEnding()
    {
        StartCoroutine(GoodEndingRoutine());
    }

    public void BadEnding()
    {
        StartCoroutine(BadEndingRoutine());
    }

    public void StartCalibration()
    {
        calibrationCanvas.SetActive(true);
    }

}
