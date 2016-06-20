using UnityEngine;
using System.Collections;

public class PlayerTriggerMove : MonoBehaviour {

    public GameObject moveobj;
    public float x;
    public float y;
    public float z;
    public float time = 2;

    private Vector3 vel;
    private Vector3 dest;
    private bool moved;
    private bool moving;
	// Use this for initialization
	void Start () {
        if (moveobj == null) moveobj = this.gameObject;
        dest = new Vector3(moveobj.transform.position.x + x, moveobj.transform.position.y + y, moveobj.transform.position.z + z);
        vel = new Vector3(x / time, y / time, z / time);
        moved = false;
        moving = false;
    }

    bool arrive() {
        double threshold = 0.1;
        //Debug.Log(Mathf.Abs(moveobj.transform.position.y - dest.y));
        if (Mathf.Abs(moveobj.transform.position.x - dest.x) > threshold) return false;
        if (Mathf.Abs(moveobj.transform.position.y - dest.y) > threshold) return false;
        if (Mathf.Abs(moveobj.transform.position.z - dest.z) > threshold) return false;
        return true;
    }
	
	// Update is called once per frame
	void Update () {
        if (moving.Equals(true)) {
            //moveobj.transform.Translate(vel * Time.deltaTime);
            Vector3 post = moveobj.transform.position;
            post += (vel * Time.deltaTime);
            moveobj.transform.position = post;
            if (arrive().Equals(true)) {
                moving = false;
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;
        if (obj.tag.CompareTo("Player") == 0)
        {
            if (moved.Equals(false)) {
                moved = true;
                moving = true;
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
    }
    void OnTriggerExit(Collider other)
    {
    }
    void OnCollisionEnter(Collision collisionInfo)
    {
    }
    void OnCollisionExit(Collision collisionInfo)
    {
    }
    void OnCollisionStay(Collision collisionInfo)
    {
    }
}
