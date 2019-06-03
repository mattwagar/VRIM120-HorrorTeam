using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVRFade : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
	    Color startColor = new Color(0, 0, 0, 1);
	    Color endColor = new Color(0, 0, 0, 0);
        SteamVR_Fade.Start(startColor, 0);
        SteamVR_Fade.Start(endColor, 3f);
    }
}
