using UnityEngine;
using System.Collections;
using System.IO;

public class MapStage02 : MonoBehaviour {
    private Texture2D tex;
    public GameObject player;
    private string path = "D:/UnityTest/SoulSummoner/Assets/Stages/Stage02/Map02.png";
    private bool nowset = true;
    private Color black = Color.black;
    private Color white = Color.white;
    private int sizeX = 500;
    private int sizeZ = 500;

    void newTex2D()
    {
        // new tex2 , set false
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeZ; j++)
            {
                tex.SetPixel(i, j, black);
            }
        }
    }

	// Use this for initialization
	void Start () {
        tex = new Texture2D(sizeX, sizeZ);
        if (!File.Exists(path))
        {
           newTex2D();
        }
        else
        {
            byte[] content = File.ReadAllBytes(path);
            tex.LoadImage(content);
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        Debug.Log("Auto Set : " + nowset);
	}
	
	// Update is called once per frame
	void Update () {
        if (player != null)
        {
            int px = (int)player.transform.position.x;
            int pz = (int)player.transform.position.z;
            if (nowset.Equals(true)) tex.SetPixel(px, pz, white);
            else tex.SetPixel(px, pz, black);

            if (Input.GetKeyDown(KeyCode.C))
            {
                nowset = !nowset;
                Debug.Log("Auto Set : " + nowset);
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Save!");
            File.WriteAllBytes(path, tex.EncodeToPNG());
        }
	}
}
