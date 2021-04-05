using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

namespace Dialogue
{
    public class SpeakerUI : MonoBehaviour
    {
        public Image image;
        public Text characterName;
        public Text dialogue;
        
        public string CharacterName
        {
            get => characterName.text;
            set => characterName.text = value;
        }

        public string Dialogue
        {
            get => dialogue.text;
            set => dialogue.text = value;
        }

        
    }
}