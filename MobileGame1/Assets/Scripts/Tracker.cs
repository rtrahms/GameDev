using UnityEngine;
using System.Collections;

public class Tracker : MonoBehaviour {

	public GameObject platform;
	public GameObject fuelBeacon;
	public GameObject shields;
	public GameObject laserBot_fast;
	public GameObject laserBot_slow;
	public GameObject[] boulders;
	
	public float platformMinStep = 10.0f;
	public float chanceOfPlatformSpawn = 0.5f;
	public float chanceOfBoulderSpawn = 0.5f;
	public float chanceOfFastLaserSpawn = 0.1f;
	public float chanceOfSlowLaserSpawn = 0.1f;
	public float chanceOfFuelSpawn = 0.1f;
	public float chanceOfShieldSpawn = 0.1f;
	public float xRangeAbsolute = 30.0f;
	public float yRangeOffset = 10.0f;
	
	private float currentHeight;
	private float lastHeight;
	
	
	// Use this for initialization
	void Start () {
		currentHeight = 0f;
		lastHeight = platformMinStep;
				
		chanceOfPlatformSpawn = PlayerPrefs.GetFloat ("ChanceOfPlatformSpawn");
		chanceOfBoulderSpawn = PlayerPrefs.GetFloat ("ChanceOfBoulderSpawn");
		chanceOfFastLaserSpawn = PlayerPrefs.GetFloat ("ChanceOfFastLaserSpawn");
		chanceOfSlowLaserSpawn = PlayerPrefs.GetFloat ("ChanceOfSlowLaserSpawn");
		chanceOfFuelSpawn = PlayerPrefs.GetFloat ("ChanceOfFuelSpawn");
		chanceOfShieldSpawn = PlayerPrefs.GetFloat ("ChanceOfShieldSpawn");
	}
	
	// Update is called once per frame
	void Update () {
		currentHeight = transform.position.y;
		if (currentHeight > lastHeight)
		{
			 lastHeight = currentHeight + platformMinStep;
			 
			 checkSpawn(currentHeight,chanceOfPlatformSpawn, platform);
			 checkSpawn(currentHeight,chanceOfFuelSpawn, fuelBeacon);
			 checkSpawn(currentHeight,chanceOfShieldSpawn, shields);
			 checkSpawn(currentHeight,chanceOfFastLaserSpawn, laserBot_fast);
			 checkSpawn(currentHeight,chanceOfSlowLaserSpawn, laserBot_slow);
			 checkArraySpawn(currentHeight,chanceOfBoulderSpawn, boulders);
		}
	}
		//print ("Current Height = " + currentHeight);
	
	void checkSpawn(float height,float chanceOfSpawn, GameObject go)
	{
		float placePlatform = Random.Range (0f,1f); 
		if (placePlatform < chanceOfSpawn)
		{
			// place a new platform
			float xPos = Random.Range (-xRangeAbsolute,xRangeAbsolute);
			Vector3 objPos = new Vector3(xPos,height + yRangeOffset, 0f);
			
			Instantiate (go,objPos,Quaternion.identity);
		}
	}
	
	void checkArraySpawn(float height,float chanceOfSpawn, GameObject[] go_array)
	{
		float placePlatform = Random.Range (0f,1f); 
		if (placePlatform < chanceOfSpawn)
		{
			// Randomly pick one from the array
			int index = Random.Range(0,go_array.Length-1);
			
			// place a new platform
			float xPos = Random.Range (-xRangeAbsolute,xRangeAbsolute);
			Vector3 objPos = new Vector3(xPos,height + yRangeOffset, 0f);
			
			Instantiate (go_array[index],objPos,Quaternion.identity);
		}
	}
}
