using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraResizer))]
public class CameraResizerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Bake"))
        {
            var x = UnityEditor.Handles.GetMainGameViewSize();
            ((CameraResizer)target).DoUpdate(x);
        }
    }
}
