using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {

	public GameObject scanner;
	public GameObject weapon;
	public float firingDistance = 50f;
	public float rotateSpeed = 10f;
	
	public float patrolDistance = 10f;
	public float turnDeg = 90f;
	public float speed = 25f;
	
	private Vector3 lastTurnPos;
	private DroneAutoscan2 targetScan;
	
	// Use this for initialization
	void Start () {
		targetScan = scanner.GetComponent<DroneAutoscan2>();
		weapon.SetActive(false);
		//lastTurnPos = transform.position;
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		if (targetScan.targets.Count > 0)
		{	
			FollowMode();
			
			//Debug.DrawRay (transform.position, transform.forward * 10f, Color.cyan);
			transform.position += (transform.forward * speed * Time.deltaTime);				
		}
		/*
		else
		{
			PatrolMode();
		}
		*/

	}
	
	void PatrolMode()
	{
		/*
		if (Vector3.Distance (transform.position,lastTurnPos) > patrolDistance)
		{
			// turn!
			transform.Rotate(transform.up,turnDeg);
			lastTurnPos = transform.position;
		}
		*/		
	}
	
	void FollowMode()
	{
		GameObject seenTarget = null;
		
		foreach (GameObject go in targetScan.targets)
		{
			// TODO: seenTarget just assigned to last gameobject in list for now
			seenTarget = go;
		}	
		
		if (seenTarget != null)
		{
			transform.LookAt(seenTarget.transform.position);
			
			if (Vector3.Distance(seenTarget.transform.position,transform.position) < firingDistance)
				weapon.SetActive (true);
			else
				weapon.SetActive (false);
		}
	}
	
}
