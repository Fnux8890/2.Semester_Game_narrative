using System;
using UnityEngine;
using Utilities;

namespace GameSystems.Dialogue
{
    public class DialogueUIHandler : Singleton<DialogueUIHandler>
    {
        public event Action ExitDialogue;

        public void OnExitDialogue()
        {
            ExitDialogue?.Invoke();
        }
    }
}