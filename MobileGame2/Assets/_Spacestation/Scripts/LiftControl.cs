using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Animator))]

public class LiftControl : MonoBehaviour {

	public float floorPauseSec = 8f;
	
	private Animator anim;
	private float startTime;
	private float floorNumber;
	
	void Awake() {
		anim = GetComponent<Animator>();
	}
	
	// Use this for initialization
	void Start () {
		floorNumber = 1f;
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if ((Time.time - startTime) > floorPauseSec)
		{
			print ("floorNum = " + floorNumber);
			anim.SetInteger ("FloorNumber",(int) floorNumber);
			startTime = Time.time;
			floorNumber = (floorNumber + 1f) % 5f;
			if (floorNumber == 0f)
				floorNumber = 1f;
		}
	}
	
	void OnTriggerEnter(Collider col)
	{
		// set the parent of the object to the elevator
		col.transform.parent = transform;
		col.transform.Rotate (0,0,0);
		col.transform.Translate (new Vector3(0,1,0));
	}
	
	void OnTriggerExit(Collider col)
	{
		// reset the parent of the object
		col.transform.parent = null;
	}
	
}
