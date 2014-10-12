using UnityEngine;
using System.Collections;

public class RepairShields : MonoBehaviour {

	public float shieldBoost = 10.0f;
	
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
			pc.adjustHealth(shieldBoost);
			Destroy (gameObject);
		}
	}	
}
