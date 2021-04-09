using Interfaces;
using UnityEngine;

namespace scribble_objects.Characters
{
    [CreateAssetMenu(fileName = "New Interactable Character", menuName = "Interactable/Character/Friendly", order = 1)]
    public class CharacterInteractable : ScriptableObject, IInteractable
    {
        public TextAsset Json;

        public void Interact()
        {
            
        }

        public void StopInteract()
        {
            
        }
    }
}