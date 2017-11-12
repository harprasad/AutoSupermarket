using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFiller : MonoBehaviour {

    public Transform binStartingPoint;
    


    public float maxstuffineachBin;
    public float maxZ;
    public float maxX;
    List<int> ommiteednumbers = new List<int> { 36, 37, 38, 39 };

    int MaxAvailableObjs = 80;  //actual +1
    float xstep = 0;
    float zstep = 0;
    Vector3 nextpoint;
    public int NoOfColumns;
    public float ColumnSpacing;
    public float RowSpacing;
    // Use this for initialization
    void Start () {
        ColorCodes.FillColorCodes();
        nextpoint = binStartingPoint.transform.position;
        StartCoroutine(KeepRefresh());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void PlaceStuff()
    {
        for (int xcolumns = 0; xcolumns < 100; xcolumns++)
        {
            int objindex = SelectObjectIndex();
            for (int columns = 0; columns < NoOfColumns; columns++)
            {
                for (int rows = 0; rows < 3; rows++)
                {
                    SpawnSelectedObject(objindex);
                    nextpoint.z += zstep + ColumnSpacing;
                    if ((nextpoint.z + zstep) > maxZ)
                    {
                        break;
                    }
                }
                nextpoint.x += xstep + RowSpacing;
                nextpoint.z = binStartingPoint.position.z;
                if (nextpoint.x > maxX)
                {
                    break;
                }
            }
            if (nextpoint.x > maxX)
            {
                break;
            }
        }
    }

    int SelectObjectIndex()
    {
        int randomobjindex = 1;
        do
        {
            randomobjindex = Random.Range(1, MaxAvailableObjs);
        } while (ommiteednumbers.Contains(randomobjindex));
        return randomobjindex;
    }

    void SpawnSelectedObject(int ObjectIndex)
    {
        string path = "Prefabs/" + ObjectIndex.ToString();
        GameObject ObjectInstance = Instantiate(Resources.Load(path, typeof(GameObject))) as GameObject;
        ObjectInstance.name = ObjectIndex.ToString();
        ObjectInstance.transform.position = new Vector3(0, 0, 0);
        Bounds bounds = GetBounds(ObjectInstance);
        float xpose = nextpoint.x + bounds.extents.x;
        float zpose = nextpoint.z + bounds.extents.z;
        float ypose = nextpoint.y;

        ObjectInstance.transform.position = new Vector3(xpose, ypose, zpose);
        xstep = bounds.size.x;
        zstep = bounds.size.z;
        
    }

    Bounds GetBounds(GameObject obj)
    {
        Bounds bounds = new Bounds(new Vector3(0, 0, 0), new Vector3(0, 0, 0));
        Renderer[] rends = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in rends)
        {
            if (rend.bounds.size.magnitude > bounds.size.magnitude)
            {
                bounds = rend.bounds;
            }
        }
        return bounds;
    }
   
    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawCube(new Vector3(0, -1.6f, 2.8f), bounds.size);
    //}


    IEnumerator KeepRefresh()
    {
        int counter = 0;
        while(true){
            yield return new WaitForSeconds(1f);
            DestroyAll();
            PlaceStuff();
            yield return new WaitForSeconds(0.1f);
            GameObject[] Stuffs = GameObject.FindGameObjectsWithTag("Stuff");
            CustomUtils.TakeScreenShots(counter, Stuffs, false,false);
            yield return new WaitForSeconds(0.1f);
            CustomUtils.SetCullingMask(true);
            yield return new WaitForSeconds(0.1f);
            CustomUtils.TakeScreenShots(counter, Stuffs, true,true);
            yield return new WaitForSeconds(0.1f);
            CustomUtils.TakeScreenShots(counter, Stuffs, true,false);
            yield return new WaitForSeconds(0.1f);
            CustomUtils.SetCullingMask(false);
            counter++;

        }
        
    }

    void DestroyAll()
    {
        GameObject[] stuffs = GameObject.FindGameObjectsWithTag("Stuff");
        foreach (GameObject stuff in stuffs) {
            Destroy(stuff);
        }
    }
}
