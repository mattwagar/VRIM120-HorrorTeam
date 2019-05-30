using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSCanvasManager : MonoBehaviour
{
    public AudioSource staticAudio;
    public CanvasGroup canvasGroup;
    public Transform GPSRay;
    public float animSpeed = 0.2f;
    public bool isChased = false;

    private IEnumerator GPSIndicator()
    {
        float decisionDir = 0f;
        yield return new WaitUntil(() => isChased);
        while(isChased)
        {
            // Bit shift the index of the layer (8) to get a bit mask
            int layerMask = 1 << 9;

            // This would cast rays only against colliders in layer 8.
            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            layerMask = ~layerMask;

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, GPSRay.TransformDirection(Vector3.forward), out hit, 20, layerMask))
            {
                Debug.DrawRay(transform.position, GPSRay.TransformDirection(Vector3.forward) * hit.distance, Color.blue);

                //Replace with animations
                if(hit.collider.gameObject.name == "BadChoice")
                {
                    decisionDir += Mathf.Clamp01(Time.deltaTime * animSpeed);
                }
                else
                {
                    decisionDir -= Mathf.Clamp01(Time.deltaTime * animSpeed);
                }
            }
            canvasGroup.alpha = decisionDir;
            staticAudio.volume = decisionDir;
        }
    }
}
