using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * Original Author: Greg Kilmer
 * V2 Author: Matthew Napolillo
 * Function: Contols the movement of a bike object.
 * Last Updated: 4/25/2019
 */

public class BikeMovement : MonoBehaviour {
	
	public GameObject bike;
	public float accelerationRate;
	public float decelerationRate;
	public float topSpeed;
	public float minSpeed;
    public LayerMask boostLayers;

	public float leanRate;

	public float turnRate;

	public float gravity;

	public Vector3 curVelocity;

	private Rigidbody bikeRigidBody;

	public LeanAxisControl leanAxisController;

    public float horizInput, vertInput, handbrake;

	public bool hasMinSpeed = false;

	[Header("InEditorTesting")]
	public bool keyboardControl = false;
	public GameObject[] disableForTesting;
	public GameObject[] disableForVR;
	public TextMeshProUGUI Debug_BikeSpeed;
	public TextMeshProUGUI Debug_DriftSpeed;

	private void Awake() 
	{
		if(keyboardControl)
		{
			foreach(GameObject gObj in disableForTesting)
			{
				gObj.SetActive(false);
			}
		}
		else
		{
			foreach(GameObject gObj in disableForVR)
			{
				gObj.SetActive(false);
			}
		}
	}

	// Use this for initialization
	void Start () 
	{
		bikeRigidBody = GetComponent<Rigidbody> ();
        bikeRigidBody.velocity = curVelocity;
	}
	
	// Update is called once per frame
	void Update () 
	{
        // Get throttle input
		curVelocity = bikeRigidBody.velocity;
		Vector3 locVel = transform.InverseTransformDirection(curVelocity);
		Debug_BikeSpeed.text = "Speed: " + locVel.z;
		Debug_DriftSpeed.text = "Drift Speed: " + locVel.x;
		
		//CubikeRigidBody Velocity to topSpeed
		if (bikeRigidBody.velocity.magnitude >= topSpeed) 
		{
			bikeRigidBody.velocity = bikeRigidBody.velocity.normalized * topSpeed;
		}

		if (bikeRigidBody.velocity.magnitude <= minSpeed && hasMinSpeed) 
		{
			bikeRigidBody.velocity = bikeRigidBody.velocity.normalized * topSpeed;
		}

		Debug.DrawRay(bike.transform.position, bike.transform.right*10,Color.red);

        // pass the input to the car!
		if(keyboardControl)
		{
			horizInput = Input.GetAxis("Horizontal");
		}
		else
		{
        	horizInput = leanAxisController.combinedAxis;
		}
        // Fixed input
		//vertInput = Input.GetAxis("Vertical");

        handbrake = Input.GetAxis("Jump");
		if(curVelocity.sqrMagnitude > 1)
        	Turn(horizInput);
        Accelerate(vertInput);
        Decelerate(handbrake);
		// FixDrift();

		//Apply gravity over time
		bikeRigidBody.AddForce(Vector3.down * gravity * Time.deltaTime);
    }

	//Increase speed in forward direction
	public void Accelerate() 
	{
        if (Physics.Raycast(bike.transform.position, transform.up * -1, 10, boostLayers.value))
        {
            bikeRigidBody.AddForce(bike.transform.forward * accelerationRate * Time.deltaTime);
        } 
		else
        {
           bikeRigidBody.AddForce(bike.transform.forward * accelerationRate * Time.deltaTime);
        }
	}

	//Decrease speed in the forward direction
	public void Decelerate(float input) 
	{
		if(bikeRigidBody.velocity.sqrMagnitude > 1)
		{
			bikeRigidBody.AddForce(bikeRigidBody.velocity.normalized * input * -decelerationRate * Time.deltaTime);
		}
		if(input == 1 && bikeRigidBody.velocity.sqrMagnitude < 1)
		{
			bikeRigidBody.velocity = Vector3.zero;
		}
	}

	public void FixDrift()
	{
		Vector3 locVel = transform.InverseTransformDirection(bikeRigidBody.velocity);
		if(locVel.x > 1)
		{
			bikeRigidBody.AddForce(bike.transform.right * -decelerationRate * 3f * Time.deltaTime);
		}
		else if(locVel.x < -1)
		{
			bikeRigidBody.AddForce(bike.transform.right * decelerationRate * 3f * Time.deltaTime);
		}
		if(Mathf.Abs(locVel.x) < 1)
		{
			bikeRigidBody.velocity = new Vector3(0, bikeRigidBody.velocity.y, bikeRigidBody.velocity.z);
		}
	}

    public void Accelerate(float percent)
    {
        if (Physics.Raycast(bike.transform.position, bike.transform.up * -1, 10, boostLayers.value))
        {
            bikeRigidBody.AddForce(bike.transform.forward * percent * accelerationRate * Time.deltaTime);
        }
        else
        {
            bikeRigidBody.AddForce(bike.transform.forward * percent * accelerationRate * Time.deltaTime );
        }
    }

    public void Turn(float percent)
    {
        float turnAmt = percent * turnRate;
        transform.RotateAround(bike.transform.position, bike.transform.up, turnAmt);
        bikeRigidBody.velocity = Quaternion.AngleAxis(turnAmt, bike.transform.up) * bikeRigidBody.velocity;
    }

	//Turn left using handle bars
	public void TurnLeft() {
		float turnAmt = -turnRate * bikeRigidBody.velocity.magnitude;
		transform.RotateAround (transform.position, transform.up, turnAmt);
		bikeRigidBody.velocity = Quaternion.AngleAxis (turnAmt, transform.up) * bikeRigidBody.velocity;
	}

	//Turn right using handle bars
	public void TurnRight() {
		float turnAmt = turnRate * bikeRigidBody.velocity.magnitude;
		transform.RotateAround (transform.position, transform.up, turnAmt);
		bikeRigidBody.velocity = Quaternion.AngleAxis (turnAmt, transform.up) * bikeRigidBody.velocity;
	}

	//Slides bike left by leaning left
	public void LeanLeft(float percentageOfLeanRate) {
		//TODO revamp to use velocity scaling only in the forward direction
		bikeRigidBody.AddForce ((transform.right + transform.up * Mathf.Tan (transform.rotation.eulerAngles.z)) * -leanRate * percentageOfLeanRate);
	}

	//Slides bike right by learning right
	public void LeanRight(float percentageOfLeanRate) {
		//TODO revamp to use velocity scaling only in the forward direction
		bikeRigidBody.AddForce ((transform.right + transform.up * Mathf.Tan (transform.rotation.eulerAngles.z)) * leanRate * percentageOfLeanRate);
	}
}
