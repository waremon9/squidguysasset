using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioObject), true)]
public class SoundPreview : Editor
{
    [SerializeField] private AudioSource Previewer;

    public void OnEnable()
    {
        Previewer = EditorUtility.CreateGameObjectWithHideFlags("Audio preview", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();
    }

    public void OnDisable()
    {
        DestroyImmediate(Previewer);
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        EditorGUI.BeginDisabledGroup((serializedObject.isEditingMultipleObjects));
        if (GUILayout.Button("Preview"))
        {
            ((AudioObject)target).Play(Previewer);
        }
        EditorGUI.EndDisabledGroup();
    }
}
