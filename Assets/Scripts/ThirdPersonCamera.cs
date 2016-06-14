using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour {

	public Transform lookAtObj;

	private float maxDis = 50.0f;
	private float minDis = 3.0f;
	private float distance=10.0f;

	private float maxTheta = 179.0f;
	private float minTheta = 1.0f;
	private float phi=0.0f;
	private float theta=30.0f;

	void Start(){
		if(lookAtObj==null){
			lookAtObj = transform.parent;
		}
		Vector3 vec = transform.position - lookAtObj.transform.position;
		float factor = 180.0f / Mathf.PI;
		theta = Mathf.Atan2(vec.x,vec.y)*factor;
		phi = Mathf.Atan2 (vec.z, vec.x)*factor;
		distance = vec.magnitude;
	}

	void Update(){
		// rotate camera
		if(Input.GetMouseButton(1)){
			float rotateX = Input.GetAxis ("Mouse X");
			float rotateY = Input.GetAxis ("Mouse Y");
			phi += rotateX*2.0f;
			theta += rotateY*2.0f;

			theta = Mathf.Min (theta, maxTheta);
			theta = Mathf.Max (theta, minTheta);
			//Debug.Log ("phi=" + phi + " theta=" + theta);
		}
		Vector3 vec = Quaternion.Euler (0,phi,theta)*Vector3.up;
		
		// update distance
		float scroll = Input.GetAxis ("Mouse ScrollWheel");
		distance *= (1 - 0.6f*scroll);
		distance = Mathf.Min (distance, maxDis);
		distance = Mathf.Max (distance, minDis);
		transform.position = lookAtObj.position + vec * distance;

		// keep direction
		transform.LookAt (lookAtObj);
	}
}
