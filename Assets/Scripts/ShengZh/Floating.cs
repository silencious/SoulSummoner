using UnityEngine;
using System.Collections;

public class Floating : MonoBehaviour {
    public float period;
    public float amplitude;

    private float time = 0;
    private float omega;
    private Vector3 orgPos;

	// Use this for initialization
	void Start () {
        orgPos = transform.position;
        omega = 2 * Mathf.PI / period;
	}
	
	// Update is called once per frame
	void Update () {
        time = time + Time.deltaTime;
        if (time > period) time -= period;
        float a = amplitude * Mathf.Cos(omega * time);
        transform.position = new Vector3(orgPos.x, orgPos.y + a, orgPos.z);
	}


}
