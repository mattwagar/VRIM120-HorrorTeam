using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeManager : MonoBehaviour
{
    public BikeMovement bikeMovement;
    public AudioManager audioManager;
    public ClimaxAdjust climax;
    public int choiceValue;
    public float speedModifier = 0.1f;

    public bool encounterActive = false;

    public Animator[] initialAnimations;
    public Animator badEndAnimCon;

    private IEnumerator InitialEncounterRoutine()
    {
        bikeMovement.vertInput = 0;
        while(bikeMovement.curVelocity.magnitude > .0001)
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
        bikeMovement.vertInput = 2f;
        FindObjectOfType<GPSCanvasManager>().isChased = true;
        yield return new WaitForSeconds(2);
        bikeMovement.hasMinSpeed = true;
        bikeMovement.vertInput = 1f;
    }

    private IEnumerator GoodEndingRoutine()
    {
        bikeMovement.vertInput = 1;
        Debug.Log("Waiting for encounter to end.");
        yield return new WaitUntil(() => !encounterActive);
        bikeMovement.vertInput = 2f;
        yield return new WaitForSeconds(2);
        bikeMovement.hasMinSpeed = true;
        bikeMovement.vertInput = 1f;
    }

    private IEnumerator BadEndingRoutine()
    {
        // initialAnimation.Play("Encounter");
        bikeMovement.vertInput = 0.5f;
        yield return new WaitUntil(() => encounterActive);
        bikeMovement.vertInput = 0;
        while(bikeMovement.curVelocity.magnitude > .0001)
        {
            bikeMovement.handbrake = 0.2f;
            yield return null;
        }
        Debug.Log("Waiting for encounter to end.");
        yield return new WaitUntil(() => !encounterActive);
        
        badEndAnimCon.Play("End");
    }

    // Start is called before the first frame update
    void Start()
    {
        bikeMovement = FindObjectOfType<BikeMovement>();
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
        StartCoroutine(GoodEndingRoutine());
    }

    public void BadEnding()
    {
        encounterActive = false;
        StartCoroutine(BadEndingRoutine());
    }

}
