using UnityEngine;
using System.Collections;

public class TouchButtonLogic : MonoBehaviour {

	bool buttonTouched = false;
	
	// Update is called once per frame
	void Update () {
		if (Input.touches.Length <= 0)
		{
			// if no touches
			buttonTouched = false;
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
						//Debug.Log("The touch has begun on " + this.name);
						buttonTouched = true;
					}
					else if (Input.GetTouch (i).phase == TouchPhase.Ended)
					{
						//Debug.Log ("The touch has ended on " + this.name);
						buttonTouched = false;
					}
					
				}
			}
		}
	}
	
	public bool isTouched()
	{
		return buttonTouched;
	}
}
