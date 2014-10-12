using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public float maxLatSpeed = 5.0f;
	public float maxBoost = 15.0f;
	
	public float latAcc = 0.5f;
	public float boostAcc = 1.0f;
	
	public float maxHealth = 100.0f;
	public float maxFuel = 1000.0f;
	public float dangerousVertVel = 20.0f;

	public ParticleSystem booster;
	public ParticleSystem leftThrust;
	public ParticleSystem rightThrust;
	public ParticleSystem explosion;
	
	public GameObject touchPanelControlGroup;
	public GUITexture boosterButton;
	public GUITexture leftButton;
	public GUITexture rightButton;
	public GUITexture baseButton;
	
	private TouchButtonLogic boosterTouch;
	private TouchButtonLogic leftTouch;
	private TouchButtonLogic rightTouch;
	private TouchButtonLogic baseTouch;
	
	private bool boosterOn;
	private bool moveLeft, moveRight;
	private bool setupBase;
	private bool levelComplete;
	private bool died;
	private float latSpeed;
	private float boost;
	private float landerHealth;
	private float vertVel;
	private float fuel;
	private float oldYPos;
	
	private GameManager gameMgr;
	private GameObject[] deathWalls;
		
	// audio clips
	public AudioClip[] audioClips;	
	
	void Awake() {
		boosterOn = false;
		moveLeft = false;
		moveRight = false;
		setupBase = false;
		vertVel = 0.0f;
		fuel = maxFuel;
		landerHealth = maxHealth;
	}
	
	// Use this for initialization
	void Start () {
		deathWalls = GameObject.FindGameObjectsWithTag("DeathWall");
		gameMgr = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
				
#if UNITY_ANDROID
		touchPanelControlGroup.SetActive(true);
		boosterTouch = boosterButton.GetComponent<TouchButtonLogic>();
		leftTouch = leftButton.GetComponent<TouchButtonLogic>();
		rightTouch = rightButton.GetComponent<TouchButtonLogic>();
		baseTouch = baseButton.GetComponent<TouchButtonLogic>();
#else
		touchPanelControlGroup.SetActive(false);
#endif	
		
		levelComplete = false;
		died = false;
		
		oldYPos = transform.position.y;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		if (!levelComplete)
		{
#if UNITY_ANDROID
			CheckTouchPanel();
#else
			CheckKeyboardInputs();
#endif
		}
		
		// thruster right moves left!
		if ((moveRight) && (fuel > 0))
		{
			latSpeed = adjustSpeed (latSpeed, latAcc, maxBoost, true);
			rightThrust.Play();
		}		
		// thruster left moves right!
		else if ((moveLeft) && (fuel > 0))
		{
			latSpeed = adjustSpeed (latSpeed,-latAcc,-maxBoost, false);	
			leftThrust.Play ();	
		}
		else 
		{
			latSpeed = 0.0f;
			rightThrust.Stop ();
			leftThrust.Stop ();
		}
				
		// lander boost toggle (upward)
		if (boosterOn && (fuel > 0))
		{
			boost = adjustSpeed (boost,boostAcc,maxBoost, true);
			booster.Play ();
			if (!audio.isPlaying)
				PlaySound (0);
			
			adjustFuel (-boostAcc);
		}
		else if ((!levelComplete) && (!died))
		{
			audio.Stop();
			booster.Stop ();
			boost = 0.0f;
		}
		
				
		Vector3 forceVec = new Vector3();
		forceVec = transform.right * latSpeed + transform.forward * boost;
		//forceVec = transform.right * latSpeed + transform.up * boost;
		rigidbody.AddForce (forceVec);
		
		// adjust the height of death walls on left and right sides
		foreach (GameObject dw in deathWalls)
		{
			dw.transform.position = new Vector3(dw.transform.position.x, 
													transform.position.y,
												dw.transform.position.z);
		}

		// update vertical velocity
		float newYPos = transform.position.y;
		vertVel = (newYPos - oldYPos)/Time.deltaTime;
		
		// update Game Manager on stats
		gameMgr.altitude = newYPos;
		gameMgr.fuel = fuel;
		gameMgr.health = landerHealth;
		gameMgr.vertVel = vertVel; 
		oldYPos = newYPos;
		
		// does the player want to stop the level with extra fuel and health?
		if (setupBase && (vertVel < 0.01f) && (!levelComplete)) {		
			// do this once at end of level
			CelebrateComplete();
			CompleteLevel ();
		}		
	}
	
#if UNITY_ANDROID
	void CheckTouchPanel()
	{
		if (leftTouch.isTouched ())
		{
			moveLeft = true;
		}
		else if (rightTouch.isTouched ())
		{
			moveRight = true;
		}
		else
		{
			moveRight = false;
			moveLeft = false;
		}
		
		if (boosterTouch.isTouched())
		{
			boosterOn = true;
		}
		else
		{
			boosterOn = false;
		}
		
		if (baseTouch.isTouched())
		{
			setupBase = true;
		}
		
	}
#else
	void CheckKeyboardInputs()
	{
		if (Input.GetKey (KeyCode.D))
		{
			moveRight = true;
		}
		else if (Input.GetKey (KeyCode.A))
		{
			moveLeft = true;
		}
		else
		{
			moveRight = false;
			moveLeft = false;
		}
		
		if (Input.GetKey (KeyCode.Space))
		{
			boosterOn = true;
		}
		else
		{
			boosterOn = false;
		}
		
		if (Input.GetKey(KeyCode.B))
		{
			setupBase = true;
		}
		
	}
#endif
		
	
	public void adjustHealth(float healthAdjust)
	{
		landerHealth += healthAdjust;
		if (landerHealth > maxHealth)
		{
			landerHealth = maxHealth;
		}
		
		if (landerHealth <= 0.0f)
		{
			landerHealth = 0.0f;
			Die ();
		}
	}
	
	public void adjustFuel(float addAmount)
	{
		fuel += addAmount;
		
		if (addAmount > 0)
			PlaySound (1);	
		
		if (fuel <= 0f)
		{
			fuel = 0f;
		}
	}
		
	float adjustSpeed(float speed, float accel, float maxSpeed, bool increase)
	{
		float newSpeed = speed + accel;
		
		if ((increase) && (newSpeed > maxSpeed))
		{
			newSpeed = maxSpeed;
		}
		if ((!increase) && (newSpeed < maxSpeed))
		{
			newSpeed = maxSpeed;
		}
		
		return newSpeed;
	}
	
	void PlaySound(int clipIndex)
	{
		audio.clip = audioClips[clipIndex];
		audio.Play();
	}
	
	void OnCollisionEnter(Collision col)
	{
		// collision at too fast a vertical velocity
		float velDiff = Mathf.Abs(vertVel) - dangerousVertVel;
		float multipler = 5.0f;
		if (velDiff > 0.0f)
		{
			// adjust health by difference
			adjustHealth (-velDiff*multipler);
		}
	}
	
	public void Die()
	{
		died = true;

		// play explosion particle effect
		explosion.Play();
		
		// play explosion sound
		PlaySound (3);
		
		// disable mesh
		renderer.enabled = false;
		
		StartCoroutine(ReloadTitle());		
	}
	
	void CompleteLevel()
	{
		// mark the score and end the level and notify the gameManager of level complete
		gameMgr.playerScore += oldYPos;
		gameMgr.levelComplete = true;
		levelComplete = true;		
	}
	
	void CelebrateComplete() 
	{
		print ("You made it!");
		//if (!audio.isPlaying)
		//{
			PlaySound (2);
		//}
	}
		
	IEnumerator ReloadTitle() 
	{
		yield return new WaitForSeconds(0.5f);
		GameManager gameMgr = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		gameMgr.LoadTitle();
	}
		
}
