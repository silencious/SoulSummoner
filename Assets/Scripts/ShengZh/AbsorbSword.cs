using UnityEngine;
using System.Collections;

public class AbsorbSword : MonoBehaviour {

    public GameObject accordFire;
    public GameObject stagecontrol;

    private Stage2Control sc;

	// Use this for initialization
	void Start () {
        if (accordFire != null) {
            accordFire.SetActive(false);
        }

        if (stagecontrol != null) {
            sc = stagecontrol.GetComponent<Stage2Control>();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDestroy() {
        if (accordFire != null) {
            accordFire.SetActive(true);
        }
        if (sc != null) {
            sc.add();
        }
    }
}
