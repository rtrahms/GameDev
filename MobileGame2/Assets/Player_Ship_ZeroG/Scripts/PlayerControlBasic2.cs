using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlayerControlBasic2 : MonoBehaviour {

	public float maxRotationForce = 25f;
	public float maxTravelForce = 25f;
	public float maxJumpForce = 25f;
	public float rollForce = 50f;
	public AudioClip thrusterClip;
	public GameObject objToControl;
		
	private Transform myTransform;
	private Rigidbody myRigidBody;
	
	// Use this for initialization
	void Start () {
		myTransform = objToControl.transform;
		myRigidBody = objToControl.rigidbody;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
					
		float v = DeadZone(Input.GetAxis("Vertical"),-0.1f,0.1f);
		
		// determine forward force
		Vector3 forwardForceVec;
			
		if (myTransform.parent != null)
		{
			// if object is a child of a parent FOV, convert forward based on that FOV
			forwardForceVec = myTransform.parent.TransformDirection (myTransform.forward) * v * maxTravelForce;			
		}
		else
		{
			forwardForceVec = myTransform.forward * v * maxTravelForce;			
		}	
		
		if (forwardForceVec != null)	
			myRigidBody.AddForce(forwardForceVec);
		
		// determine up force
		Vector3 upForceVec;	
		
		if (Input.GetKey (KeyCode.Space))
		{
			if (myTransform.parent != null)
			{
				// if object is a child of a parent FOV, convert up based on that FOV
				upForceVec = myTransform.parent.TransformDirection (myTransform.up) * maxJumpForce;
			}
			else
			{
				upForceVec = myTransform.up * maxJumpForce;
			}
			
			myRigidBody.AddForce(upForceVec);							
		}
		/*
		if ((forwardForceVec != null) || (upForceVec != null))
		{
			AudioSource.PlayClipAtPoint(thrusterClip,transform.position);
		}
		*/
		
		
		// TODO determine roll
		/*
		Vector3 rollVec;
		if (Input.GetKey (KeyCode.A))
		{
			print ("rolling left");
			rollVec = transform.forward * -rollForce;
			rigidbody.AddTorque (rollVec);
		}
		else if (Input.GetKey (KeyCode.D))
		{
			print ("rolling right");
			rollVec = transform.forward * rollForce;
			rigidbody.AddTorque (rollVec);
		}
		*/		
	}
		
	public void AdjustHealth(float value)
	{
		// TODO: make adjustment to health of player
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
