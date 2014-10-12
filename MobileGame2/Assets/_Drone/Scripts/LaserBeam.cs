using UnityEngine;
using System.Collections;

[RequireComponent (typeof(LineRenderer))]
[RequireComponent (typeof(AudioSource))]
public class LaserBeam : MonoBehaviour {
	
	public float laserWidth = 0.07f;
	public float noise = 0.01f;
	public float maxLength = 75.0f;
	public float laserPower = 0.001f;
	public Color color = Color.red;
	public AudioClip laserSoundClip;
	
	LineRenderer lineRenderer;
	int length;
	Vector3[] position;
	
	//Cache any transforms here
	Transform myTransform;
	Transform endEffectTransform;
	
	//The particle system, in this case sparks which will be created by the Laser
	public ParticleSystem endEffect;
	Vector3 offset;
	
	// Use this for initialization
	
	void Start () {
		
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.SetWidth(laserWidth, laserWidth);
		lineRenderer.useWorldSpace = true;
		
		myTransform = transform;
		offset = new Vector3(0,0,0);
		endEffect = GetComponentInChildren<ParticleSystem>();
		
		if(endEffect)	
		{
			endEffectTransform = endEffect.transform;
		}
		else
		{
			Debug.Log ("laserbeam.start: warning, no endEffect found");
		}
	}
	
	// Update is called once per frame
	
	void Update () {
		RenderLaser();
		AudioSource.PlayClipAtPoint(laserSoundClip,transform.position);
	}
	
	void RenderLaser(){
		
		//Shoot our laserbeam forwards!	
		UpdateLength(); 
		
		//
		Debug.DrawRay (myTransform.position, myTransform.forward * length,Color.blue);
		
		lineRenderer.SetColors(color,color);
		
		// temp:  laser with two position points - no noise
		/*
		position = new Vector3[2];
		lineRenderer.SetVertexCount(2);
		position[0] = myTransform.position;
		lineRenderer.SetPosition(0,position[0]);
		position[1] = myTransform.position + length*myTransform.forward;
		lineRenderer.SetPosition(1,position[1]);
		*/
		
		//Move through the Array - laser with noise
		position = new Vector3[length];
		position[0] = myTransform.position;
		lineRenderer.SetPosition(0, position[0]);
		for(int i = 1; i<length; i++){		
			
			// works in the global z direction only!! 
			// If I rotate the parent toward 90 deg from Z, this moves down to a dot at the local origin
			
			offset.x = myTransform.position.x + 1.2f*i*myTransform.forward.x + Random.Range(-noise,noise);	
			offset.y = myTransform.position.y + 1.2f*i*myTransform.forward.y + Random.Range(-noise,noise);	
			offset.z = myTransform.position.z + 1.2f*i*myTransform.forward.z + Random.Range(-noise,noise);	
			
			
			position[i] = offset;	
			
			lineRenderer.SetPosition(i, position[i]);
		}
		
	}
	
	
	
	void UpdateLength(){
		
		//Raycast from the location of the cube forwards	
		RaycastHit[] hits;
		
		//hits = Physics.RaycastAll(myTransform.position, myTransform.forward, maxLength);	
		
		Vector3 fwd = transform.TransformDirection (Vector3.forward);
		hits = Physics.RaycastAll(myTransform.position, fwd, maxLength);	
		
		Debug.DrawRay (myTransform.position,myTransform.forward * maxLength, Color.yellow);
		
		int i = 0;	
		while(i < hits.Length){		
			//Check to make sure we aren't hitting triggers but colliders		
			if(!hits[i].collider.isTrigger)		
			{		
				// if hit a player, adjust health downward
				if (hits[i].collider.gameObject.tag == "Player")
				{
					HullStatus hs = hits[i].collider.gameObject.GetComponent<HullStatus>();
					hs.adjustHealth (-laserPower);
				}
				
				// adjust length
				length = (int)Mathf.Round(hits[i].distance);		
				
				//Debug.DrawLine (myTransform.position,hits[i].point,Color.red);
				
				position = new Vector3[length];
				
				//Move our End Effect particle system to the hit point and start playing it			
				if(endEffect){			
					endEffectTransform.position = hits[i].point;			
					if(!endEffect.isPlaying)	
					{
						endEffect.Play();	
					}
				}
				
				lineRenderer.SetVertexCount(length);
				
				return;	
			}	
			i++;	
		}
		
		//If we're not hitting anything, don't play the particle effects
		if(endEffect){	
			if(endEffect.isPlaying)		
				endEffect.Stop();	
		}
		
		length = (int)maxLength;
		position = new Vector3[length];
		lineRenderer.SetVertexCount(length);
	}
	
}