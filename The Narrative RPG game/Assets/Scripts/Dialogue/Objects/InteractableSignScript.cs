using System;
using GameSystems.Dialogue;
using scribble_objects.Characters;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Dialogue.Objects
{
    [RequireComponent(typeof(ArcCollider2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class InteractableSignScript : MonoBehaviour
    {
        public CharacterInteractable test;
        public TextAsset json;

        public bool PlayerInRange = false;

        private TextAsset _previousJson;
    
        private PolygonCollider2D _polygonCollider;
        private ArcCollider2D _arcCollider2D;
        private BoxCollider2D _boxCollider;
        private SpriteRenderer _spriteRenderer;
        private GameObject _dialogueCanvas;
        

    
        // Start is called before the first frame update
        void Start()
        {
            // Init
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _boxCollider = GetComponent<BoxCollider2D>();
            _arcCollider2D = GetComponent<ArcCollider2D>();
            _polygonCollider = GetComponent<PolygonCollider2D>();
            _dialogueCanvas = GameObject.Find("DialogueCanvas");


            //modify
            _arcCollider2D.PizzaSlice = true;
            _arcCollider2D.Radius = (float) 1.5;
            _arcCollider2D.OffsetRotation = 190;
            _arcCollider2D.TotalAngle = 160;

            _polygonCollider.isTrigger = true;

            if (_spriteRenderer.sprite.name == "outside_4")
            {
                _boxCollider.offset = new Vector2((float) 0.001211166, (float) 0.2278429);
                _boxCollider.size = new Vector2((float) 0.9312592, (float) 0.3999817);
            }

        }

        private void OnValidate()
        {
            UpdateJson();
        }

        public void UpdateJson()
        {
            if (_previousJson == null) _previousJson = new TextAsset();
            if (json.GetType() != typeof(TextAsset))
                throw new InvalidOperationException("File can only be Text Assets");
            if (_previousJson.ToString().Equals(json.ToString()) ^ _previousJson.name == json.name) return;
            _previousJson = json;
            CustomUtils.PrettifyJson(json);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.E) && PlayerInRange)
            {
                try
                {
                    var topDialogue = _dialogueCanvas.transform.Find("DialogueBox Top");
                    topDialogue.gameObject.SetActive(true);
                    var topDialogueText = topDialogue.transform.Find("Text");
                    topDialogueText.GetComponent<Text>().text = DialogueMananger.Instance.Nodes[0].Text;
                }
                catch (Exception e)
                {
                    Debug.Log($"{e.Message}");
                    throw;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerInRange = true;
                DialogueMananger.Instance.LoadJson(json);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerInRange = false;
                DialogueMananger.Instance.UnloadJson();
                var topDialogue = _dialogueCanvas.transform.Find("DialogueBox Top");
                topDialogue.gameObject.SetActive(false);
                var topDialogueText = topDialogue.transform.Find("Text");
                topDialogueText.GetComponent<Text>().text = null;
            }
        }
    
    }
}




