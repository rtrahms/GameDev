using UnityEngine;
using System.Collections;

public class HelloShaggyLogic : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
	
#if UNITY_ANDROID
		if (Input.touches.Length <= 0)
		{
			// if no touches
		}
		else 
		{
			// check for ALL touches
			for (int i=0; i<Input.touchCount; i++)
			{
				// work on touch #i
				if (this.guiTexture.HitTest(Input.GetTouch(i).position))
				{
					// if current touch hits our GUItexture, run this code
					if (Input.GetTouch (i).phase == TouchPhase.Began)
					{
						Application.LoadLevel("CompanyInfo");
					}
				}
			}
		}
#else
		if (Input.GetMouseButtonDown(0))
		{
			if (this.guiTexture.HitTest(Input.mousePosition))
			{
				Application.LoadLevel("CompanyInfo");
			}
		}
#endif
					
	}
}
