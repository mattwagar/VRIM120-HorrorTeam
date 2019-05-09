using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeManager : MonoBehaviour
{
    public BikeMovement bikeMovement;
    public ClimaxAdjust climax;
    public int choiceValue;
    public int speedModifier = 300;

    // Start is called before the first frame update
    void Start()
    {
        bikeMovement = FindObjectOfType<BikeMovement>();
    }

    public void BadChoice()
    {
        bikeMovement.vertInput -= speedModifier;
        choiceValue -= 1;
        climax.Adjust(choiceValue);
    }

    public void GoodChoice()
    {
        bikeMovement.vertInput += speedModifier;
        choiceValue += 1;
        climax.Adjust(choiceValue);
    }

}
