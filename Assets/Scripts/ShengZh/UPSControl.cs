using UnityEngine;
using System.Collections;

public class UPSControl : MonoBehaviour {
    //Universal Particle System Control

    //for list of prefebs with particle system
    public GameObject pslist1;
    public float threshold1 = 0;
    public GameObject pslist2;
    public float threshold2 = 0;
    public GameObject pslist3;
    public float threshold3 = 0;
    public GameObject pslist4;
    public float threshold4 = 0;
    public GameObject pslist5 ;
    public float threshold5 = 0;
    public GameObject pslist6;
    public float threshold6 = 0;

    //for single particle systems
    public GameObject obj1;
    public float threshold01 = 0;
    public GameObject obj2;
    public float threshold02 = 0;
    public GameObject obj3;
    public float threshold03 = 0;
    public GameObject obj4;
    public float threshold04 = 0;
    public GameObject obj5;
    public float threshold05 = 0;
    public GameObject obj6;
    public float threshold06 = 0;

    public GameObject player;

    private ArrayList list;
    private ArrayList thre;

	// Use this for initialization
	void Start () {
        list = new ArrayList();
        thre = new ArrayList();

        if (pslist1 != null) {
            foreach (Transform ts in pslist1.transform) {
                list.Add(ts.gameObject);
                thre.Add(threshold1);
            }
        }
        if (pslist2 != null) {
            foreach (Transform ts in pslist2.transform) {
                list.Add(ts.gameObject);
                thre.Add(threshold2);
            }
        }
        if (pslist3 != null) {
            foreach (Transform ts in pslist3.transform) {
                list.Add(ts.gameObject);
                thre.Add(threshold3);
            }
        }
        if (pslist4 != null) {
            foreach (Transform ts in pslist4.transform) {
                list.Add(ts.gameObject);
                thre.Add(threshold4);
            }
        }
        if (pslist5 != null) {
            foreach (Transform ts in pslist5.transform) {
                list.Add(ts.gameObject);
                thre.Add(threshold5);
            }
        }
        if (pslist6 != null)
        {
            foreach (Transform ts in pslist6.transform)
            {
                list.Add(ts.gameObject);
                thre.Add(threshold6);
            }
        }

        if (obj1 != null)
        {
            list.Add(obj1);
            thre.Add(threshold01);
        }
        if (obj2 != null)
        {
            list.Add(obj2);
            thre.Add(threshold02);
        }
        if (obj3 != null)
        {
            list.Add(obj3);
            thre.Add(threshold03);
        }
        if (obj4 != null)
        {
            list.Add(obj4);
            thre.Add(threshold04);
        }
        if (obj5 != null)
        {
            list.Add(obj5);
            thre.Add(threshold05);
        }
        if (obj6 != null)
        {
            list.Add(obj6);
            thre.Add(threshold06);
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
	}

    float dist(Vector3 a, Vector3 b)
    {
        float d = (a.x - b.x) * (a.x - b.x);
        d += ((a.y - b.y) * (a.y - b.y));
        d += ((a.z - b.z) * (a.z - b.z));
        d = Mathf.Sqrt(d);
        return d;
    }

    bool judge(GameObject obj,float max_dist)
    {
        if (dist(obj.transform.position, player.transform.position) > max_dist)
        {
            return false;
        }
        return true;
    }
	
	// Update is called once per frame
	void Update () {
        foreach (GameObject obj in list)
        {
            bool j = judge(obj,(float)(thre[list.IndexOf(obj)]));
            if (!obj.activeSelf.Equals(j))
            {
                obj.SetActive(j);
            }
        }
	}
}
