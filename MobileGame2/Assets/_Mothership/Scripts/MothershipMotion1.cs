using UnityEngine;
using System.Collections;

public class MothershipMotion1 : MonoBehaviour {

	public Vector3 center = new Vector3(0,0,0);
	public Vector3 size = new Vector3(1000f, 30f, 1000f);
	public float courseChangePeriodSec = 15f;
	public float minCourseChangeDist = 200f;
	public float speed = 10f;
	public float turnSpeed = 1f;
	
	private float startTime;
	private Vector3 newLocation;
	
	// Use this for initialization
	void Start () {
		
		// force first course change
		startTime = Time.time - courseChangePeriodSec;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		if ((Time.time - startTime) > courseChangePeriodSec)
		{
			newLocation = transform.position;
			while (Vector3.Distance (newLocation,transform.position) < minCourseChangeDist)
			{
				newLocation = new Vector3(center.x + Random.Range (-size.x,size.x),
			                                  center.y + Random.Range (-size.y,size.y),
			                                  center.z + Random.Range (-size.z,size.z));
			}
			
			//print ("mothership currentLoc = " + transform.position + " newLoc = " + newLocation);
		     
		    startTime = Time.time; 
		}
		                                  
			
		// target set - rotate toward that
		Vector3 travelVec = (newLocation - transform.position).normalized;
		Quaternion rot = Quaternion.LookRotation(travelVec);
		transform.rotation = Quaternion.Slerp (transform.rotation, rot, turnSpeed * Time.deltaTime);
		
		// travel
		transform.position = transform.position + transform.forward * speed * Time.deltaTime;
	}
}
