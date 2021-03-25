using System;
using System.Collections;
using System.Collections.Generic;
using Systems.Dialogue;
using Systems.Dialogue.Dialogue_Json_Classes;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public TextAsset json;
    private Dialogue[] dialogueArray;
    private Dialogue _dialogue;
    void Start()
    {
        dialogueArray = JsonHelper.getJsonArray<Dialogue>(json.text);
        _dialogue = dialogueArray[0];
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.H))
        {
            for (int i = 0; i < _dialogue.nodes.Capacity; ++i)
            {
                if (_dialogue.nodes[i].text.ENG == null) continue;
                Debug.Log(_dialogue.nodes[i].text.ENG);
            }
        }
    }
}


