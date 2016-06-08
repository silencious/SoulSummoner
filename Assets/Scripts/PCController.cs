using UnityEngine;

using System.Collections;

public class PCController : MonoBehaviour
{
	public Animator anim;
	public Rigidbody rb;
	public Camera cam;
	public float moveSpeed;
	public float jumpSpeed;
	public bool onGround;

	void Start ()
	{
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
		moveSpeed = 15.0f;
		jumpSpeed = 10.0f;
		onGround = true;
	}

	void Update ()
	{
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

		if (Input.GetMouseButton (0)) {
			//anim.Play ("Reach");
		}
	}

	void OnCollisionEnter(Collision other){
		//Debug.Log ("collision enter");
		if (other.gameObject.CompareTag ("Terrain")) {
			onGround = true;
			//anim.SetBool ("jumping", false);
		}
	}

	void OnCollisionStay(Collision other){
		if (other.gameObject.CompareTag ("Cube") && Input.GetMouseButton(0)) {
			other.gameObject.SendMessage ("Explode");
		}
	}
}
