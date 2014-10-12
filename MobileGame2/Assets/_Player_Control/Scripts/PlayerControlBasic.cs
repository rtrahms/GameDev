using UnityEngine;
using System.Collections;

public class PlayerControlBasic : MonoBehaviour {

	public float maxRotationForce = 25f;
	public float maxTravelForce = 25f;
	public float maxJumpForce = 25f;
	public float minYDeg = -80.0f;
	public float maxYDeg = 80.0f;
	
	private Camera mainCam;
	private float xDeg, yDeg;
	private Quaternion fromRotation;
	private Quaternion toRotation;
	private Quaternion cameraTilt;
	public float maxFov = 90f;
	public float minFov = 10f;
	private float currentFov = 50f;
	
	// Use this for initialization
	void Start () {
		
		xDeg = 0.0f;
		yDeg = 0.0f;
		
		mainCam = GetComponentInChildren<Camera>();
		if (mainCam == null)
			print ("Could not find camera");

	}
	
	// Update is called once per frame
	void FixedUpdate () {
			
		xDeg += (Input.GetAxis ("Mouse X") * maxRotationForce);
		yDeg += -1.0f * Input.GetAxis ("Mouse Y") * maxRotationForce;
		yDeg = Limiter(yDeg, minYDeg, maxYDeg);
		
		// rotate player only in x axis
		//toRotation = Quaternion.Euler(yDeg, xDeg, 0);
		toRotation = Quaternion.Euler(0, xDeg, 0);
		transform.rotation = toRotation;

		// camera tilt only in both x and y axis
		cameraTilt = Quaternion.Euler (yDeg,xDeg,0);
		mainCam.transform.rotation = cameraTilt;
					
		//print ("xDeg = " + xDeg + " yDeg = " + yDeg);
		
		float v = DeadZone(Input.GetAxis("Vertical"),-0.1f,0.1f);
		
		// determine forward force
		Vector3 forwardForceVec;	
		if (transform.parent != null)
		{
			// if object is a child of a parent FOV, convert forward based on that FOV
			forwardForceVec = transform.parent.TransformDirection (transform.forward) * v * maxTravelForce;
		}
		else
		{
			forwardForceVec = transform.forward * v * maxTravelForce;
		}
		rigidbody.AddForce(forwardForceVec);
		
		// determine up force
		Vector3 upForceVec;	
		if (Input.GetKey (KeyCode.Space))
		{
			if (transform.parent != null)
			{
				// if object is a child of a parent FOV, convert up based on that FOV
				upForceVec = transform.parent.TransformDirection (transform.up) * maxJumpForce;
			}
			else
			{
				upForceVec = transform.up * maxJumpForce;
			}
			rigidbody.AddForce(upForceVec);
		}		
		
		// zoom out
		if (Input.GetMouseButtonDown(1))
		{
			mainCam.fieldOfView = 50f;
		}
		
		// zoom in
		//if (Input.GetKey (KeyCode.Z) && (mainCam.fieldOfView > minFov))
		if (Input.GetMouseButton(1) && (mainCam.fieldOfView > minFov))
		{
			mainCam.fieldOfView -= 1f;
		}
	}
	
	float Limiter(float value, float minVal, float maxVal)
	{
		if (value < minVal)
			return minVal;
		if (value > maxVal)
			return maxVal;
		
		return value;
	}	
	
	float DeadZone(float value, float negThresh, float posThresh)
	{
		if (value < negThresh)
			return value;
		if (value > posThresh)
			return value;
		
		return 0f;
	}	
	
	public void AdjustHealth(float value)
	{
		// TODO: make adjustment to health of player
	}
	
	
}
