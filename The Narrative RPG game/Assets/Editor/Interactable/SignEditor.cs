
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


        signScript.enumDirection =
            (InteractDirection) EditorGUILayout.EnumPopup("Direction", signScript.enumDirection);


        if (signScript.bc!=null)
        {
            signScript.bc.transform.position = signScript.test.GETDirection(signScript.enumDirection);
        }
    }
}
