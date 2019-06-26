using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetEncounter : MonoBehaviour
{
    public bool encounterVal = false;

    public NarrativeManager narrativeManager;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.name == "Bike")
            narrativeManager.encounterActive = encounterVal;
    }

}
