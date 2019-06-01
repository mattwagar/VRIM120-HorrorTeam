using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DecisionCollisionHandler : MonoBehaviour
{
    public NarrativeManager narrativeManager;

    // Debug 
    public TextMeshProUGUI Debug_GoodDec;
    public TextMeshProUGUI Debug_BadDec;

    private float goodDec = 0;
    private float badDec = 0;

    private void Start() 
    {
        narrativeManager = FindObjectOfType<NarrativeManager>();
    }

    private void OnTriggerEnter(Collider other) 
    {
        Debug.Log("Collision Occurred");
        if(other.gameObject.name.Equals("BadChoice"))
        {
            narrativeManager.BadChoice();
            badDec++;
            Debug_BadDec.text = "Bad Decisions: " + badDec;
        }
        else if(other.gameObject.name.Equals("GoodChoice"))
        {
            narrativeManager.GoodChoice();
            goodDec++;
            Debug_GoodDec.text = "Good Decisions: " + goodDec;
        }
    }
}
