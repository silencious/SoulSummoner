using UnityEngine;
using System.Collections;
using System.IO;

public class MapStage03 : MonoBehaviour {
    private Texture2D tex;

    public Terrain terrian;
    public GameObject outline; //The Scale must be (1,1,1)
    public GameObject player;

    private ArrayList SC;
    //These SCs must be Cylinder which scaleX = scaleZ and their parent's Scale must be (1,1,1)

    private string path = "D:/UnityTest/SoulSummoner/Assets/Stages/Stage03/Map03.png";
    private string colliderPrefix = "SC";

    private bool nowset = true;
    private Color black = Color.black;
    private Color white = Color.white;
    private int sizeX = 500;
    private int sizeZ = 500;

    private float minHeight = 119.8f;
    private float maxHeight = 130f;

    private float[] heights = {50f,60f,65f,70f};
    private float diff = 0.01f;

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


        //check terrian , set true
        if (terrian != null)
        {
            TerrainData td = terrian.terrainData;
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeZ; j++)
                {
                    float h = td.GetHeight(i, j);
                    if (h > minHeight && h < maxHeight) {
                        tex.SetPixel(i, j, white);
                        continue;
                    }

                    foreach (float tarh in heights) {
                        if (Mathf.Abs(h - tarh) < diff) {
                            tex.SetPixel(i, j, white);
                            break;
                        }
                    } 
                }
            }
        }

        //check seven stars ,set true
        for (int i = 0; i < sizeX; i++) {
            for (int j = 0; j < sizeZ; j++) {
                foreach (GameObject obj in SC) {
                    float dist = ((float)i+0.5f - obj.transform.position.x) * ((float)i+0.5f - obj.transform.position.x);
                    dist = dist + ((float)j+0.5f - obj.transform.position.z) * ((float)j+0.5f - obj.transform.position.z);
                    dist = Mathf.Sqrt(dist);

                    if (dist < obj.transform.localScale.x/2) {
                        tex.SetPixel(i, j, white);
                        break;
                    }
                } 
            }
        }

        //check outline ,set false
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
	void Start () {
        tex = new Texture2D(sizeX, sizeZ);
        if (!File.Exists(path))
        {
            SC = new ArrayList();
            for (int i = 1; i <= 7; i++)
            {
                string objname = colliderPrefix + i;
                GameObject obj = GameObject.Find(objname);
                if (obj != null)
                {
                    SC.Add(obj);
                }
            }
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
        if (player != null) {
            int px = (int)player.transform.position.x;
            int pz = (int)player.transform.position.z;
            if (nowset.Equals(true)) tex.SetPixel(px, pz, white);
            else tex.SetPixel(px,pz,black);

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
