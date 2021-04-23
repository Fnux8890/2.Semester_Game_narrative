
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
        
    }
}
