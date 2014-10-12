using UnityEngine;
using System.Collections;

public class SpaceStationRotate : MonoBehaviour {

	public float speed = 15f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.Rotate (new Vector3(0,0,speed*Time.deltaTime));
	}
}
