using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLink : MonoBehaviour
{
    public NarrativeManager narrativeManager;

    public void StartEncounter()
    {
        narrativeManager.encounterActive = false;
    }
}
