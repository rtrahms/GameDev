using UnityEngine;
using System.Collections;

public class HighScores : MonoBehaviour {

	// High score list
	
	public GUISkin skin;
	Rect labelRect;
	Rect highScoreRect;
	Rect highLevelRect;
	Rect returnToMainButtonRect;
	Rect clearScoresButtonRect;
	
	private float highScore = -100.0f;
	private float highLevel = -100.0f;
	
	// Use this for initialization
	void Start () {
		labelRect = new Rect(Screen.width/2-200, Screen.height/4, 200,50);
		highScoreRect = new Rect(Screen.width/2-200, Screen.height/4 + 60, 200,50);
		highLevelRect = new Rect(Screen.width/2-200, Screen.height/4 + 120, 200,50);
		clearScoresButtonRect = new Rect(Screen.width/2-200, Screen.height/4 + 180, 200,50);
		returnToMainButtonRect = new Rect(Screen.width/2-200, Screen.height/4 + 240, 200,50);
		
		if (PlayerPrefs.HasKey ("High Score"))
			highScore = PlayerPrefs.GetFloat ("High Score");
		else
			highScore = -100.0f;
			
		if (PlayerPrefs.HasKey ("High Level"))
			highLevel = PlayerPrefs.GetFloat ("High Level");
		else
			highLevel = -100.0f;
	}
	
	public float GetHighScore()
	{
		return highScore;
	}
		
	void OnGUI() {
		GUI.skin = skin;
		
		GUI.Label (labelRect,"Achievements",skin.GetStyle ("HighScores"));
		
		// TODO: GUI List for high scores
		
		// highest score
		if (highScore < 0.0f)
			GUI.Label(highScoreRect,"Champion score: EMPTY",skin.GetStyle ("HighScores"));
		else
		{
			string highScoreStr = "Champion Score: " + string.Format("{0:0.0}",highScore);
			GUI.Label(highScoreRect,highScoreStr,skin.GetStyle ("HighScores"));
		}
		
		// highest level
		if (highLevel < 0.0f)
			GUI.Label(highLevelRect,"Highest Level: EMPTY",skin.GetStyle ("HighScores"));
		else
		{
			string highLevelStr = "Highest Level: " + string.Format("{0:0.0}",highLevel);
			GUI.Label(highLevelRect,highLevelStr,skin.GetStyle ("HighScores"));
		}
		
		if (GUI.Button (clearScoresButtonRect, "Clear High Scores//Levels"))
		{
			PlayerPrefs.DeleteKey ("High Score");
			PlayerPrefs.DeleteKey ("High Level");
		}
		if (GUI.Button (returnToMainButtonRect, "Return to Game Title"))
		{
			Application.LoadLevel("TitleScene");
		}
	}
}
