using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GalaxyMap : MonoBehaviour {

	public GameObject stationPrefab;
	public GameObject msPrefab;
	
	public Vector3 dimensions;
	public int numStations;
	public int numMotherships;
	public float hazardMultiplier;
	public float minDistanceBetweenStations = 1000f;
	public float maxDistanceForMotherships = 1000f;
	public float refreshRateSec = 5f;
	
	public List<StationInfo> stations;
	public List<MothershipInfo> motherships;
	
	private float startTime;
		
	// Use this for initialization
	void Start () {
		stations = CreateStations ();	
		motherships = CreateMotherships();
		
		startTime = Time.time;
	}
		
	private List<StationInfo> CreateStations()
	{
		List<StationInfo> stationList = new List<StationInfo>();
		
		print ("Preparing to add " + numStations + " stations");
		
		for (int i=0; i<numStations; i++)
		{
			StationInfo myStation = new StationInfo();
			
			Vector3 possibleLocation = Vector3.zero;
			bool goodStationLocation = false;
			//while (!goodStationLocation) {
				possibleLocation = new Vector3(Random.Range(-dimensions.x/2f, dimensions.x/2f),
												Random.Range(-dimensions.y/2f, dimensions.y/2f),
												Random.Range(-dimensions.z/2f, dimensions.z/2f));
			//	goodStationLocation = CheckLocation(possibleLocation,stationList);
			//}
			myStation.location = possibleLocation;
			myStation.go = Instantiate (stationPrefab,myStation.location,Quaternion.identity) as GameObject;
			
			stationList.Add(myStation);
			print ("Added station #" + i + " at " + myStation.location);
		}
		
		return stationList;
	}
	
	private bool CheckLocation(Vector3 newLocation, List<StationInfo> currentStations)
	{
		if (currentStations.Count == 0)
			return true;
			
		foreach (StationInfo s in currentStations)
		{
			print ("checking location of " + newLocation);
			
			if (Vector3.Distance (s.location,newLocation) < minDistanceBetweenStations)
				return false;
		}
		
		return true;
	}
	
	private List<MothershipInfo> CreateMotherships()
	{
		List<MothershipInfo> msList = new List<MothershipInfo>();
		
		print ("Preparing to add " + numMotherships + " motherships");
		
		for (int i=0; i < numMotherships; i++)
		{
			MothershipInfo msInfo = new MothershipInfo();
			
			int stationIndex = Random.Range (0,stations.Count-1);
			
			Vector3 msLoc = stations[stationIndex].location;
			msLoc.x += Random.Range(-maxDistanceForMotherships,maxDistanceForMotherships);
			msLoc.y += Random.Range(-maxDistanceForMotherships,maxDistanceForMotherships);
			msLoc.z += Random.Range(-maxDistanceForMotherships,maxDistanceForMotherships);
			
			msInfo.location = msLoc;
			msInfo.go = Instantiate (msPrefab,msInfo.location,Quaternion.identity) as GameObject;
			
			msList.Add (msInfo);
			print ("Added mothership #" + i + " at " + msInfo.location);
			
		}

		return msList;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Time.time - startTime > refreshRateSec)
		{
			// clean up lists (remove destroyed objects
			for (int i=0; i<stations.Count; i++)
			{
				if (stations[i] == null)
					stations.RemoveAt (i);
			}
			
			for (int i=0; i<motherships.Count; i++)
			{
				if (motherships[i] == null)
					motherships.RemoveAt (i);
			}
			
			startTime = Time.time;
		}
	}
}
