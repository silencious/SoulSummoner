using UnityEngine;
using System.Collections;
using System.IO;

public class MapStage02Rotate : MonoBehaviour {
    //Use After complete one part
    private Texture2D tex;
    private string path = "D:/UnityTest/SoulSummoner/Assets/Stages/Stage02/Map02.png";
    private Color black = Color.black;
    private Color white = Color.white;
    private int sizeX = 500;
    private int sizeZ = 500;
    private int startX = 180;
    private int startZ = 329;
    private int endX = 321;
    private int endZ = 477;
    private Vector2 mid4 = new Vector2(150, 58);
    private Vector2 mid5 = new Vector2(350, 58);
    private Vector2 rmp4;
    private Vector2 rmp5;
    private Vector2 mid;

    Texture2D newTex2D()
    {
        Texture2D ret = new Texture2D(sizeX,sizeZ);
        // new tex2 , set false
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeZ; j++)
            {
                ret.SetPixel(i, j, black);
            }
        }
        return ret;
    }

    Vector2 Rotate(Vector2 a, Vector2 o,float degree) {
        float x = a.x;
        float y = a.y;
        float rx0 = o.x;
        float ry0 = o.y;

        float x0= (x - rx0)*Mathf.Cos(degree*Mathf.Deg2Rad) - (y - ry0)*Mathf.Sin(degree*Mathf.Deg2Rad) + rx0 ;
        float y0 = (x - rx0) * Mathf.Sin(degree*Mathf.Deg2Rad) + (y - ry0) * Mathf.Cos(degree*Mathf.Deg2Rad) + ry0;
        return (new Vector2(x0, y0));
    }
	// Use this for initialization
	void Start () {
        mid = new Vector2(sizeX / 2, sizeZ / 2);
        rmp4 = Rotate(new Vector2(250, 450), mid, 135);
        rmp5 = Rotate(new Vector2(250, 450), mid, 225);

	    tex = new Texture2D(sizeX, sizeZ);
        byte[] content = File.ReadAllBytes(path);
        tex.LoadImage(content);

        Texture2D tex2 = newTex2D();
        Texture2D tex3 = newTex2D();
        Texture2D tex4 = newTex2D();
        Texture2D tex5 = newTex2D();
        
        for (int i = startX; i <= endX; i++) {
            for (int j = startZ; j <= endZ; j++) {
                if (tex.GetPixel(i, j).Equals(black)) continue;
                Vector2 stPoint = new Vector2(i,j);
                Vector2 toPoint;

                toPoint = Rotate(stPoint, mid, 90);
                tex2.SetPixel((int)toPoint.x, (int)toPoint.y, white);

                toPoint = Rotate(stPoint, mid, 270);
                tex3.SetPixel((int)toPoint.x, (int)toPoint.y, white);

                
                toPoint = Rotate(stPoint, mid, 135);
                toPoint.x += (mid4.x - rmp4.x);
                toPoint.y += (mid4.y - rmp4.y);
                tex4.SetPixel((int)toPoint.x, (int)toPoint.y, white);

                toPoint = Rotate(stPoint, mid, 225);
                toPoint.x += (mid5.x - rmp5.x);
                toPoint.y += (mid4.y - rmp5.y);
                tex5.SetPixel((int)toPoint.x, (int)toPoint.y, white);
                    
            }
        }

        for (int i = 0; i < sizeX; i++) {
            for (int j = 0; j < sizeZ; j++) {
                if (tex.GetPixel(i, j).Equals(white)) continue;
                if (tex2.GetPixel(i, j).Equals(white)) { tex.SetPixel(i, j, white); continue; }
                if (tex3.GetPixel(i, j).Equals(white)) { tex.SetPixel(i, j, white); continue; }
                if (tex4.GetPixel(i, j).Equals(white)) { tex.SetPixel(i, j, white); continue; }
                if (tex5.GetPixel(i, j).Equals(white)) { tex.SetPixel(i, j, white); continue; }
            }
        }

        for (int i = 0; i < sizeX; i++)
        {
            if (i.Equals(0) || i.Equals(sizeX-1)) continue;
            for (int j = 0; j < sizeZ; j++)
            {
                if (j.Equals(0) || j.Equals(sizeZ-1)) continue;
                if (tex.GetPixel(i, j).Equals(white)) continue;

                if (!tex.GetPixel(i - 1, j).Equals(white)) continue;
                if (!tex.GetPixel(i + 1, j).Equals(white)) continue;
                if (!tex.GetPixel(i, j - 1).Equals(white)) continue;
                if (!tex.GetPixel(i, j + 1).Equals(white)) continue;
                tex.SetPixel(i, j, white);
            }
        }
        File.WriteAllBytes(path, tex.EncodeToPNG());
	}

    // Update is called once per frame
    void Update()
    {
	
	}
}
