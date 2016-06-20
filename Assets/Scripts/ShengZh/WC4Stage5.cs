using UnityEngine;
using System.Collections;

public class WC4Stage5 : MonoBehaviour {
    //Weather Control For Stage 5

    public GameObject rain;
    public GameObject snow;
    public GameObject fire;

    public GameObject L;
    public GameObject SnowS;
    public GameObject FireS;
    public GameObject SnowM;
    public GameObject FireM;

    private int now;
    private double outside = 20.79;
    private double insideL = 20.75;
    private double insideM = 20.9;
    private double insideS = 21;

	// Use this for initialization
	void Start () {
        now = 0;
        if (snow != null) snow.SetActive(false);
        if (fire != null) fire.SetActive(false);
	}

    double dist(Vector3 a, Vector3 b) {
        //double d = Mathf.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y) + (a.z - b.z) * (a.z - b.z));
        double d = Mathf.Sqrt((a.x - b.x) * (a.x - b.x) + (a.z - b.z) * (a.z - b.z));
        return d;
    }

    int judge() {
        //Debug.Log("Judge!");
        if ((Mathf.Abs(transform.position.y - (float)insideL) > 1e-2)
            && (Mathf.Abs(transform.position.y - (float)insideM) > 1e-2)
            && (Mathf.Abs(transform.position.y - (float)insideS) > 1e-2)) return 0;
        if (dist(transform.position, SnowS.transform.position) < SnowS.transform.localScale.x/2) return 1;//snow
        if (dist(transform.position, FireS.transform.position) < FireS.transform.localScale.x/2) return 2;//fire
        if (dist(transform.position, SnowM.transform.position) < SnowM.transform.localScale.x/2) return 1;//snow
        if (dist(transform.position, FireM.transform.position) < FireM.transform.localScale.x/2) return 2;//fire
        if (transform.position.x < L.transform.position.x) return 1;//snow
        return 2;//fire
    }

	// Update is called once per frame
	void Update () {
        int j = judge();
        if (!now.Equals(j)) {
            //Debug.Log("New Weather " + j); 
            if (snow != null) snow.SetActive(false);
            if (fire != null) fire.SetActive(false);
            if (rain != null) rain.SetActive(false);

            now = j;
            switch (now) {
                case 0: if (rain != null) rain.SetActive(true);
                    break;
                case 1: if (snow != null) snow.SetActive(true);
                    break;
                case 2: if (fire != null) fire.SetActive(true);
                    break;
            }
        }
	}
}
