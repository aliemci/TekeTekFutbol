using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefabReader : MonoBehaviour    
{
    public GameObject child;


    public void Start()
    {
        #if UNITY_EDITOR

        var go = PrefabUtility.GetNearestPrefabInstanceRoot(child);

        print(go);
        print(AssetDatabase.GetAssetPath(go));

        #endif
        
    }




}
