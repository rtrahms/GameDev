using UnityEngine;
using System.Collections;

public class MothershipHealth : MonoBehaviour {

	public float maxHealth = 100f;
	
	private float health;

	public float dischargeRate = 20f;
	public float rechargeRate = 1f;
	
	public GameObject explosion;
	public float collisionBounce = 10f;
	public AudioClip boom;
	
	private GameObject expl1;
	
	void Start()
	{
		health = maxHealth;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		if (health < maxHealth)
			health += rechargeRate * Time.deltaTime;
		if (health > maxHealth)
			health = maxHealth;
	}
	
	void OnCollisionEnter(Collision hit)
	{
		//print ("collided with something!");
		
		GameObject go = hit.gameObject;
		if ((go.tag == "Player") || (go.tag == "Weapon"))
		{
			health -= dischargeRate;
			
			if (health <= 0)
			{
				// explode!
				expl1 = Instantiate (explosion,transform.position,Quaternion.identity) as GameObject;
				
				AudioSource.PlayClipAtPoint(boom,transform.position);
				Destroy (expl1,9f);    // need to destroy this one *before* destroying the gameObject!
				Destroy (gameObject);
			}
		}
	}
		
}
