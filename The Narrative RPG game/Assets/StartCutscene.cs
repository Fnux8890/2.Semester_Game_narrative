using System;
using System.Collections;
using System.Collections.Generic;
using GameSystems.CustomEventSystems;
using UnityEngine;
using UnityEngine.Timeline;

public class StartCutscene : MonoBehaviour
{
    public TextAsset json;
    public TimelineAsset cutscene;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerCutSceneHandler.Instance.OnTriggerCutScene(json, cutscene.name);
        }
    }
}
