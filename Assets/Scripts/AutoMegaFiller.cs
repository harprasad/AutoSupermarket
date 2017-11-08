using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AutoMegaFiller : MonoBehaviour {
    MegaScatterObject MsObj;
    
    //When we will have 1000 objects we will increase the following number from  to 1001
    int MaxAvailableObjs = 80;  //actual +1
    string RadiusFilepath = "Assets/Resources/RadiusFile/RadiusInfos.txt";
    List<int> ommiteednumbers = new List<int> { 36,37,38,39 };
    float[] PossibleRotations = { 0, 90, 180, 270 };
    // Use this for initialization
    void Start () {
        ColorCodes.FillColorCodes();
        MsObj = gameObject.GetComponent<MegaScatterObject>();
        AddMesh(Random.Range(0,4));
        StartCoroutine(KeepRefreshing());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void AddMesh(int seed)
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        int MaxObjectTypesPerBin = 4;
        for (int i = 0; i < MaxObjectTypesPerBin; i++)
        {
            MegaScatterLayer scatterLayer = new MegaScatterLayer();
            if(i  < 2) { scatterLayer.weight = 10; }
            //Choose a random object
            //the prefabs path given below is with respect to the Resources foler not assets folder
            int randomobjindex = Random.Range(1, MaxAvailableObjs);
            if (ommiteednumbers.Contains(randomobjindex)) {
                i--;
                continue;
            }
            string path = "Prefabs/" + randomobjindex.ToString();
            scatterLayer.obj = Resources.Load(path, typeof(GameObject)) as GameObject;
            float radius = GetRadius(randomobjindex.ToString());
            scatterLayer.radius = radius;
            scatterLayer.noOverlap = true;
            scatterLayer.LayerName = randomobjindex.ToString();
            scatterLayer.snap = new Vector3(radius*2, 0, 0);
            if (radius <= 0.08f) {
               // scatterLayer.rotHigh = new Vector3(0, 270, 0);
               // scatterLayer.weight = 10;
                scatterLayer.vertexnoise = false;
                scatterLayer.snap = new Vector3(radius * 2, 0, 0);
            }
            scatterLayer.seed = seed;
            float randomangle = PossibleRotations[Random.Range(0, PossibleRotations.Length)];
            scatterLayer.prerot = new Vector3(0, randomangle, 0);
            MsObj.layers.Add(scatterLayer);
        }
        MsObj.update = true;
    }

 
    float GetRadius(string prefabname)
    {
        
        StreamReader textreader = new StreamReader(RadiusFilepath);
        string line;
        do
        {
            line = textreader.ReadLine();
            if (line == null) {
                return 0.1f;
            }
            if (line.Equals(prefabname))
            {
                float radius = float.Parse(textreader.ReadLine());
                textreader.Close();
                return radius;
            }
        } while (line != null);
        textreader.Close();
        return 0.1f;//default value
    }

    IEnumerator KeepRefreshing()
    {
        int counter = 0;
        while (true)
        {
            yield return new WaitForSeconds(1.5f);
            ClearAllObjects();
            AddMesh(Random.Range(0, 10));
            yield return new WaitForSeconds(0.1f);
            GameObject[] Stuffs = GameObject.FindGameObjectsWithTag("Stuff");
            TakeScreenShots(counter, Stuffs, false,false);
            yield return new WaitForSeconds(0.1f);
            SetCullingMask(true);
            yield return new WaitForSeconds(0.1f);
            TakeScreenShots(counter, Stuffs, true,true);
            yield return new WaitForSeconds(0.1f);
            TakeScreenShots(counter, Stuffs, true,false);
            yield return new WaitForSeconds(0.1f);
            SetCullingMask(false);
            counter++;
        }
    }

    void SetCullingMask(bool allblack)
    {
        if (!allblack)
        {
            Camera.main.cullingMask = (1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Objects"));
        }
        else
        {
            Camera.main.cullingMask = (1 << LayerMask.NameToLayer("Objects"));
        }
    }
    void ClearAllObjects()
    {
        MsObj.RemoveObjects();
        MsObj.layers.RemoveRange(0, MsObj.layers.Count);
    }

    void TakeScreenShots(int counter,GameObject[] Stuffs,bool Labled,bool classSpecific)
    {
        if (!Labled)
        {
            ScreenCapture.CaptureScreenshot("ScreenShots/JPEGImages/" + "Screenshot" + counter.ToString() + ".png", 3);
        }
        else
        {
            byte colorindex = 10;
            foreach (GameObject stuff in Stuffs)
            {
                if (stuff.GetComponent<Renderer>() == null)
                {
                    //This is for the downloaded models which contains multiple meshes in children
                    Renderer[] renderers = stuff.GetComponentsInChildren<Renderer>();
                    foreach (Renderer rend in renderers)
                    {
                        rend.material.mainTexture = null;
                        if (classSpecific)
                        {
                            foreach (Material mat in rend.materials)
                            {
                                mat.shader = Shader.Find("Unlit/Color");
                                mat.color = ColorCodes.Colordictionary[stuff.name];
                            }
                        }
                        else
                        {
                            foreach (Material mat in rend.materials)
                            {
                                mat.shader = Shader.Find("Unlit/Color");
                                mat.color = new Color32(colorindex, colorindex, colorindex, 255);
                            }
                        }
                    }
                    colorindex++;
                    if (classSpecific)
                    {
                        ScreenCapture.CaptureScreenshot("ScreenShots/cls/" + "Screenshot" + counter.ToString() + ".png", 3);
                    }
                    else
                    {
                        ScreenCapture.CaptureScreenshot("ScreenShots/inst/" + "Screenshot" + counter.ToString() + ".png", 3);
                    }

                }

            }
        }
    }

}
