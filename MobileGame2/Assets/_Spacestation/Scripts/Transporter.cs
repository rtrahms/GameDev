using UnityEngine;
using System.Collections;

public class Transporter : MonoBehaviour {

	private TransporterRegistry registry;
	public bool available = true;
	public GameObject ring;
	private Transporter destination = null;
		
	// Use this for initialization
	void Start () {
		registry = GameObject.FindGameObjectWithTag("TransporterRegistry").GetComponent<TransporterRegistry>();
		ring.renderer.material.color = Color.green;
		destination = null;
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnTriggerEnter(Collider col)
	{
	
		available = false;
		ring.renderer.material.color = Color.red;
		col.transform.parent = transform;
		
		destination = registry.findNearestAvailable(this);
		
		// need to give callback to player in transporter
		if (col.tag == "Player")
		{
			PlayerControl pc = col.GetComponent<PlayerControl>();
			pc.transporter = this;
		}
	}
	
	void OnTriggerExit(Collider col)
	{
		available = true;
		ring.renderer.material.color = Color.green;
		destination = null;
		
		// pass parenting to containing object, or null if no containing object
		col.transform.parent = transform.parent;
	}
	
	public void Activate(GameObject go) {
		if (destination != null)
		{
			// set parent to other transporter
			go.transform.position = destination.transform.position;
			go.transform.rotation = destination.transform.rotation;
			go.transform.parent = destination.transform;
		}
	}
	
}
