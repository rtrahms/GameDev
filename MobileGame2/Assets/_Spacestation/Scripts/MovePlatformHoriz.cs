using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovePlatformHoriz : MonoBehaviour {

	public float speed = 5f;
	public float range = 40f;
	public float startTimeSec = 2.0f;
	public float stopTimeSec = 2.0f;
	
	private Vector3 initialPos;
	private float direction = 1.0f;
	private float distanceToZero;
	private bool pausing = false;
	private float startTime, stopTime;
	
	private List<GameObject> passengers;
	
	// Use this for initialization
	void Start () {
		initialPos = transform.position;
		passengers = new List<GameObject>();
		startTime = Time.time;
		stopTime = Time.time;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		distanceToZero = Vector3.Distance(initialPos,transform.position);
		if (distanceToZero > range)
		{
			direction = -direction;
		}
		
		if (!pausing)
		{
			Vector3 travel = transform.forward * speed * direction * Time.deltaTime;
			transform.Translate (travel);	
		}
				
		if (!pausing && ((Time.time - startTime) > startTimeSec) && (distanceToZero < 0.5))
		{
			print ("pausing distanceToZero = " + distanceToZero);
			pausing = true;					
			stopTime = Time.time;					
		}
		
		if (pausing && ((Time.time - stopTime) > stopTimeSec))
		{
			print ("not pausing");
			pausing = false;					
			startTime = Time.time;					
		}
		
	}
	
	void OnTriggerEnter(Collider col)
	{
		print ("Passenger boarding!");
		passengers.Add (col.gameObject);
		
		// set the parent of the object to the elevator
		col.transform.parent = transform;
		col.transform.rotation = transform.rotation;
		col.rigidbody.useGravity = false;
		col.rigidbody.drag = 50f;
	}
	
	void OnTriggerExit(Collider col)
	{
		print ("Passenger departing!");
		passengers.Remove(col.gameObject);

		// reset the parent of the object
		col.transform.parent = null;
		col.rigidbody.useGravity = true;
		col.rigidbody.drag = 1f;
		
	}
}
