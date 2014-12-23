using UnityEngine;
using System.Collections;

public class TitleMenu : MonoBehaviour {

	public GUISkin skin;
	private AdMobHandler admobHandler;
	
#if UNITY_ANDROID
	//private AdMobHandler admob;
#endif
		
	void Start()
	{
#if UNITY_ANDROID
		//admob = GameObject.Instantiate (admobHandler) as AdMobHandler;
		admobHandler = GameObject.FindObjectOfType<AdMobHandler>();	
#endif
	}
		
	void OnGUI()
	{
		GUI.skin = skin;
		
		GUI.Label (new Rect(10,10,400,60), "Lander Challenge", skin.GetStyle ("HighScores"));
#if UNITY_ANDROID
#else	
		GUI.Label (new Rect(450,150,400,60), "Controls:", skin.GetStyle ("HighScores"));
		GUI.Label (new Rect(450,200,400,60), "Left-Right Thrusters: A/D Keys", skin.GetStyle ("HighScores"));
		GUI.Label (new Rect(450,250,400,60), "Main Booster: Space bar", skin.GetStyle ("HighScores"));
		GUI.Label (new Rect(450,300,400,60), "Finish Level: B key", skin.GetStyle ("HighScores"));
#endif	
		
		if (PlayerPrefs.HasKey ("CurrentLevel") && (PlayerPrefs.GetInt ("CurrentLevel") > 1))
		{
			if (GUI.Button(new Rect(10,150,200,45), "Resume Last Game"))
			{
				PlayerPrefs.SetFloat ("CurrentScore",0.0f);
				
				// (use saved spawn percentages)
				
				Application.LoadLevel(1);
			}
		}
		if (GUI.Button(new Rect(10,100,200,45), "New Game"))
		{
#if UNITY_ANDROID
			// pop up an Ad first
			admobHandler.ShowInterstitialAd();
#endif
			
			PlayerPrefs.SetInt ("CurrentLevel",1);
			PlayerPrefs.SetFloat ("CurrentScore",0.0f);
			
			// reset spawn percentages
			PlayerPrefs.SetFloat ("ChanceOfPlatformSpawn", 0.5f);
			PlayerPrefs.SetFloat ("ChanceOfBoulderSpawn", 0.5f);
			PlayerPrefs.SetFloat ("ChanceOfFastLaserSpawn", 0.05f);
			PlayerPrefs.SetFloat ("ChanceOfSlowLaserSpawn", 0.3f);
			PlayerPrefs.SetFloat ("ChanceOfFuelSpawn",0.1f);
			PlayerPrefs.SetFloat ("ChanceOfShieldSpawn",0.1f);
			
			Application.LoadLevel(1);
		}
		if (GUI.Button(new Rect(10,200,200,45), "High Scores"))
		{
			Application.LoadLevel("HighScores");
		}
		
		if (GUI.Button(new Rect(10,250,200,45), "Quit"))
		{
			print("I am quitting!");
			Application.Quit();
		}
	}
	
}
