using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerJoinEvent))]
public class AddPlayerLobby : Editor
{
    private static int index = 0;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (EditorApplication.isPlaying)
        {
            if (GUILayout.Button("Add a new player"))
            {
                ((PlayerJoinEvent)target).Raise("Jeff"+index++, Color.white);
            }
        }
    }
}
