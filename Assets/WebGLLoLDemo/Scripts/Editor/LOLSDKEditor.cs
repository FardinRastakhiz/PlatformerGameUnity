using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LoLSDK.LOLSDK))]
public class LOLSDKEditor : Editor
{
    public override void OnInspectorGUI ()
    {
        EditorGUILayout.LabelField($"LOL SDK: V{LoLSDK.WebGL.SDK_VERSION}");
    }
}
