using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUDTargets : MonoBehaviour {

	public float scannerRange = 1000f;
	public float refreshRateSec = 5f;
	public GameObject GameManager;
	private GalaxyMap map;
	
	private float startTime;
	private GameObject[] currentDrones;
	private GameObject[] currentMotherships;
	private GameObject[] currentStations;
	//private List<GalaxyObject> galaxyObjs;
	
	private Texture2D redTexture, magentaTexture, greenTexture;
	
	// Use this for initialization
	void Start () {
		startTime = Time.time;
		//currentTargets = null;		
		//map = GameManager.GetComponent<GalaxyMap>();		
		//galaxyObjs = map.galaxyObjs;
		
		redTexture = new Texture2D(1, 1);
		redTexture.SetPixel(0,0,Color.red);
		redTexture.Apply();
		
		magentaTexture = new Texture2D(1, 1);
		magentaTexture.SetPixel(0,0,Color.magenta);
		magentaTexture.Apply();
		
		greenTexture = new Texture2D(1, 1);
		greenTexture.SetPixel(0,0,Color.green);
		greenTexture.Apply();
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		if ((Time.time - startTime) > refreshRateSec)
		{
			currentDrones = GameObject.FindGameObjectsWithTag("Drone");
			currentMotherships = GameObject.FindGameObjectsWithTag("Mothership");
			currentStations = GameObject.FindGameObjectsWithTag("Station");						
			startTime = Time.time;
		}

	}
	
	void OnGUI()
	{		
			//MarkTargets (currentTargets,true, Color.red);
			
			MarkTargets (currentDrones, false, Color.red);
			MarkTargets (currentMotherships, false, Color.magenta);
			MarkTargets (currentStations, false, Color.green);
			
			//MarkTargets (galaxyObjs);
	}
			
	void MarkTargets(GameObject[] list, bool needScannerRange, Color color)
	//void MarkTargets(List<GalaxyObject> list)
	{
		if (list != null)
		{
			int numTargets = list.Length;
			for (int i=0; i<numTargets; i++)
			{
				if (list[i] != null)
				{
					Vector3 targetVec = list[i].transform.position - transform.position;
					//Vector3 targetVec = list[i].location - transform.position;
					
					// if it is in range and it is in front of us, mark it
					// OR if it is a known target that doesn't need a scanner, mark it
					if ((!needScannerRange || (Vector3.Magnitude(targetVec) < scannerRange)) &&
					   (Vector3.Dot(targetVec,transform.forward) > 0))
					{
						//Vector2 viewPos = Camera.main.WorldToScreenPoint(list[i].location);
						Vector2 viewPos = Camera.main.WorldToScreenPoint(list[i].transform.position);
						Rect targetRect = new Rect(viewPos.x, Screen.height - viewPos.y, 4, 4);
						DrawQuad (targetRect,color);
					}
				}
			}
		} 
	}
	
	void DrawQuad(Rect position, Color color) {
		if (color == Color.red)
			GUI.skin.box.normal.background = redTexture;
		else if (color == Color.magenta)
			GUI.skin.box.normal.background = magentaTexture;
		else
			GUI.skin.box.normal.background = greenTexture;
			
		GUI.Box(position, GUIContent.none);
	}
	
}
