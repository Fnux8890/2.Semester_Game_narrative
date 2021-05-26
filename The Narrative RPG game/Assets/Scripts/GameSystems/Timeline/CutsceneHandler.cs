using System;
using UnityEngine;
using Utilities;

namespace GameSystems.Timeline
{
    public class CutsceneHandler : Singleton<CutsceneHandler>
    {
        public event Action<TextAsset, string> StartCutsceneWithDialogue;
        public event Action<string, string> StartCutsceneWithDialogueName;
        public event Action<string> StartCutsceneWithNoDialogue;

        public void OnStartCutsceneWithDialogue(TextAsset json, string cutscene)
        {
            StartCutsceneWithDialogue?.Invoke(json, cutscene);
        }

        public void OnStartCutsceneWithNoDialogue(string obj)
        {
            StartCutsceneWithNoDialogue?.Invoke(obj);
        }

        public void OnStartCutsceneWithDialogueName(string json, string scene)
        {
            StartCutsceneWithDialogueName?.Invoke(json, scene);
        }
    }
}