using System;
using System.Reflection;
using Systems.Dialogue;
using Systems.Dialogue.Dialogue_Json_Classes;
using Newtonsoft.Json;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public TextAsset json;
    private Dialogue[] _dialogueArray;
    private Dialogue _dialogue;
    void Start()
    {
        //_dialogueArray = JsonConvert.DeserializeObject<Dialogue>(json.text);
        _dialogueArray = JsonHelper.getJsonArray<Dialogue>(json.text);
        _dialogue = _dialogueArray[0];

    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.H))
        {
            //Type type = typeof(Dialogue);
            //PropertyInfo[] paramsArray = type.GetProperties();
            foreach (var node in _dialogue.nodes)
            {
                Debug.Log(node.NodeName);
            }
        }
    }
}


