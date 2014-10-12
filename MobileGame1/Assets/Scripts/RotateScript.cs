using UnityEngine;
using System.Collections;

public class RotateScript : MonoBehaviour {

	public float degRate_X;
	public float degRate_Y;
	public float degRate_Z;
	public bool randomRotate = false;
	
	// Use this for initialization
	void Start () {
		if (randomRotate)
		{
			degRate_X = Random.Range (-90.0f, 90.0f);
			degRate_Y = Random.Range (-90.0f, 90.0f);
			degRate_Z = Random.Range (-90.0f, 90.0f);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		transform.Rotate (new Vector3(degRate_X*Time.deltaTime,
									  degRate_Y*Time.deltaTime,
									  degRate_Z*Time.deltaTime));
	}
}
