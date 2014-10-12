using UnityEngine;
using System.Collections;

public class Refuel : MonoBehaviour {

	public float fuelTank = 500.0f;
		
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{	
			PlayerControl pc = other.gameObject.GetComponent<PlayerControl>();
			pc.adjustFuel(fuelTank);
			Destroy (gameObject);
		}
	}	
}
