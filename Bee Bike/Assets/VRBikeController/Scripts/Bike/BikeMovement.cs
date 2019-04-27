using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Greg Kilmer
 * Function: Contols the movement of a bike object. Intended for use with both player and enemy bikes.
 * Last Updated: 5/17/2018
 */

public class BikeMovement : MonoBehaviour {
	public GameObject bike;
	public float accelerationRate;
	public float decelerationRate;
	public float topSpeed;
    public float bridgeBoostFactor;
    public LayerMask boostLayers;

	public float leanRate;

	public float turnRate;
	//public float maxTurn;

	public Vector3 curVelocity;

	private Rigidbody rb;

	public LeanAxisControl leanAxisController;


    public float h, v, handbrake;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
        rb.velocity = curVelocity;
	}
	
	// Update is called once per frame
	void Update () {
        // Get throttle input
        
		curVelocity = rb.velocity;
		//Curb Velocity to topSpeed
		if (rb.velocity.magnitude >= topSpeed) {
			rb.velocity = rb.velocity.normalized* topSpeed;
		}

		Debug.DrawRay(bike.transform.position, bike.transform.right*10,Color.red);

        // pass the input to the car!
        //float h = CrossPlatformInputManager.GetAxis("Horizontal");
        h = leanAxisController.combinedAxis;
        v = Input.GetAxis("Vertical");

        handbrake = Input.GetAxis("Jump");
        Turn(h);
        Accelerate(v);
        Decelerate(handbrake);
        //v, v, handbrake);

      //  rb.AddForce(bike.transform.forward * Input.GetAxis("Vertical"), ForceMode.VelocityChange);
    }

	//Increase speed in forward direction
	public void Accelerate() {
        if (Physics.Raycast(bike.transform.position,transform.up*-1,10,boostLayers.value))
        {
            rb.AddForce(bike.transform.forward * bridgeBoostFactor * accelerationRate * Time.deltaTime);
        } else
        {
           rb.AddForce(bike.transform.forward * accelerationRate * Time.deltaTime);
        }
	}

	//Decrease speed in the forward direction
	public void Decelerate(float input) {
		rb.AddForce (bike.transform.forward * input * -decelerationRate * Time.deltaTime);
	}

    public void Accelerate(float percent)
    {
        if (Physics.Raycast(bike.transform.position, bike.transform.up * -1, 10, boostLayers.value))
        {
            rb.AddForce(bike.transform.forward * percent * bridgeBoostFactor * accelerationRate * Time.deltaTime);
        }
        else
        {
            rb.AddForce(bike.transform.forward * percent * accelerationRate * Time.deltaTime );
        }
    }

    public void Turn(float percent)
    {
        float turnAmt = percent * turnRate;
        transform.RotateAround(bike.transform.position, bike.transform.up, turnAmt);
        rb.velocity = Quaternion.AngleAxis(turnAmt, bike.transform.up) * rb.velocity;
    }

	//Turn left using handle bars
	public void TurnLeft() {
		float turnAmt = -turnRate * rb.velocity.magnitude;
		transform.RotateAround (transform.position, transform.up, turnAmt);
		rb.velocity = Quaternion.AngleAxis (turnAmt, transform.up) * rb.velocity;
	}

	//Turn right using handle bars
	public void TurnRight() {
		float turnAmt = turnRate * rb.velocity.magnitude;
		transform.RotateAround (transform.position, transform.up, turnAmt);
		rb.velocity = Quaternion.AngleAxis (turnAmt, transform.up) * rb.velocity;
	}

	//Slides bike left by leaning left
	public void LeanLeft(float percentageOfLeanRate) {
		//TODO revamp to use velocity scaling only in the forward direction
		rb.AddForce ((transform.right + transform.up * Mathf.Tan (transform.rotation.eulerAngles.z)) * -leanRate * percentageOfLeanRate);
	}

	//Slides bike right by learning right
	public void LeanRight(float percentageOfLeanRate) {
		//TODO revamp to use velocity scaling only in the forward direction
		rb.AddForce ((transform.right + transform.up * Mathf.Tan (transform.rotation.eulerAngles.z)) * leanRate * percentageOfLeanRate);
	}
}
