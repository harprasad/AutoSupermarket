using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AutoPrefab : MonoBehaviour {
    
    [MenuItem("CustomTool/Create Simple Prefab")]
    static void DoCreateSimplePrefab()
    {
        Transform[] transforms = Selection.transforms;
        
        foreach (Transform t in transforms)
        {
            GameObject parent = new GameObject();
            parent.name = t.name;
            parent.transform.position = t.position;
            t.parent = parent.transform;
            parent.tag = "Stuff";
            Object prefab = PrefabUtility.CreateEmptyPrefab("Assets/GeneratedPrefabs/" + parent.name + ".prefab");
            PrefabUtility.ReplacePrefab(parent, prefab, ReplacePrefabOptions.ConnectToPrefab);
        }
    }


    [MenuItem("CustomTool/Change Scale")]
    static void ChangeScales()
    {
        float scalefactor = 0.2f;
        Transform[] transforms = Selection.transforms;
        foreach (Transform t in transforms)
        {
            t.localScale = new Vector3(scalefactor, scalefactor, scalefactor);
        }
    }

}
