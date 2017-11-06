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
            Object prefab = PrefabUtility.CreateEmptyPrefab("Assets/GeneratedPrefabs/" + t.gameObject.name + ".prefab");
            PrefabUtility.ReplacePrefab(t.gameObject, prefab, ReplacePrefabOptions.ConnectToPrefab);
        }
    }
}
