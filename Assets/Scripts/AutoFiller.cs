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
    // Use this for initialization
    void Start () {
        nextpoint = binStartingPoint.transform.position;
        PlaceStuff();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void PlaceStuff()
    {
        for ( int row =0; row < 100 ;row++ )
        {
        int objindex = SelectObjectIndex();
            for (int column = 0; column < 3; column++)
            {
                SpawnSelectedObject(objindex);
                nextpoint.z += zstep;
                if ((nextpoint.z + zstep) > maxZ)
                {
                    break;
                }
            }
            nextpoint.x += xstep;
            nextpoint.z = binStartingPoint.position.z;
            if (nextpoint.x > maxX) {
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


    public void OnRefresh()
    {
        DestroyAll();
        PlaceStuff();
    }

    void DestroyAll()
    {
        GameObject[] stuffs = GameObject.FindGameObjectsWithTag("Stuff");
        foreach (GameObject stuff in stuffs) {
            Destroy(stuff);
        }
    }
}
