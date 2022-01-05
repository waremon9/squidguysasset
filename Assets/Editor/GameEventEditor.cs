using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameEvent))]
public class GameEventEditor : Editor
{
    private void OnEnable()
    {
        _event = target as GameEvent;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Raise"))
        {
            _event.Raise();
        }
    }

    private GameEvent _event;
}
