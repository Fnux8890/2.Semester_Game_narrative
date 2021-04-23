using System;
using Interfaces;
using UnityEngine;

namespace scribble_objects.Characters
{
    public enum InteractDirection{
        North,
        South,
        West,
        East
    }
    
    [CreateAssetMenu(fileName = "New Interactable Character", menuName = "Interactable/Character/Friendly", order = 1)]
    public class CharacterInteractable : ScriptableObject, IInteractable
    {
        public TextAsset Json;
        

        private void OnEnable()
        {
            
        }

        public void Interact()
        {
            throw new NotImplementedException();
        }

        public void StopInteract()
        {
            throw new NotImplementedException();
        }
    }
}