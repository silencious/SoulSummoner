using UnityEngine;
using System.Collections;
using System.IO;

public class MapStage01 : MonoBehaviour {
    private Texture2D tex;
    
    public Terrain terrian;
    public GameObject outline; //The Scale must be (1,1,1)

    private string path = "D:/UnityTest/SoulSummoner/Assets/Stages/Stage01/Map01.png";

    private bool nowset = true;
    private Color black = Color.black;
    private Color white = Color.white;
    private int sizeX = 500;
    private int sizeZ = 500;

    private float minHeight = 19.9f;
    private float maxHeight = 85f;

    void newTex2D() {
        for (int i = 0; i < sizeX; i++) {
            for (int j = 0; j < sizeZ; j++) {
                tex.SetPixel(i, j, white);
            }
        }
        if (terrian != null) {
            TerrainData td = terrian.terrainData;
            for (int i = 0; i < sizeX; i++) {
                for (int j = 0; j < sizeZ; j++) {
                    float curHeight = td.GetHeight(i, j);
                    if (curHeight < minHeight || curHeight > maxHeight) {
                        tex.SetPixel(i, j, black);
                    }
                }
            }
        }

        if (outline != null)
        {
            foreach (Transform ts in outline.transform)
            {
                GameObject obj = ts.gameObject;
                for (int i = (int)(obj.transform.position.x - obj.transform.localScale.x / 2);
                    i < (int)(obj.transform.position.x + obj.transform.localScale.x / 2);
                    i++)
                {
                    for (int j = (int)(obj.transform.position.z - obj.transform.localScale.z / 2);
                        j < (int)(obj.transform.position.z + obj.transform.localScale.z / 2);
                        j++)
                    {
                        tex.SetPixel(i, j, black);
                    }
                }
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        tex = new Texture2D(sizeX, sizeZ);
        if (!File.Exists(path))
        {
            newTex2D();
        }
        else {
            byte[] content = File.ReadAllBytes(path);
            tex.LoadImage(content);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Save!");
            File.WriteAllBytes(path, tex.EncodeToPNG());
        }
	}

    void onDestroy() {
    }
}
