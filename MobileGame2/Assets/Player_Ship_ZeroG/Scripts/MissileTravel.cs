using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MissileTravel : MonoBehaviour {

	public float speed = 100f;
	public float explosive = 100f;
	public GameObject explosion;
	public float lifetimeSec = 10f;
	public GameObject targetGO;
	public float turnSpeedIncr = 2f;
	public AudioClip boom;
	
	private float turnSpeed = 1f;
	private float startTime;
	
	// Use this for initialization
	void Start () {
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		Vector3 travelVec;

		if (targetGO != null)
		{
			// target set - rotate toward that
			travelVec = (targetGO.transform.position - transform.position).normalized;
			Quaternion rot = Quaternion.LookRotation(travelVec);
			transform.rotation = Quaternion.Slerp (transform.rotation, rot, turnSpeed * Time.deltaTime);
			
			// slowly increase turning speed over guided missile's life
			turnSpeed = turnSpeed + (turnSpeedIncr * Time.deltaTime);
		}
		
		travelVec = transform.forward * speed * Time.deltaTime;
		transform.position = transform.position + travelVec;
		
		
		if ((Time.time - startTime) > lifetimeSec)
		{
			Explode ();
		}
		
	}
	
	void OnCollisionEnter(Collision hit)
	{
		GameObject go = hit.gameObject;
		Explode ();
	}
	
	void Explode()
	{
		
		Object expl1 = Instantiate (explosion,transform.position,Quaternion.identity);
		
		AudioSource.PlayClipAtPoint(boom,transform.position);
		Destroy (expl1,2f);      // need to destroy this one *before* destroying the gameObject!
		Destroy (gameObject);
	}
	
}
