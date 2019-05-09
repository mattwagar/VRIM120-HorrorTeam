using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionCollisionHandler : MonoBehaviour
{
    public NarrativeManager narrativeManager; 

    private void Start() 
    {
        narrativeManager = FindObjectOfType<NarrativeManager>();    
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.name.Equals("BadChoice"))
        {
            narrativeManager.BadChoice();
        }
        else if(other.name.Equals("GoodChoice"))
        {
            narrativeManager.GoodChoice();
        }
    }
}
