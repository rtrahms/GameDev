using UnityEngine;
using System.Collections;

public class UpdateUIValues : MonoBehaviour {

	public GameObject playerShip;
	public GameObject targetingSystem;
	
	public UILabel positionValue;
	public UILabel speedValue;
	public UITexture missileLock;
	public UISlider shieldLevel;
	public UISlider weaponCharge;
	public UILabel cruiseMode;
	
	public GameObject navCommDialog;
	
	private ShipStatus status;
	private MissileFire firingSystem;
	private bool showNavComm;
	private float stateTimerSec;
	private float startTime;
	private SmoothMouseLook mouseControl;
	
	// Use this for initialization
	void Start () {
		status = playerShip.GetComponent<ShipStatus>();
		firingSystem = targetingSystem.GetComponent<MissileFire>();
		mouseControl = playerShip.GetComponent<SmoothMouseLook>();
		
		showNavComm = false;
		stateTimerSec = 0.25f;
		startTime = Time.time;		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		positionValue.text = status.position.ToString();
		speedValue.text = string.Format("{0:000}",status.speed);
		shieldLevel.value = status.shields;
		weaponCharge.value = firingSystem.weaponChargePercent;
		missileLock.enabled = firingSystem.targetLock;
		cruiseMode.enabled = status.cruiseMode;				
		
		if ((Input.GetKey(KeyCode.N)) && (Time.time - startTime > stateTimerSec))
		{
			showNavComm = !showNavComm;
			
			mouseControl.enabled = !showNavComm;
			firingSystem.enabled = !showNavComm;
			
			Screen.showCursor = showNavComm;
			navCommDialog.SetActive(showNavComm);
			startTime = Time.time;
		}
	}
	
}
