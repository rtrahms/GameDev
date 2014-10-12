using UnityEngine;
using System.Collections;

public class GalaxyObject  {

	public GameObject go;
	public Vector3 location;
	public float scale;
	public int hazardIndex;
	
	public GalaxyObject() {
		location = Vector3.zero;
		hazardIndex = 0;
		scale = 1f;
	}
}
