using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TransporterRegistry : MonoBehaviour {

	public float minRange;
	
	private List<Transporter> transporters;
	
	
	// Use this for initialization
	void Start () {
		transporters = new List<Transporter>();
		buildTransporterList ();
	}

	private void buildTransporterList()
	{
		GameObject[] gameObjs = GameObject.FindGameObjectsWithTag("Transporter");
		
		foreach (GameObject go in gameObjs)
		{
			print ("Added transporter at " + go.transform.position);
			transporters.Add(go.GetComponent<Transporter>());
		}
	}
		
	public void Add(Transporter newOne)
	{
		newOne.tag = "Transporter";
		transporters.Add(newOne);
	}
	
	
	public Transporter findNearestAvailable(Transporter reqTransporter)
	{
		float shortestDist = 1e7f;
		Transporter nearestTransporter = null;
		
		foreach (Transporter t in transporters)
		{
			float distance = Vector3.Distance(t.transform.position,reqTransporter.transform.position);
			if ((distance < shortestDist) && (t != reqTransporter))
			{
				shortestDist = distance;
				nearestTransporter = t;
			}
		}
		
		return nearestTransporter;
	}
}
