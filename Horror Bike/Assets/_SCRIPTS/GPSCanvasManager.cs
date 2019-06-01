using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class GPSCanvasManager : MonoBehaviour
{
    public AudioSource staticAudio;
    public CanvasGroup canvasGroup;
    public Transform GPSRay;
    public float animSpeed = 0.2f;
    public bool isChased = false;
    public Sprite brokenSprite;
    public Image gpsUI;
    public float decisionDir = 0f;

    private void Start() 
    {
        StartCoroutine(GPSIndicator());
    }

    // private void Update() 
    // {
    //     Debug.DrawRay(transform.position, GPSRay.TransformDirection(Vector3.forward) * 20, Color.blue);
    // }

    private IEnumerator GPSIndicator()
    {
        yield return new WaitUntil(() => isChased);
        gpsUI.sprite = brokenSprite;
        while(isChased)
        {
            // Bit shift the index of the layer (8) to get a bit mask
            int layerMask = 1 << 9;

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, GPSRay.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
                // Debug.DrawRay(transform.position, GPSRay.TransformDirection(Vector3.forward) * hit.distance, Color.blue);

                Debug.Log("GPS Hitting: " + hit.collider.gameObject.name);

                if(hit.collider.gameObject.name == "BadChoice")
                {
                    decisionDir = Mathf.Clamp01(decisionDir + (Time.deltaTime * animSpeed));
                }
                else
                {
                    decisionDir = Mathf.Clamp01(decisionDir - (Time.deltaTime * animSpeed));
                }
            }
            canvasGroup.alpha = decisionDir;
            staticAudio.volume = decisionDir;
            yield return null; 
        }
    }
}
