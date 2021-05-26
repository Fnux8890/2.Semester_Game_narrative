using System;
using System.Collections;
using System.Collections.Generic;
using GameSystems.CustomEventSystems;
using GameSystems.Dialogue;
using GameSystems.Timeline;
using UnityEngine;
using UnityEngine.Timeline;
using Utilities;

public class StartCutscene : MonoBehaviour
{
    public TextAsset json;
    public TimelineAsset cutscene;
    private CutsceneManager cutman;
    private DialogueManager diaman;
    private DialogueUIManager diamanbetter;
    private TextAsset _previousJson;

    private void OnEnable()
    {
        cutman = CutsceneManager.Instance;
        diaman = DialogueManager.Instance;
        diamanbetter = DialogueUIManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerCutSceneHandler.Instance.OnTriggerCutScene(json, cutscene.name);
        }
    }
    
    private void OnValidate()
    {
        UpdateJson();
    }
        
    public void UpdateJson()
    {
        if (json != null)
        {
            if (_previousJson == null) _previousJson = new TextAsset();
            if (json.GetType() != typeof(TextAsset))
                throw new InvalidOperationException("File can only be Text Assets");
            if (_previousJson.ToString().Equals(json.ToString()) ^ _previousJson.name == json.name) return;
            _previousJson = json;
            CustomUtils.PrettifyJson(json);
        }
            
    }
}
