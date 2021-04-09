using UnityEngine;

namespace scribble_objects.Characters
{
    [CreateAssetMenu(fileName = "New Character", menuName = "Non-Interactable/Character/Friendly", order = 1)]
    public class Character : ScriptableObject
    {
        public int Health;
        public int Mana;

    }
}
