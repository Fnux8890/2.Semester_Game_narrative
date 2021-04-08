using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

namespace Dialogue
{
    public class SpeakerUI : MonoBehaviour
    {
        private Image _image;
        private Text _characterName;
        private Text _dialogue;

        public string CharacterName
        {
            get => _characterName.text;
            set => _characterName.text = value;
        }

        public string Dialogue
        {
            get => _dialogue.text;
            set => _dialogue.text = value;
        }

        private void Start()
        {
            var componentList = GetComponentsInChildren<Component>().ToList();
            _image = InstanceOfType <Image> (componentList);
            _characterName = InstanceOfType <Text> (componentList, "Name");
            _dialogue = InstanceOfType <Text> (componentList, "Dialogue");
        }

        private T InstanceOfType<T>(List<Component> list)
        {
            var result =  list.Find(component => 
                component.GetType() == typeof(T));
            var resultOfType = (T) Convert.ChangeType(result, typeof(T));
            return resultOfType;
        } 
        private T InstanceOfType<T>(List<Component> list, string search)
        {
            var result = list.Find(component => 
                component.GetType() == typeof(T) && component.name == search);
            var resultOfType = (T) Convert.ChangeType(result, typeof(T));
            return resultOfType;
        } 
        

        

        
    }
}