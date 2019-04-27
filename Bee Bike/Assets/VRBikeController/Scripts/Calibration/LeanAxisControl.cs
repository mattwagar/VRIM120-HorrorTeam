using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanAxisControl : MonoBehaviour {
    public Transform left, right, center, root;
    public Transform handleBarTracker;

    public float leftAxisAmount = 0f;
    public float rightAxisAmount = 0f;
    public float combinedAxis = 0f;

    float distanceCenterToRight;
    float distanceCenterToLeft;

    Calibrate2 calibrationComponent;
    void Start() {
        calibrationComponent = GetComponent<Calibrate2>();
    }

	// Update is called once per frame
	void Update () {
        //Vector3 rootToCenter  = center.position - root.position;
        //Vector3 rootToRight   = right.position - root.position;
        //Vector3 rootToLeft    = left.position - root.position;
        //Vector3 rootToCurrent = handleBarTracker.position - root.position;
        //
        //float centerToLeftAngle  = Vector3.Angle(rootToCenter, rootToRight);
        //float currentToLeftAngle = Vector3.Angle(rootToCurrent, rootToRight);
        //
        //float centerToRightAngle = Vector3.Angle(rootToCenter, rootToLeft);
        //float currentToRightAngle = Vector3.Angle(rootToCurrent, rootToLeft);

        //Debug.Log(centerToLeftAngle);
        //Debug.Log(centerToRightAngle);
        //leftAxisAmount = Mathf.Clamp(0f, 1f, currentToLeftAngle / centerToLeftAngle);
        //rightAxisAmount = Mathf.Clamp(0f, 1f, currentToRightAngle / centerToRightAngle);
        if (calibrationComponent.calibrateCompleted)
        {
            distanceCenterToLeft = Vector3.Distance(center.position, left.position);
            distanceCenterToRight = Vector3.Distance(center.position, right.position);
            if (distanceCenterToLeft > 0.001f && distanceCenterToRight > 0.001f)
            {
                leftAxisAmount = 1f - Mathf.Clamp(1f, 0f, (Vector3.Distance(handleBarTracker.position, left.position) / distanceCenterToLeft));
                rightAxisAmount = 1f - Mathf.Clamp(1f, 0f, (Vector3.Distance(handleBarTracker.position, right.position) / distanceCenterToRight));
                //Debug.Log(Mathf.Clamp((float)1f,(float)0f,(float)(Vector3.Distance(handleBarTracker.position, left.position) / distanceCenterToLeft)));
                //Debug.Log(distanceCenterToRight = Vector3.Distance(handleBarTracker.position, right.position));
                combinedAxis = -1f * leftAxisAmount + rightAxisAmount;
            }
        }
    }
}
