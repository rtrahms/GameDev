using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnSaucer : MonoBehaviour {

	public GameObject dronePrefab;
	public int fleetSize = 25;
	public float timeBetweenSpawnSec = 45.0f;
		
	private List<GameObject> drones;
	private float startTime;
	
	// Use this for initialization
	void Start () {
		drones = new List<GameObject>();
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (((Time.time - startTime) > timeBetweenSpawnSec) && (drones.Count < fleetSize))
		{
			SpawnDrone();
			RefreshList();
			startTime = Time.time;
			print ("fleet size: " +  drones.Count);
		}
	}
	
	void SpawnDrone() {
		
		GameObject drone = Instantiate (dronePrefab, transform.position, Quaternion.identity) as GameObject;
		drones.Add(drone);
	}
	
	// remove drones that have been destroyed (are null)
	void RefreshList() {
		for (int i=drones.Count-1; i> -1; i--)
		{
			if (drones[i] == null)
				drones.RemoveAt (i);
		}
	}
	
}
