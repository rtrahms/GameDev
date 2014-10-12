using UnityEngine;
using System.Collections;

public class NavigationControls : MonoBehaviour {

	private bool isVisible;
	
	// Use this for initialization
	void Start () {
		isVisible = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKeyDown (KeyCode.N))
		{
			isVisible = !isVisible;
		}
		transform.gameObject.SetActive(true); // = isVisible;
	}
}
