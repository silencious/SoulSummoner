using UnityEngine;

using System.Collections;

public class PCBehaviour : SoulBehaviour
{
	// control relative
	public Animator anim;
	public Rigidbody rb;
	public Camera cam;
	public float moveSpeed;
	public float jumpSpeed;
	public bool onGround;

	// gameplay relative
	Elements container;		// elements the PC has collected

	protected virtual void Start ()
	{
		// init gameplay relative parameters
		base.Start ();
		container = Elements.zero;	// empty initial element container

		// init moving parameters
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
		moveSpeed = 15.0f;
		jumpSpeed = 10.0f;
		onGround = true;
	}

	protected virtual void Update ()
	{
		if(hp<=0){
			HandleDeath ();
		}
		HandleMove ();
		HandleAction ();
	}

	protected virtual void OnCollisionEnter(Collision other){
		//Debug.Log ("collision enter");
		if (other.gameObject.CompareTag ("Terrain")) {
			onGround = true;
			//anim.SetBool ("jumping", false);
		}
	}

	void OnCollisionStay(Collision other){
		
	}

	void HandleMove(){
		float vertical = Input.GetAxis ("Vertical");
		float horizontal = Input.GetAxis ("Horizontal");

		Vector3 forward = cam.transform.forward;
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
		if(Input.GetMouseButtonDown(0)){
			// left, summon/cast
			//anim.Play ("Reach");
			Cast ();
		}
		if(Input.GetMouseButtonDown(1)){
			// right, absorb
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
		
	}
}
