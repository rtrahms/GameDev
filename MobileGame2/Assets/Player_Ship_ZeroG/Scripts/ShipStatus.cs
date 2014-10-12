using UnityEngine;
using System.Collections;

public class ShipStatus : MonoBehaviour {

	public float shields;
	public float shieldRechargeRate = 0.01f;
	public float weaponRecharge;
	public float targetLock;
	public Vector3 position;
	public float speed;
	public bool cruiseMode;
	
	private float maxShields = 1f;
	
	private Vector3 lastPos;
	private PlayerControlBasic3 player;
	
	// Use this for initialization
	void Start () {
		lastPos = transform.position;
		player = GetComponent<PlayerControlBasic3>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		position = transform.position;
		
		Vector3 diffVec = position - lastPos;
		float dir = Vector3.Dot (diffVec.normalized,transform.forward);
		float mag = diffVec.magnitude;
		
		speed = (dir * mag)/Time.deltaTime;	
		lastPos = position;
		
		cruiseMode = player.cruiseControl;
		
		// if shields are low, recharge
		if (shields < maxShields)
			shields += (shieldRechargeRate * Time.deltaTime);
		if (shields > maxShields)
			shields = maxShields;			
	}

}
