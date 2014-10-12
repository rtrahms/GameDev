using UnityEngine;
using System.Collections;

public class DeathFloor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) 
	{
		// Player Died!  Notify player controller
		if (other.gameObject.tag == "Player")
		{
			PlayerControl pc = other.gameObject.GetComponent<PlayerControl>();
			pc.Die ();
		}		
	}
}
