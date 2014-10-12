using UnityEngine;
using System.Collections;

public class LaserSwitch : MonoBehaviour {
	public float laserOnTimeSec = 2.0f;
	public float laserOffTimeSec = 5.0f;
	public GameObject laser;
	public bool laserSound = true;
	
	private bool laserOn = false;
	private float lastTime;
	
	// audio clips
	public AudioClip[] audioClips;	
	
	// Use this for initialization
	void Start () {
		laserOn = false;
		lastTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		checkLaserTime ();
		
		if (laserOn) {
			laser.SetActive(true);
			if (laserSound)
				PlaySound (0);
		}
		else {
			laser.SetActive (false);
			if (laserSound)
				audio.Stop ();
		}
		
	}
	
	void checkLaserTime() {
		
		float currentTime = Time.time;
		
		//print ("in checkLaserTime: period = " + (currentTime - lastTime).ToString ());
		
		if ((laserOn) && ((currentTime - lastTime) > laserOnTimeSec))
		{
			//print ("laser off");
			laserOn = false;
			lastTime = currentTime;
		}
		if ((!laserOn) && ((currentTime - lastTime) > laserOffTimeSec))
		{
			//print ("laser on");
			laserOn = true;
			lastTime = currentTime;
		}	
	}

	void PlaySound(int clipIndex)
	{
		audio.clip = audioClips[clipIndex];
		audio.Play();
	}
	
}
