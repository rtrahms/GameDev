using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public float maxRotationForce = 25f;
	public float maxTravelForce = 200f;
	public float maxJumpForce = 200f;
	public float minYDeg = -80.0f;
	public float maxYDeg = 80.0f;
	
	private Camera mainCam;
	private float xDeg, yDeg;
	private Quaternion fromRotation;
	private Quaternion toRotation;
	private Quaternion cameraTilt;
	
	public Transporter transporter;
	private bool activateTransporter;
	private float startTime;
	
	// Use this for initialization
	void Start () {
	
		transporter = null;
		
		xDeg = 0.0f;
		yDeg = 0.0f;
		
		mainCam = GetComponentInChildren<Camera>();
		if (mainCam == null)
			print ("Could not find camera");
			
		activateTransporter = false;
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
		
		// player wants to activate transporter
		if (Input.GetKey (KeyCode.E) && (!activateTransporter))
		{
			activateTransporter = true;
			startTime = Time.time;
		}
		
		// slow down teleport by 0.25 s
		if ((activateTransporter) && (Time.time - startTime > 0.25f))
		{
			if (transporter != null)
			{
				transporter.Activate(this.gameObject);
				transporter = null;
			}
			activateTransporter = false;
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
