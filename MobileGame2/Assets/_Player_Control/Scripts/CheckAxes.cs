using UnityEngine;
using System.Collections;

public class CheckAxes : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawRay (transform.position,transform.up * 10f,Color.cyan);
		Debug.DrawRay (transform.position, transform.forward * 10f, Color.red);
	}
}
