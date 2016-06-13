using UnityEngine;

using System.Collections;

public class PCBehaviour : SoulBehaviour
{
	public Animator anim;
	public Rigidbody rb;

	// camera relative
	public Camera firstPersonCamera;
	public Camera thirdPersonCamera;
	private Camera currentCamera;

	// physics relative
	public float moveSpeed;
	public float jumpSpeed;
	public bool onGround;

	// gameplay relative
	Elements reserve;		// elements the PC has collected
	float maxAbsorbDistance = 10.0f;

	protected override void Start ()
	{
		// init gameplay relative parameters
		base.Start ();
		reserve = Elements.zero;	// empty initial element reserve

		// init moving parameters
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
		moveSpeed = 15.0f;
		jumpSpeed = 10.0f;
		onGround = true;
	}

	void FixedUpdate(){
		HandleMove ();
	}

	protected override void OnCollisionEnter(Collision other){
		//Debug.Log ("collision enter");
		if (other.gameObject.CompareTag ("Terrain")) {
			onGround = true;
			//anim.SetBool ("jumping", false);
		}
	}

	void OnCollisionStay(Collision other){
		
	}

	protected override void Update ()
	{
		if(hp<=0){
			HandleDeath ();
		}
		HandleAction ();
	}

	void HandleMove(){
		float vertical = Input.GetAxis ("Vertical");
		float horizontal = Input.GetAxis ("Horizontal");

		Vector3 forward = thirdPersonCamera.transform.forward;
		forward.y = 0;

		forward.Normalize ();
		Vector3 rightward = Vector3.Cross (Vector3.up, forward);

		Vector3 velocity = moveSpeed * (forward * vertical+rightward*horizontal);

		if (Mathf.Abs(vertical)+Mathf.Abs(horizontal) > 0.1f) {
			// set direction of character
			transform.forward = velocity;
		}


		//anim.SetFloat ("velocity", velocity.magnitude);
		if (Input.GetKey (KeyCode.Space) && onGround) {
			velocity.y = jumpSpeed;
			onGround = false;
			//anim.Play("Jump");
			//anim.SetBool("jumping", true);
		} else {
			velocity.y = rb.velocity.y;
		}

		// set velocity of rigidbody
		rb.velocity =velocity;
	}

	void HandleAction(){
		if(Input.GetKeyDown(KeyCode.Tab)){
			// switch camera
			if(currentCamera==firstPersonCamera){
				firstPersonCamera.enabled = false;
				thirdPersonCamera.enabled = true;
				currentCamera = thirdPersonCamera;
			}else{
				thirdPersonCamera.enabled = false;
				firstPersonCamera.enabled = true;
				currentCamera = firstPersonCamera;
			}
		}
		if(Input.GetMouseButtonDown(0)){
			// left mouse button, summon/cast
			//anim.Play ("Reach");
			Cast ();
		}
		if(Input.GetMouseButtonDown(1)){
			// right mouse button, absorb
			Absorb ();
		}
		// switch spell as key pressed
		if(Input.GetKeyDown(KeyCode.Alpha0)){

		}
	}

	void HandleDeath(){
		
	}

	void Cast(){
		
	}

	void Absorb(){
		Ray ray = currentCamera.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			if (hit.distance > maxAbsorbDistance)
				return;
			var obj = hit.collider.gameObject;
			// if the object can be absorbed
			var soul = obj.GetComponent<SoulBehaviour> ();
			if(soul!=null){
				// absorb the soul: add its elements to pc's reserve, destroy soul obj
				reserve = Elements.max (reserve + soul.elements, elements);
				Destroy (soul);
			}
		}
	}
}
