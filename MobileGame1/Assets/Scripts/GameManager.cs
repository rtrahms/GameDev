using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GUISkin skin;
	public float levelTime = 200.0f;
	public bool levelComplete = false;
	public bool highScoreAchieved = false;
	public bool highLevelAchieved = false;
	public float deltaPercentage = 0.05f;
	
	
	Rect levelRect;
	Rect timerRect;
	Rect altitudeRect;
	Rect vertVelRect;
	Rect fuelRect;
	Rect healthRect;
	Rect completeBoxRect1, completeBoxRect2;
	Rect nextLevelButtonRect;
	Rect returnToMenuButtonRect;
	Rect warningRect;

	// audio clips
	public AudioClip[] audioClips;
		
	// set by the player & tracker
	public float playerScore;
	public float altitude;
	public float vertVel;
	public float fuel = 1000.0f;
	public float health = 100.0f;
	private float timer;
	
	private int currentLevel;
	private string levelStr;
	private string altitudeStr;
	private string vertVelStr;
	private string fuelStr;
	private string healthStr;
	private string timerStr;
	
	private bool warning = false;
	
	void Start() {
	
		playerScore = PlayerPrefs.GetFloat ("CurrentScore");
		currentLevel = PlayerPrefs.GetInt ("CurrentLevel");
	
		print("currentLevel = " + currentLevel + " playerScore = " + playerScore);
		
		int spacing = Screen.width/5;
		
		levelRect = new Rect(Screen.width/2-100,10,200,50);
		timerRect = new Rect(10,60,200,50);
		altitudeRect = new Rect(10+spacing,60,200,50);
		vertVelRect = new Rect(10+(spacing*2),60,200,50);
		fuelRect = new Rect(10+(spacing*3),60,200,50);
		healthRect = new Rect(10+(spacing*4),60,200,50);
		warningRect = new Rect(Screen.width/2 - 100, 120, 200, 50);
		completeBoxRect1 = new Rect(Screen.width/2-200,Screen.height/2-200, 200,50);
		completeBoxRect2 = new Rect(Screen.width/2-200,Screen.height/2-150, 200,50);
		nextLevelButtonRect = new Rect(Screen.width/2-200,Screen.height/2-100, 200, 50);
		returnToMenuButtonRect = new Rect(Screen.width/2-200,Screen.height/2, 200, 50);
		
		warning = false;
		timer = levelTime;
	}
	
	// Update is called once per frame
	void Update () {
		if (!levelComplete) 
		{
			timer -= Time.deltaTime;
			if (timer <= 0.0f)
			{
				timer = 0.0f;
				levelComplete = true;
				Time.timeScale = 0;
			}
			
			levelStr = "Level " + currentLevel.ToString ();
			timerStr = string.Format("{0:0.0}",timer);
			altitudeStr = "alt: " + string.Format("{0:0.0}",altitude);
			vertVelStr = "vertVel: " + string.Format ("{0:0.0}",vertVel);
			fuelStr = "fuel: " + string.Format ("{0:0.0}",fuel);
			healthStr = "health: " + string.Format ("{0:0.0}",health);
			
			// warn if low fuel
			if ((fuel > 0.0f) && (fuel < 300.0f))
			{
				if (!audio.isPlaying)
					PlaySound (0);
				warning = true;
			}	
			else if (fuel <= 0.0f)
			{
				if (!audio.isPlaying)
					PlaySound (2);
				warning = true;
			}
			else if (health < 20.0f)
			{
				if (!audio.isPlaying)
					PlaySound (1);
				warning = true;
			}
			else
			{
				warning = false;
			}		
		}	
		else 
		{
			if (!highScoreAchieved)
			{
				// level is complete - how did we do on score?
				highScoreAchieved = EvaluateIfHigh("High Score", playerScore);
			}
			
			if (!highLevelAchieved)
			{
				// level is complete - how did we do on score?
				highLevelAchieved = EvaluateIfHigh("High Level", (float) currentLevel);
			}
		}
	}
	
	void OnGUI() {
		GUI.skin = skin;
		
		GUI.Label (levelRect,levelStr,skin.GetStyle("Status"));
		GUI.Label (timerRect,timerStr,skin.GetStyle("Status"));
		GUI.Label (altitudeRect,altitudeStr,skin.GetStyle("Status"));
		GUI.Label (vertVelRect,vertVelStr,skin.GetStyle("Status"));
		GUI.Label (fuelRect,fuelStr,skin.GetStyle("Status"));
		GUI.Label (healthRect, healthStr,skin.GetStyle("Status"));
		
		if (warning)
		{
			GUI.Label (warningRect,"WARNING",skin.GetStyle("Warning"));
		}
		
		if (levelComplete)
		{
			ShowCompleteDialog();
		}
		
	}
	
	void ShowCompleteDialog()
	{
	
		if (highScoreAchieved)
			GUI.Box(completeBoxRect1,"NEW HIGH SCORE = " + string.Format("{0:0.0}",playerScore), skin.GetStyle("Score"));
		else
			GUI.Box(completeBoxRect1,"Score = " + string.Format("{0:0.0}",playerScore), skin.GetStyle("Score"));
					
		if (GUI.Button (nextLevelButtonRect,"Next Level"))
		{
			LoadNextLevel ();
		}
		
		if (GUI.Button (returnToMenuButtonRect,"Return to Main Menu"))
		{
			LoadTitle ();
		}
	}	

	bool EvaluateIfHigh(string keyValStr, float currentValue)
	{
		float highVal = -100.0f;
				
		if (PlayerPrefs.HasKey (keyValStr))
		{			
			highVal = PlayerPrefs.GetFloat (keyValStr);
			
			if (currentValue > highVal)
			{
				print("new high value for " + keyValStr + " = " + currentValue);
				
				// new high value!
				highVal = currentValue;
				PlayerPrefs.SetFloat (keyValStr, highVal);
				return true;
			}
			else
				// thanks for playing - NOT high score
				return false;
		}
		else
		{
			// first time - default high score!
			print("first time high values for " + keyValStr + " = " + currentValue);
			highVal = currentValue;
			PlayerPrefs.SetFloat (keyValStr, highVal);
			return true;
		}
	}
		
	void PlaySound(int clipIndex)
	{	
		audio.clip = audioClips[clipIndex];
		audio.Play();			
	}
	
	void LoadNextLevel()
	{		
		// update latest level number
		currentLevel++;
		PlayerPrefs.SetInt ("CurrentLevel",currentLevel);
		
		// make the percentages more difficult
		float chanceOfPlatformSpawn = PlayerPrefs.GetFloat ("ChanceOfPlatformSpawn") - deltaPercentage;
		float chanceOfBoulderSpawn = PlayerPrefs.GetFloat ("ChanceOfBoulderSpawn") + deltaPercentage;
		float chanceOfFastLaserSpawn = PlayerPrefs.GetFloat ("ChanceOfFastLaserSpawn") + deltaPercentage;
		float chanceOfSlowLaserSpawn = PlayerPrefs.GetFloat ("ChanceOfSlowLaserSpawn") - deltaPercentage;
		float chanceOfFuelSpawn = PlayerPrefs.GetFloat ("ChanceOfFuelSpawn") - deltaPercentage;
		float chanceOfShieldSpawn = PlayerPrefs.GetFloat ("ChanceOfShieldSpawn") - deltaPercentage;
		
		PlayerPrefs.SetFloat ("ChanceOfPlatformSpawn", chanceOfPlatformSpawn);
		PlayerPrefs.SetFloat ("ChanceOfBoulderSpawn", chanceOfBoulderSpawn);
		PlayerPrefs.SetFloat ("ChanceOfFastLaserSpawn", chanceOfFastLaserSpawn);
		PlayerPrefs.SetFloat ("ChanceOfSlowLaserSpawn", chanceOfSlowLaserSpawn);
		PlayerPrefs.SetFloat ("ChanceOfFuelSpawn",chanceOfFuelSpawn);
		PlayerPrefs.SetFloat ("ChanceOfShieldSpawn",chanceOfShieldSpawn);

		PlayerPrefs.SetFloat ("CurrentScore",playerScore);
		//Application.LoadLevel(currentLevel);
		Application.LoadLevel (1);
		Time.timeScale = 1;
	}
	
	public void LoadTitle()
	{
		//currentLevel = 0;
		//PlayerPrefs.SetInt ("CurrentLevel",currentLevel);			
		Application.LoadLevel(0);
		Time.timeScale = 1;
	}
	
}
