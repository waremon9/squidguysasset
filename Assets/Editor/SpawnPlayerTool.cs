using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpawnManager))]
public class SpawnPlayerTool : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        if (EditorApplication.isPlaying)
        {
            if (GUILayout.Button("Spawn a new player"))
            {
                ((SpawnManager)target).SpawnNewPlayer("Test spawn");
            }
        }
    }
}
