using System;
using System.Collections;
using GameSystems.Dialogue.Dialogue_Json_Classes;
using UnityEngine;
using Utilities;

namespace GameSystems.CustomEventSystems.Interaction
{
    public class InteractionHandler : Singleton<InteractionHandler>
    {
        public event Action Interact;
        public event Action<GameObject, bool> LookingAt;
        public event Action<Node> UpdateNode;
        public event Action StartDialogue;

        public  void OnInteract()
        {
            Interact?.Invoke();
        }

        public void OnLookingAt(GameObject go, bool inRange)
        {
            LookingAt?.Invoke(go, inRange);
        }

        public void OnUpdateNode(Node obj)
        {
            UpdateNode?.Invoke(obj);
        }

        public void OnStartDialogue()
        {
            StartDialogue?.Invoke();
        }
    }
}