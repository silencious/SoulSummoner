using UnityEngine;
using System.Collections;

public class BGC4Stage4 : MonoBehaviour {
    //Buring Ground Control For stage 4

    public GameObject islands;
    public GameObject player;

    private float max_dist = 100;
    private ArrayList list;
    // Use this for initialization
	void Start () {
        list = new ArrayList();
        foreach (Transform ts in islands.transform) {
            foreach (Transform ts2 in ts)
            {
                if (ts2.gameObject.name.StartsWith("BG"))
                {
                    list.Add(ts2.gameObject);
                    Debug.Log("Ok!");
                }
            }
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
	}

    float dist(Vector3 a, Vector3 b) {
        float d = (a.x - b.x) * (a.x - b.x);
        d += ((a.y - b.y) * (a.y - b.y));
        d += ((a.z - b.z) * (a.z - b.z));
        d = Mathf.Sqrt(d);
        return d;
    }
    bool judge(GameObject obj) {
        if (obj.transform.position.y > player.transform.position.y) {
            return false;
        }
        if (dist(obj.transform.position, player.transform.position) > max_dist) {
            return false;
        }
        return true;
    }
	// Update is called once per frame
	void Update () {
        foreach (GameObject obj in list) {
            bool j = judge(obj);
            if (!obj.activeSelf.Equals(j)) {
                obj.SetActive(j);
            }
        }
	}
}
