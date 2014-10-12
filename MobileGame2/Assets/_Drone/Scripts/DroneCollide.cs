using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class DroneCollide : MonoBehaviour {

	//public ParticleSystem explosion;
	public GameObject explosion;
	public float collisionBounce = 10f;
	public AudioClip boom;
		
	private GameObject expl1;
	
	void Startup()
	{
		explosion.gameObject.SetActive (false);
	}
		
	void OnCollisionEnter(Collision hit)
	{
		//print ("collided with something!");
		
		GameObject go = hit.gameObject;
		if ((go.tag == "Player") || (go.tag == "Weapon"))
		{
			
			// explode!
			expl1 = Instantiate (explosion,transform.position,Quaternion.identity) as GameObject;
			
			AudioSource.PlayClipAtPoint(boom,transform.position);
			Destroy (expl1,3f);    // need to destroy this one *before* destroying the gameObject!
			Destroy (gameObject);
		}
		else
		{
			// push away from this object
			Vector3 forceDir = transform.position - go.transform.position;
			rigidbody.AddForce (forceDir * collisionBounce);
		}
	}	
}
