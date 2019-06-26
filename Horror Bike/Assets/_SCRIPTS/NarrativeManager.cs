using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeManager : MonoBehaviour
{
    public BikeMovement bikeMovement;
    public AudioManager audioManager;
    public ScreenUIManager screenUIManager;
    public ClimaxAdjust climax;
    public int choiceValue;
    public float speedModifier = 0.1f;

    public bool encounterActive = false;

    public Animator lightAnimCon;
    public Animator[] initialAnimations;
    public Animator[] badEndAnimCons;

    private IEnumerator InitialEncounterRoutine()
    {
        bikeMovement.vertInput = 0;
        audioManager.SlowMotorcycle();
        while(bikeMovement.curVelocity.magnitude > 0.0001f)
        {
            bikeMovement.handbrake = 0.2f;
            yield return null;
        }
        foreach(Animator anim in initialAnimations)
        {
            anim.Play("Encounter");
        }

        Debug.Log("Waiting for encounter to end.");
        yield return new WaitUntil(() => !encounterActive);
        audioManager.SpeedUpMotorcycle();
        audioManager.PlaySpookyLaugh();
        bikeMovement.vertInput = 2f;
        FindObjectOfType<GPSCanvasManager>().isChased = true;
        yield return new WaitForSeconds(2);
        bikeMovement.hasMinSpeed = true;
        bikeMovement.vertInput = 1f;
    }

    private IEnumerator GoodEndingRoutine()
    {
        bikeMovement.vertInput = 1;
        audioManager.FadeMusic();
        yield return new WaitForSecondsRealtime(2f);
        lightAnimCon.Play("GoodEnding");
        audioManager.GoodEnding();
        Debug.Log("Waiting for encounter to end.");
        yield return new WaitUntil(() => !encounterActive);
        bikeMovement.vertInput = 1.2f;
        screenUIManager.blackoutSphere.Play("FadeOut");
        yield return new WaitForSeconds(1.5f);
        screenUIManager.GoodEnding();
    }

    private IEnumerator BadEndingRoutine()
    {
        audioManager.FadeMusic();
        bikeMovement.hasMinSpeed = false;
        bikeMovement.vertInput = 0.6f;
        audioManager.SlowMotorcycle();
        lightAnimCon.Play("BadEnding");
        yield return new WaitUntil(() => encounterActive);
        bikeMovement.vertInput = 0;
        while(bikeMovement.curVelocity.magnitude > 0.005f)
        {
            Debug.Log("Bike velocity: " + bikeMovement.curVelocity.magnitude);
            bikeMovement.handbrake = 0.008f;
            yield return null;
        }
        audioManager.StopMotorcycle();
        Debug.Log("Playing final encounter anim.");
        
        foreach(Animator anim in badEndAnimCons)
        {
            anim.Play("Encounter");
        }
        yield return new WaitForSeconds(3.5f);
        screenUIManager.blackoutSphere.Play("CutToBlack");
        audioManager.PlaySpookyLaugh();
        screenUIManager.BadEnding();
    }

    // Start is called before the first frame update
    void Start()
    {
        bikeMovement = FindObjectOfType<BikeMovement>();
        audioManager = FindObjectOfType<AudioManager>();
        screenUIManager = FindObjectOfType<ScreenUIManager>();
    }

    public void BadChoice()
    {
        bikeMovement.vertInput = Mathf.Clamp01(bikeMovement.vertInput - speedModifier);
        choiceValue -= 1;
        climax.Adjust(choiceValue);
    }

    public void GoodChoice()
    {
        bikeMovement.vertInput = Mathf.Clamp01(bikeMovement.vertInput + speedModifier);
        choiceValue += 1;
        climax.Adjust(choiceValue);
    }

    public void InitialEncounter()
    {
        encounterActive = true;
        StartCoroutine(InitialEncounterRoutine());
    }

    public void GoodEnding()
    {
        encounterActive = true;
        StartCoroutine(GoodEndingRoutine());
    }

    public void BadEnding()
    {
        encounterActive = false;
        StartCoroutine(BadEndingRoutine());
    }

}
