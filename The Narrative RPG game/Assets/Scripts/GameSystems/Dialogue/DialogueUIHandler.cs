using System;
using System.Collections;
using GameSystems.Dialogue.Dialogue_Json_Classes;
using UnityEngine;
using Utilities;

namespace GameSystems.Dialogue
{
    public class DialogueUIHandler : Singleton<DialogueUIHandler>
    {
        public event Func<Node, IEnumerator> ShowDialogue;
        public event Action ExitDialogue;

        public void OnExitDialogue()
        {
            ExitDialogue?.Invoke();
        }

        public IEnumerator OnShowDialogue(Node obj)
        {
            yield return ShowDialogue?.Invoke(obj);
        }
    }
}