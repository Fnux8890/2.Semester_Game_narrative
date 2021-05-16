using System;
using GameSystems.CustomEventSystems.Interaction;
using GameSystems.Dialogue;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Utilities;

namespace Dialogue.Objects
{
    [RequireComponent(typeof(ArcCollider2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class InteractableSignScript : MonoBehaviour
    {
        public TextAsset json;

        public bool playerInRange = false;

        private TextAsset _previousJson;
    
        private PolygonCollider2D _polygonCollider;
        private ArcCollider2D _arcCollider2D;
        private BoxCollider2D _boxCollider;
        private SpriteRenderer _spriteRenderer;
        private GameObject _dialogueCanvas;
        //Input system
        private PlayerActionControls _playerActionControls;

    
        // Start is called before the first frame update
        void Start()
        {
            Setup();
            _playerActionControls.Land.Interact.performed += Interact;
        }
        
        private void OnValidate()
        {
            UpdateJson();
        }
        
        private void Interact(InputAction.CallbackContext ctx)
        {
            if (ctx.performed && playerInRange)
            {
                try
                {
                    var topDialogue = _dialogueCanvas.transform.Find("DialogueBox Top");
                    topDialogue.gameObject.SetActive(true);
                    var topDialogueText = topDialogue.transform.Find("Text");
                    topDialogueText.GetComponent<Text>().text = DialogueManager.Instance.Nodes[0].Text;
                }
                catch (Exception e)
                {
                    Debug.Log($"{e.Message}");
                    throw;
                }
            }
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                DialogueManager.Instance.LoadJson(json);
                InteractionHandler.Instance.OnLookingAt(gameObject, true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            DialogueManager.Instance.UnloadJson();
            InteractionHandler.Instance.OnLookingAt(null, false);
            DialogueUIHandler.Instance.OnExitDialogue();
        }
        
        private void Setup()
        {
            // Init
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _boxCollider = GetComponent<BoxCollider2D>();
            _arcCollider2D = GetComponent<ArcCollider2D>();
            _polygonCollider = GetComponent<PolygonCollider2D>();
            _dialogueCanvas = GameObject.Find("DialogueCanvas");
            _playerActionControls = new PlayerActionControls();

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
    
    }
}




