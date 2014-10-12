using UnityEngine;
using System.Collections;

// this script controls objects that are kinematic - ie do not respond to AddForce like PlayerControlBasic2

[RequireComponent(typeof(AudioSource))]
public class PlayerControlBasic3 : MonoBehaviour {

	public float maxRotationForce = 25f;
	public float maxTravelSpeed = 1f;
	public float maxJumpSpeed = 1f;
	public float frictionFactor = 0.999f;
	public AudioClip thrusterClip;
	public GameObject objToControl;
		
	private float speedIncrement = 1.0f;
	public bool cruiseControl;
	private Transform myTransform;
	private float forwardSpeed, sideSpeed;
	private float upSpeed;
	private Vector3 velocity;
	private Vector3 forwardForceVec, sideForceVec, upForceVec;
	private float uiDebounce = 0.25f;
	private float startTime;
	
	// Use this for initialization
	void Start () {
		myTransform = objToControl.transform;
		velocity = Vector3.zero;
		forwardSpeed = 0f;
		sideSpeed = 0f;
		upSpeed = 0f;
		cruiseControl = false;
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
				
		// thruster force (forward)	
		float v = DeadZone(Input.GetAxis("Vertical"),-0.1f,0.1f);	
		if (Mathf.Abs (v) > 0f)
		{
			// determine forward force
			forwardSpeed += v * speedIncrement;
			forwardSpeed = Limiter(forwardSpeed,-maxTravelSpeed,maxTravelSpeed);
			
			forwardForceVec = (myTransform.forward * forwardSpeed);	
			velocity += forwardForceVec;
		}
		
		// strafing force (sideways)
		float h = DeadZone(Input.GetAxis("Horizontal"),-0.1f,0.1f);
		if (Mathf.Abs (h) > 0f)
		{
			sideSpeed += h * speedIncrement;
			sideSpeed = Limiter(sideSpeed,-maxTravelSpeed,maxTravelSpeed);
			sideForceVec = (myTransform.right * sideSpeed);	
			velocity += sideForceVec;
		}
				
		// booster force (up)
		if (Input.GetKey (KeyCode.Space))
		{
			// determine up force		
			upSpeed += speedIncrement;
			upSpeed = Limiter(upSpeed,-maxJumpSpeed,maxJumpSpeed);						
			upForceVec = myTransform.up * upSpeed;	
			velocity += upForceVec;
		}

		if (Input.GetKey (KeyCode.F) && (Time.time - startTime > uiDebounce))
		{
			cruiseControl = !cruiseControl;
			print ("Cruise control = " + cruiseControl);
			startTime = Time.time;
		}
		
		// dampen velocity
		if (!cruiseControl)
		{
			float newMagnitude = Dampen(velocity.magnitude,frictionFactor);
			velocity = velocity.normalized * newMagnitude;
		}
		
		//print ("forwardSpeed = " + forwardSpeed + " sideSpeed = " + sideSpeed + " upSpeed = " + upSpeed);
		
	}
	
	void Update () 
	{
						
		//print ("velocity magnitude = " + velocity.magnitude);

		myTransform.position += velocity * Time.deltaTime;
		

		if (velocity.magnitude < 0.5f)
		{
			upSpeed = 0f;
			forwardSpeed = 0f;
			sideSpeed = 0f;
			velocity = Vector3.zero;
		}
		
		/*
		if ((forwardForceVec != null) || (upForceVec != null))
		{
			AudioSource.PlayClipAtPoint(thrusterClip,transform.position);
		}
		*/
		
	}
		
	float Limiter(float initialValue, float minVal, float maxVal)
	{
		if (initialValue < minVal)
			return minVal;
		
		if (initialValue > maxVal)
			return maxVal;
			
		return initialValue;
	}
	
	float Dampen(float initialValue, float dampMultiplier)
	{
		float newVal;
		
		if (Mathf.Abs (initialValue) < 0.1f)
			return 0f;
		
		newVal = initialValue * dampMultiplier;
		if (Mathf.Abs (newVal) > 0.1f)
			return newVal;	
		
		return 0f;
			
	}
		
	float DeadZone(float value, float negThresh, float posThresh)
	{
		if (value < negThresh)
			return value;
		if (value > posThresh)
			return value;
		
		return 0f;
	}	
	
}
