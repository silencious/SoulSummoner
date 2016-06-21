using UnityEngine;
using System.Collections;

public class TransportZone : MonoBehaviour {

    public GameObject destination;
	public UIDisplay ui;
    //public GameObject forward;

    // Use this for initialization
    void Start()
	{
		ui = GameObject.Find ("UI") .GetComponent<UIDisplay>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;
        if (obj.tag.CompareTo("Player") == 0)
        {
			if(gameObject.name.Equals("TransportClearStage")){
				ui.ClearStage ();
			}else{
				obj.transform.position = destination.transform.position;
				//obj.transform.LookAt(forward.transform);
				obj.transform.rotation = destination.transform.rotation;
				//Debug.Log(obj.transform.rotation.eulerAngles);
				Rigidbody rb = obj.GetComponent<Rigidbody>();
				rb.velocity = new Vector3(0, 0, 0);				
			}
        }
    }
    void OnTriggerStay(Collider other)
    {
    }
    void OnTriggerExit(Collider other)
    {
    }

    //OnCollisionEnter方法必须是在两个碰撞物体都不勾选isTrigger的前提下才能进入
    //反之只要勾选一个isTrigger那么就能进入OnTriggerEnter方法。
    //一般实现碰撞用OnCollision，自己实现逻辑用OnTrigger
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
