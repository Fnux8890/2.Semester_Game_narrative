using System;
using System.Collections;
using System.Collections.Generic;
using GameSystems.Timeline;
using UnityEngine;

public class StartCutScene : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerCutSceneHandler.Instance.OnTriggerCutScene();
        }
    }
}
