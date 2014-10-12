using UnityEngine;
using System.Collections;

public class PlayerControl_ChangingGrav : MonoBehaviour {

	// main motion parameters for player
	public float gravity = 9.81f;
	public float jumpRange = 10f;
	public float jumpSpeed = 10f;
	public float turnSpeed = 90f;
	public float moveSpeed = 8f;
	public float deltaGround = 0.2f;
	public bool isGrounded = true;
	public BoxCollider collider;
	public float maxRotationForce = 100f;
	public float minYDeg = -80.0f;
	public float maxYDeg = 80.0f;
	
	// changing gravity parameters (normals)
	private Vector3 surfaceNormal;
	private Vector3 myNormal;     // character normal
	private float lerpSpeed = 10f;
	private float distGround;
	private float turnAngle = 0;
	private bool jumping = false;
	private float vertSpeed = 0;
	private Transform myTransform;
	private Quaternion lateralRotate;
	
	// camera motion parameters
	private Camera mainCam;
	private float xDeg, yDeg;
	private float oldX, oldY;
	private Quaternion fromRotation;
	private Quaternion toRotation;
	private Quaternion cameraTilt;
	public float maxFov = 90f;
	public float minFov = 10f;
	private float currentFov = 50f;
			
	// Use this for initialization
	void Start () {
			
		myNormal = transform.up;
		myTransform = transform;
		rigidbody.freezeRotation = true;
		distGround = collider.extents.y - collider.center.y;
		
		oldX = 0.0f;
		oldY = 0.0f;
		xDeg = 0.0f;
		yDeg = 0.0f;
		mainCam = GetComponentInChildren<Camera>();
		if (mainCam == null)
			print ("Could not find camera");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// apply gravity
		rigidbody.AddForce (-gravity * rigidbody.mass * myNormal);
	}
	
	void Update() {
		Ray ray;
		RaycastHit hit;

		
		xDeg += (Input.GetAxis ("Mouse X") * maxRotationForce);
		xDeg = DeadZone (xDeg, -2f, 2f);
		yDeg += -1.0f * Input.GetAxis ("Mouse Y") * maxRotationForce;
		yDeg = Limiter(yDeg, -1000, 1000);
		
		// abort while jumping to a wall
		if (jumping) 
			return;
			
		ray = new Ray(myTransform.position, myTransform.forward);

		if (Input.GetButtonDown ("Jump"))
		{			
			if (Physics.Raycast(ray, out hit, jumpRange))
			{
				//print ("hit wall in jump - rotating player");
				JumpToWall(hit.point,hit.normal);
			}
			else if (isGrounded)
			{
				//print ("hit wall, but we're grounded");
				rigidbody.velocity += jumpSpeed * myNormal;
			}
		}

		// set the rotation of the player
		//transform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0);
		//myTransform.Rotate(0, xDeg * Time.deltaTime, 0);
		lateralRotate = Quaternion.Euler (0, xDeg-oldX, 0);
		cameraTilt = Quaternion.Euler (yDeg-oldY, 0, 0);
		oldX = xDeg;
		oldY = yDeg;
		
		//myTransform.rotation = lateralRotate;
		//Debug.DrawRay(myTransform.position, myTransform.forward*10f, Color.red);
		
		// set the rotation of the camera
		//mainCam.transform.Rotate (yDeg * Time.deltaTime, 0, 0);
		//mainCam.transform.rotation = cameraTilt;
		//Debug.DrawRay(mainCam.transform.position, mainCam.transform.forward*10f, Color.blue);
		
		ray = new Ray(myTransform.position, -myNormal);
		Debug.DrawRay(myTransform.position, -myNormal*10f, Color.cyan);
		
		if (Physics.Raycast(ray, out hit))
		{
			isGrounded = (hit.distance <= distGround + deltaGround);
			surfaceNormal = hit.normal;
		}
		else
		{
			isGrounded = false;
			surfaceNormal = Vector3.up;
		}
		
		// rotate character to myNormal
		myNormal = Vector3.Lerp (myNormal,surfaceNormal, lerpSpeed*Time.deltaTime);
		Vector3 myForward = Vector3.Cross(myTransform.right, myNormal);
		
		Quaternion targetRot = Quaternion.LookRotation(myForward,myNormal) * lateralRotate;
		myTransform.rotation = Quaternion.Lerp (myTransform.rotation, targetRot, lerpSpeed*Time.deltaTime);
		Debug.DrawRay(myTransform.position, myTransform.forward*10f, Color.red);
		
		//Quaternion targetCamRot = Quaternion.LookRotation(mainCam.transform.forward,myNormal) * cameraTilt;
		//mainCam.transform.rotation = Quaternion.Lerp (mainCam.transform.rotation, targetCamRot, lerpSpeed*Time.deltaTime);
		Debug.DrawRay(mainCam.transform.position, mainCam.transform.forward*10f, Color.blue);
		
		// move forward
		transform.Translate(0, 0, Input.GetAxis ("Vertical") * moveSpeed * Time.deltaTime);		
			
	}
	
	float Limiter(float value, float minVal, float maxVal)
	{
		if (value < minVal)
			return minVal;
		if (value > maxVal)
			return maxVal;
		
		return value;
	}	
	
	float DeadZone(float value, float negThresh, float posThresh)
	{
		if (value < negThresh)
			return value;
		if (value > posThresh)
			return value;
		
		return 0f;
	}	
	
	void JumpToWall(Vector3 point, Vector3 normal) {
	
		jumping = true;		// signal it is jumping to the wall
		rigidbody.isKinematic = true;  // disable physics while jumping
		
		Vector3 orgPos = transform.position;
		Quaternion orgRot = transform.rotation;
		Vector3 dstPos = point + normal * (distGround + 0.5f);
		
		
		Vector3 myForward = Vector3.Cross(transform.right, normal);
		Quaternion dstRot = Quaternion.LookRotation(myForward,normal);

		Quaternion camRot = mainCam.transform.rotation;
		Quaternion dstCamRot = Quaternion.LookRotation(mainCam.transform.forward,normal);
		
		StartCoroutine(JumpTime (orgPos, orgRot, dstPos, dstRot, camRot, dstCamRot, normal));
		
	}
	
	private IEnumerator JumpTime(Vector3 orgPos, Quaternion orgRot, Vector3 dstPos, Quaternion dstRot, Quaternion camRot, Quaternion dstCamRot, Vector3 normal)
	{
		for (float t=0f; t < 1f; )
		{
			t += Time.deltaTime;
			
			// translate and rotate player
			transform.position = Vector3.Lerp (orgPos,dstPos, t);
			transform.rotation = Quaternion.Slerp(orgRot,dstRot,t);
			
			// rotate camera too
			mainCam.transform.rotation = Quaternion.Slerp(orgRot,dstRot,t);

			yield return null;   // return here next frame
		}
		myNormal = normal;				// update myNormal
		rigidbody.isKinematic = false;  // reenable physics
		jumping = false;
		
	}
}
