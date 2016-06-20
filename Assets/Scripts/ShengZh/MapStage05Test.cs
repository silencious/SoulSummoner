using UnityEngine;
using System.Collections;
using System.IO;

public class MapStage05Test : MonoBehaviour {
    // No Use Now!


    //Add to this to 8 test cubes
    //add 8 diagrams
    //must do generate map first and disable the generate map script
    private Texture2D tex;

    public GameObject diagram;
    public int index;


    private string path = "D:/UnityTest/SoulSummoner/Assets/Stages/Stage05/";
    private Color black = Color.black;
    private Color white = Color.white;
    private int sizeX = 500;
    private int sizeZ = 500;


    private int testSize = 90 / 2;

    private bool testing = false;
    private int testX;
    private float testY = 20.5f;
    private int testZ;
    private int startX;
    private int startZ;
    private int endX;
    private int endZ;


    void startTest()
    {
        testX = startX;
        testZ = startZ;
        testing = true;
        transform.position = new Vector3((float)(testX + 0.5), testY, (float)(testZ + 0.5));
        Debug.Log("Start Testing!");
    }

    void doTest()
    {
        testZ += 1;
        if (testZ >= endZ)
        {
            testZ = startZ;
            testX += 1;
            if (testX >= endX)
            {
                Debug.Log("Test Finish!");
                testing = false;
                transform.position = new Vector3(0, testY, 0);
            }
        }
        transform.position = new Vector3((float)(testX + 0.5), testY, (float)(testZ + 0.5));
    }

	// Use this for initialization
	void Start () {
        startX = (int)diagram.transform.position.x - testSize;
        startZ = (int)diagram.transform.position.z - testSize;
        endX = (int)diagram.transform.position.x + testSize;
        endZ = (int)diagram.transform.position.z + testSize;

        tex = new Texture2D(sizeX, sizeZ);
        byte[] content = File.ReadAllBytes(path + "Map05-0.png");
        tex.LoadImage(content);
	}

	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Save!");
            string p = path + "Map05-" + index.ToString() + ".png";
            File.WriteAllBytes(path, tex.EncodeToPNG());
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            startTest();
        }

        if (testing.Equals(true))
        {
            doTest();
            while (testing.Equals(true) && (testX < 0 || testX >= sizeX || testZ < 0 || testZ >= sizeZ)) {
                doTest();
            }
        } 
	}

    void checkObject(GameObject obj)
    {
        if (obj.name.StartsWith("Cube1"))
        {
            tex.SetPixel(testX, testZ, black);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        checkObject(other.gameObject);
    }
    void OnTriggerStay(Collider other)
    {
        checkObject(other.gameObject);
    }
    void OnTriggerExit(Collider other)
    {
    }
}
