
using System;
using Dialogue.Objects;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InteractableScript))]
public class SignEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        InteractableScript script = (InteractableScript) target;
        
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.Space((Screen.width / 100) * 20 );
        if (GUILayout.Button(
            "Update Json",
            GUILayout.Width((Screen.width / 100) * 30)))
        {
            script.UpdateJson();
        }
        
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        

    }
    
}
