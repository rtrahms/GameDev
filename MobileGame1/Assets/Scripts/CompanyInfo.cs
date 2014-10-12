using UnityEngine;
using System.Collections;

public class CompanyInfo : MonoBehaviour {

	public GUISkin skin;
	
	private Rect companyInfoRect1;
	private Rect companyInfoRect2;
	private Rect returnToMainButtonRect;
	
	// Use this for initialization
	void Start () {
	
		companyInfoRect1 = new Rect(Screen.width/4,10,200,50);
		companyInfoRect2 = new Rect(Screen.width/4,60,200,50);
		returnToMainButtonRect = new Rect(Screen.width/4, 120, 200,50);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
	
		GUI.skin = skin;
		
		GUI.Label (companyInfoRect1,"Hella Shaggy Software",skin.GetStyle("CompanyInfo"));
		GUI.Label (companyInfoRect2,"www.hellashaggy.com",skin.GetStyle("CompanyInfo"));
		
		if (GUI.Button (returnToMainButtonRect, "Return to Game Title"))
		{
			Application.LoadLevel("TitleScene");
		}
		
	}
	
}
