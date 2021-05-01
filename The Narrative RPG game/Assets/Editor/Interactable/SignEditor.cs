
using System;
using scribble_objects.Characters;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InteractableSignScript))]
public class SignEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        InteractableSignScript signScript = (InteractableSignScript) target;
        
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.Space((Screen.width / 100) * 20 );
        if (GUILayout.Button(
            "Update Json",
            GUILayout.Width((Screen.width / 100) * 30)))
        {
            signScript.UpdateJson();
        }
        
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        

    }
    
}
