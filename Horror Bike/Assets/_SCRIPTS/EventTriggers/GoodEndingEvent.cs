using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodEndingEvent : MonoBehaviour
{
    public NarrativeManager narrativeManager;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.name == "Bike")
            narrativeManager.GoodEnding();
    }

}
