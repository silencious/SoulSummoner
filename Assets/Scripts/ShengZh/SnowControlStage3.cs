using UnityEngine;
using System.Collections;

public class SnowControlStage3 : MonoBehaviour {
    public GameObject snow;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;
        if (obj.tag.CompareTo("Player") == 0)
        {
            if (snow != null) {
                snow.SetActive(false);
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
    }
    void OnTriggerExit(Collider other)
    {
        GameObject obj = other.gameObject;
        if (obj.tag.CompareTo("Player") == 0)
        {
            if (snow != null)
            {
                snow.SetActive(true);
            }
        }
    }
}
