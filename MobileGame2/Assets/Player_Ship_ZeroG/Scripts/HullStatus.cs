using UnityEngine;
using System.Collections;

public class HullStatus : MonoBehaviour {

	public GameObject shipControl;
	private ShipStatus shipStats;
	
	// Use this for initialization
	void Start () {
		shipStats = shipControl.GetComponent<ShipStatus>();
	}
		
	public void adjustHealth(float value)
	{
		float newValue = shipStats.shields + value * Time.deltaTime;
		if (newValue < 0f)
			newValue = 0f;
			
		shipStats.shields = newValue;
	}
}
