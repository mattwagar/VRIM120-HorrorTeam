using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadEndingEvent : MonoBehaviour
{
    public NarrativeManager narrativeManager;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.name == "Bike")
            narrativeManager.BadEnding();
    }

}
