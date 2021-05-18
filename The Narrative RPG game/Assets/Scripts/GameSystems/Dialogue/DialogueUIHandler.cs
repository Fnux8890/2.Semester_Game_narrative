using System;
using GameSystems.Dialogue.Dialogue_Json_Classes;
using UnityEngine;
using Utilities;

namespace GameSystems.Dialogue
{
    public class DialogueUIHandler : Singleton<DialogueUIHandler>
    {
        public event Action<Node> ShowDialogue;
        public event Action ExitDialogue;

        public void OnExitDialogue()
        {
            ExitDialogue?.Invoke();
        }

        public void OnShowDialogue(Node obj)
        {
            ShowDialogue?.Invoke(obj);
        }
    }
}