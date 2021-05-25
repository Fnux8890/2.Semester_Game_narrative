using System;
using System.Collections;
using System.Collections.Generic;
using GameSystems.CustomEventSystems;
using GameSystems.Timeline;
using UnityEngine;
using UnityEngine.Timeline;
using Utilities;

public class StartCutScene : MonoBehaviour
{
    public TimelineAsset cutscene;
    public TextAsset json;
    private TextAsset _previousJson;

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
        if (_previousJson == null) _previousJson = new TextAsset();
        if (json.GetType() != typeof(TextAsset))
            throw new InvalidOperationException("File can only be Text Assets");
        if (_previousJson.ToString().Equals(json.ToString()) ^ _previousJson.name == json.name) return;
        _previousJson = json;
        CustomUtils.PrettifyJson(json);
    }
}
