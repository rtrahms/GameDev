using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DroneAutoscan : MonoBehaviour {

	private List<Vector3> scanVectors;
	
	[HideInInspector]
	public List<GameObject> targets;
	
	public float scanRange = 100f;
	public float axialRangeDeg = 360f;
	public int axialVectors = 60;
	public float tiltRangeDeg = 60f;
	public int tiltVectors = 20;
	
	private int numScanVectors;
	private float degPerAxialVec;
	private float degPerTiltVec;
	
	// Use this for initialization
	void Start () {
		numScanVectors = axialVectors * tiltVectors;
		degPerAxialVec = axialRangeDeg/axialVectors;
		degPerTiltVec = tiltRangeDeg/tiltVectors;
		
		scanVectors = new List<Vector3>();
		targets = new List<GameObject>();
		
		populateScanList();
	}
	
	private void populateScanList()
	{
		Vector3 scanVector = new Vector3();
		int axialLimit = (int)(axialVectors/2) - 1;
		int tiltLimit = (int)(tiltVectors/2) - 1;
		for (int i=-axialLimit; i <= axialLimit; i++)
		{
			for (int j=-tiltLimit; j <= tiltLimit; j++)
			{
				scanVector = Quaternion.Euler ((float)j*degPerTiltVec,(float)i*degPerAxialVec, 0) * transform.forward;
				scanVectors.Add(scanVector);
				
			}
		}
	}
	
	void FixedUpdate () {
	
		RaycastHit hitInfo;
		
		// scan for new targets in range
		foreach (Vector3 v in scanVectors)
		{
			//Debug.DrawRay (transform.position, v * scanRange, Color.red);
			if (Physics.Raycast (transform.position,v*scanRange,out hitInfo))
			{
				GameObject go = hitInfo.collider.gameObject;
				
				// if player object seen, and not already in list, add it
				if ((go.tag == "Player") && (!targets.Contains(go)))
				{
					print ("scanner added target to list");
					targets.Add(go);
				}
			}
		}
		
		if (targets.Count > 0)
		{
			// remove targets out of range
			foreach (GameObject go in targets)
			{
				if (Vector3.Distance(transform.position,go.transform.position) > scanRange)
				{
					print ("scanner removed target to list");
					targets.Remove(go);
					if (targets.Count <= 0)
						break;
				}
			}
		}
	}
	
}
