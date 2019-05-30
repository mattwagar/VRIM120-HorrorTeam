using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimaxAdjust : MonoBehaviour
{
    public GameObject climaxSetpiece;
    public Transform goodEnding;
    public Transform badEnding;
    
    public void Adjust(int val)
    {
        climaxSetpiece.transform.position = Vector3.Lerp(badEnding.position, goodEnding.position, (val/5f) + 0.5f);
    }
}
