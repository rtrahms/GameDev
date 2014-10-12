using UnityEngine;
using System.Collections;

public class ArtificialGravityRoomControl : MonoBehaviour {
	
	public GameObject rotationGameObj;
	public float rotationRate = 20f;
	
	private Vector3 rotationPos;
	private Vector3 rotationAxis;
	
	// Use this for initialization
	void Start () {
		rotationPos = rotationGameObj.transform.position;
		rotationAxis = transform.forward;
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround (rotationPos,rotationAxis,rotationRate * Time.deltaTime);
	}
	
	/*
	void OnTriggerStay(Collider col)
	{
		
		// adjust orientation based on the new platform - almost works
		// TODO: player stays put if still, but need to look at player control inputs when moving - not quite right
		Vector3 axis = Vector3.Cross(-col.transform.up,-transform.up);
		if(axis != Vector3.zero)
		{
			float angle = Mathf.Atan2(Vector3.Magnitude(axis), Vector3.Dot(-col.transform.up,-transform.up));
			col.transform.RotateAround(axis,angle);
		}
		
		// apply artificial gravity
		Vector3 gravVec = transform.up * -9.81f;
		col.rigidbody.AddForce (gravVec,ForceMode.Acceleration);
	}
	*/
	
	void OnTriggerEnter(Collider col)
	{
		// set the parent of the object to the elevator
		//col.transform.parent = transform;
		//col.transform.rotation = transform.rotation;
		//col.rigidbody.useGravity = false;
		col.rigidbody.drag = 50f;
	}
	
	void OnTriggerExit(Collider col)
	{
		// reset the parent of the object
		//col.transform.parent = null;		
		//col.rigidbody.useGravity = true;
		col.rigidbody.drag = 1f;
	}
	
}
