using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AutoMegaFiller : MonoBehaviour {
    MegaScatterObject MsObj;
    
    //When we will have 1000 objects we will increase the following number from 3 to 1001
    int MaxAvailableObjs = 3;
    string RadiusFilepath = "Assets/Resources/RadiusFile/RadiusInfos.txt";
    
    // Use this for initialization
    void Start () {
        MsObj = gameObject.GetComponent<MegaScatterObject>();
        AddMesh(Random.Range(0,4));
        StartCoroutine(KeepRefreshing());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void AddMesh(int seed)
    {
        int MaxObjectTypesPerBin = Random.Range(2, 3);
        for (int i = 0; i < MaxObjectTypesPerBin; i++)
        {
            MegaScatterLayer scatterLayer = new MegaScatterLayer();
            //Choose a random object
            int randomobjindex = Random.Range(1, MaxAvailableObjs);
            //the prefabs path given below is with respect to the Resources foler not assets folder
            string path = "Prefabs/" + randomobjindex.ToString();
            scatterLayer.obj = Resources.Load(path, typeof(GameObject)) as GameObject;
            float radius = GetRadius(randomobjindex.ToString());
            scatterLayer.radius = radius;
            scatterLayer.noOverlap = true;
            scatterLayer.LayerName = randomobjindex.ToString();
            scatterLayer.snap = new Vector3(radius, 0, 0);
            if (radius <= 0.08f) {
                scatterLayer.rotHigh = new Vector3(0, 270, 0);
                scatterLayer.weight = 50;
            }
            MsObj.layers.Add(scatterLayer);
        }
        MsObj.seed = seed;
        MsObj.update = true;
    }

    float GetRadius(string prefabname)
    {
        StreamReader textreader = new StreamReader(RadiusFilepath);
        string line;
        do
        {
            line = textreader.ReadLine();
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
        while (true)
        {
            yield return new WaitForSeconds(2f);
            ClearAllObjects();
            AddMesh(Random.Range(0, 4));
        }
    }

    void ClearAllObjects()
    {
        MsObj.RemoveObjects();
    }
}
