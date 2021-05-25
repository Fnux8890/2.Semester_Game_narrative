using System;
using UnityEngine;
using Utilities;

namespace GameSystems.CustomEventSystems
{
    public class TriggerCutSceneHandler : Singleton<TriggerCutSceneHandler>
    {
        public event Action<TextAsset, string> TriggerCutScene;

        public void OnTriggerCutScene(TextAsset json, string cutscene)
        {
            TriggerCutScene?.Invoke(json, cutscene);
        }
    }
}
