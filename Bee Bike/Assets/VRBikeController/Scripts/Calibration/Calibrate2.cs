using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibrate2 : MonoBehaviour {
    public Transform[] tracker;
    public float neglibleDelta = .01f;
    public float continuousHeldTime = 0f;
    public Transform leftMarker, centerMarker, rightMarker, rootMarker;
    public float calibrationStartedThreshold = .05f;
    public float handlebarTrackerHeight = .5f;
    public bool calibrateStarted = false;
    public bool calibrateCompleted = false;
    public bool calibrateOnStart = true;
    public BikeMovement bikeMovementComponent;
    public CalculateBikeModelPositionAndForward bikeAlignment;

    public float centerTrackingCompletion, rightTrackingCompletion, leftTrackingCompletion;
    public string GuideText = "Begin \n Calibration";

    void Awake(){
       //if (!calibrateStarted)
       //{
       //    StartCoroutine(BeginCalibration());
       //}
    }
    void Start() {
        if (!calibrateStarted && calibrateOnStart)
        {
            StartCoroutine(BeginCalibration());
        }
    }

    void Update() {
        if (!calibrateStarted && calibrateOnStart) {
            StartCoroutine(BeginCalibration());
        }
    }

    public void BeginCalibrating() {
        StartCoroutine(BeginCalibration());
    }


    Vector3 frontPoint;
    private IEnumerator BeginCalibration()
    {
        calibrateStarted = true;
        frontPoint = tracker[0].position;

        Vector3 previousFrontPoint = frontPoint;
        Debug.Log("Dont move keep the front at the center");
        GuideText = "Keep the bike still in the center";
        continuousHeldTime = 0f;
        centerTrackingCompletion = 0f;
        while (continuousHeldTime < 1f) {
            if (Vector3.Distance(previousFrontPoint, tracker[0].position) < neglibleDelta)
            {
                continuousHeldTime += Time.deltaTime / 5f;
            } else {
                continuousHeldTime = 0f;
            }
            centerTrackingCompletion = Mathf.Clamp01(continuousHeldTime);
            previousFrontPoint = tracker[0].position;
            yield return new WaitForEndOfFrame();
        }
        centerMarker.position = previousFrontPoint;
        GuideText = "Lean all the way to the right";
        Debug.Log("Lean all the way to the right");
        while (Vector3.Distance(tracker[0].position, previousFrontPoint) < calibrationStartedThreshold)
        {
            yield return new WaitForEndOfFrame();
        }
        continuousHeldTime = 0f;
        rightTrackingCompletion = 0f;
        while (continuousHeldTime < 1f) {
            if (Vector3.Distance(previousFrontPoint, tracker[0].position) < neglibleDelta) {
                    continuousHeldTime += Time.deltaTime / 5f;
            } else {
                    continuousHeldTime = 0f;
            }
            rightTrackingCompletion = Mathf.Clamp01(continuousHeldTime);
            previousFrontPoint = tracker[0].position;
            yield return new WaitForEndOfFrame();
        }
        rightMarker.position = previousFrontPoint;
        GuideText = "Now lean all the way to the left";
        Debug.Log("Now lean all the way to the left");
        while (Vector3.Distance(tracker[0].position, previousFrontPoint) < calibrationStartedThreshold) {
            yield return new WaitForEndOfFrame();
        }
        continuousHeldTime = 0f;
        leftTrackingCompletion = 0f;
        while (continuousHeldTime < 1f) {
            if (Vector2.Distance(previousFrontPoint, tracker[0].position) < neglibleDelta) {
                continuousHeldTime += Time.deltaTime / 5f;
            } else {
                continuousHeldTime = 0f;
            }
            leftTrackingCompletion = Mathf.Clamp01(continuousHeldTime);
            previousFrontPoint = tracker[0].position;
            yield return new WaitForEndOfFrame();
        }
        leftMarker.position = previousFrontPoint;
        Debug.Log("Okay chill");

        Vector3 rootPosition = centerMarker.position;
        rootPosition.y = rootPosition.y - handlebarTrackerHeight;
        rootMarker.position = rootPosition;
        calibrateCompleted = true;
       // bikeMovementComponent.gameObject.GetComponent<Rigidbody>().velocity = bikeMovementComponent.transform.forward * 20f;
        bikeAlignment.Align();
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(tracker[0].position, tracker[0].position + calibrationStartedThreshold * transform.right);

        Gizmos.DrawLine(tracker[0].position, tracker[0].position - handlebarTrackerHeight * Vector3.up);
    }
}
