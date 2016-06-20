using UnityEngine;
using System.Collections;

public class Stage2Control : MonoBehaviour {
    private int cnt = 0;
    public GameObject terminal;

	// Use this for initialization
	void Start () {
        if (terminal != null) {
            terminal.SetActive(false);
        }
	}

    public void add() {
        cnt += 1;
        if (cnt.Equals(5) && terminal!=null) {
            terminal.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
