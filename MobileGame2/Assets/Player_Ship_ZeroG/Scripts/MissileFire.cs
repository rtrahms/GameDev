using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class MissileFire : MonoBehaviour {

	public GameObject missilePrefab;
	public GameObject spawnPoint;
	public float targetingRange = 1000f;
	public float targetLockTimeSec = 1.0f;
	public AudioClip lockSound;
	public AudioClip missileFireSound;
	public bool targetLock;
	public float weaponChargePercent;
	public float weaponDischargeRate = 0.2f;
	public float weaponRechargeRate = 0.2f;
	
	private bool lockJustAcquired;
	private float startTime;
	
	private GameObject target;
	
	// Use this for initialization
	void Start () {
		target = null;
		targetLock = false;
		lockJustAcquired = false;
		weaponChargePercent = 1.0f;
	}
	
	void Update () 
	{
		if ((Input.GetButtonDown ("Fire1")) && (weaponChargePercent > weaponDischargeRate))
		{
			GameObject go = Instantiate(missilePrefab, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;

			AudioSource.PlayClipAtPoint(missileFireSound,transform.position);
			
			MissileTravel mt = go.GetComponent<MissileTravel>();
			mt.targetGO = target;	
			
			weaponChargePercent -= weaponDischargeRate;
			if (weaponChargePercent < 0f)
				weaponChargePercent = 0f;
		}
		
		RechargeWeapon();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		Ray targetRay = new Ray(transform.position,transform.forward);
		RaycastHit hit;
		
		Debug.DrawRay (transform.position,transform.forward*1000f, Color.cyan);
		
		if ((Physics.Raycast (targetRay,out hit, targetingRange)) 
		    && ((hit.collider.gameObject.tag == "Enemy") || 
		    	(hit.collider.gameObject.tag == "Drone") ||
		 		(hit.collider.gameObject.tag == "Mothership")))
		{
			startTime = Time.time;
			target = hit.collider.gameObject;
			targetLock = true;
			
			if (lockJustAcquired)
			{
				AudioSource.PlayClipAtPoint(lockSound, transform.position);
				lockJustAcquired = false;
			}
		}
		else if ((Time.time - startTime) > targetLockTimeSec)
		{
			target = null;
			targetLock = false;
			lockJustAcquired = true;
			//targetLockTexture.enabled = false;
		}
	}	
	
	void RechargeWeapon()
	{
		if (weaponChargePercent < 1.0f)
		{
			weaponChargePercent += weaponRechargeRate * Time.deltaTime;
			if (weaponChargePercent > 1.0f)
				weaponChargePercent = 1.0f;
		}
	}	
}
