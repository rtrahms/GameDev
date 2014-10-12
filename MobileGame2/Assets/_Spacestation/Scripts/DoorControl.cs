using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Animator))]

public class DoorControl : MonoBehaviour {

	private Animator anim;
	
	// Use this for initialization
	void Awake () {
		anim = GetComponent<Animator>();	
		anim.StopPlayback ();
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter() {
		print ("entered trigger volume");
		anim.SetBool ("OpenCommand",true);
		//anim.Play("DoorOpen1");
		//anim.StopPlayback ();
	}
	
	void OnTriggerExit() {
		print ("exited trigger volume");
		anim.SetBool ("OpenCommand",false);
		//anim.Play ("DoorClose1");
		//anim.StopPlayback ();
	}
}
