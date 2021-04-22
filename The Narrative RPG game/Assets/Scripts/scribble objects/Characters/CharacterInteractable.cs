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
        public Vector2 Direction { get; set;}
        public InteractDirection _enumDirection;
        public InteractDirection EnumDirection
        {
            get => _enumDirection;
            set
            {
                _enumDirection = value;
                Direction = GETDirection(value);
                
            }
        }


        private void OnEnable()
        {
            Direction = GETDirection(_enumDirection);
        }

        public Vector2 GETDirection(InteractDirection direction)
        {
            return direction switch
            {
                InteractDirection.North => new Vector2(0, 1),
                InteractDirection.West => new Vector2(-1, 0),
                InteractDirection.East => new Vector2(1, 0),
                InteractDirection.South => new Vector2(0, -1),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void Interact()
        {
            
        }

        public void StopInteract()
        {
            
        }
        
    }
}