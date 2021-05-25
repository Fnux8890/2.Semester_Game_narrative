using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class TriggerCutSceneHandler : Singleton<TriggerCutSceneHandler>
{
    public event Action TriggerCutScene;

    public void OnTriggerCutScene()
    {
        TriggerCutScene?.Invoke();
    }
}
