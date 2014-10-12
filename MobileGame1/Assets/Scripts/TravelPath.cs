using UnityEngine;
using System.Collections;

public class TravelPath : MonoBehaviour {

	public float speed = -1.0f;
	public float rotateDegRate = -2.0f;
	public float travelSec = 15.0f;
	
	private float startTime;
	
	// Use this for initialization
	void Start () {
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		if ((Time.time - startTime) < travelSec)
		{
		
			Vector3 travelVec = transform.forward  * speed * Time.deltaTime;
			transform.position += travelVec;
			
			transform.Rotate (new Vector3(0,rotateDegRate * Time.deltaTime,0));
		}
	}
}
