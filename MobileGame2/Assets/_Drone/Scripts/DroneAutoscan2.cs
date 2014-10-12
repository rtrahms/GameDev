using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// more efficient scanner - uses tags instead of raytracing
public class DroneAutoscan2 : MonoBehaviour {
	
	public float scanRange = 5000f;
	
	[HideInInspector]
	public List<GameObject> targets;
			
	// Use this for initialization
	void Start () {
		
		targets = new List<GameObject>();
		
	}
		
	void FixedUpdate () {
	
		RaycastHit hitInfo;
		
		// get all player objects in the scene
		GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
		
		// scan for new targets in range
		foreach (GameObject go in playerObjects)
		{
			if ((Vector3.Distance(transform.position,go.transform.position) <= scanRange) && (!targets.Contains(go)))
			{				
				// if player object close, and not already in list, add it
				targets.Add(go);
			}
		}
		
		if (targets.Count > 0)
		{
			// remove targets out of range
			foreach (GameObject go in targets)
			{
				if (Vector3.Distance(transform.position,go.transform.position) > scanRange)
				{
					targets.Remove(go);
					if (targets.Count <= 0)
						break;
				}
			}
		}
	}
	
}
