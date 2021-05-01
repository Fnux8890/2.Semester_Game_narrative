using UnityEngine;

namespace Dialogue.Interactable
{
    public abstract class Interactable : MonoBehaviour
    {
        public enum InteractionType
        {
            Enter,
            Interact
        }
        
        public InteractionType interactionType;

        public abstract string GetDescription();
        public abstract void Interact();
    }
}