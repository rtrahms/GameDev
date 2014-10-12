using UnityEngine;
using System.Collections;

public class SweepRotation : MonoBehaviour {

	public float sweepAngleDeg = 45.0f;
	public float sweepAngleRate = 5.0f;
	
	private float startAngleZ;
	private float direction;
	
	// Use this for initialization
	void Start () {
		startAngleZ = transform.eulerAngles.z;
		direction = 1.0f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float deltaSweepAngle = sweepAngleRate * direction * Time.deltaTime;
		transform.Rotate (new Vector3(0.0f, 0.0f, deltaSweepAngle));
		
		if (Mathf.Abs(transform.eulerAngles.z - startAngleZ) > sweepAngleDeg)
		{
			// switch direction
			direction = -direction;
		}
	}
}
