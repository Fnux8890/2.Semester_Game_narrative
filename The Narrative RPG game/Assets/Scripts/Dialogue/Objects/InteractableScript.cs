using System;
using GameSystems.CustomEventSystems.Interaction;
using GameSystems.Dialogue;
using NonPlayerObjects;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Utilities;

namespace Dialogue.Objects
{
    public enum InteractableDirection{
        Up,
        Down,
        Left,
        Right
    }
    
    [RequireComponent(typeof(ArcCollider2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class InteractableScript : MonoBehaviour
    {
        public InteractableDirection direction;
        public Vector2 offset;
        public Vector2 size;
        public TextAsset json;
        

        private TextAsset _previousJson;
    
        private PolygonCollider2D _polygonCollider;
        private ArcCollider2D _arcCollider2D;
        private BoxCollider2D _boxCollider;
        private SpriteRenderer _spriteRenderer;
        private GameObject _dialogueCanvas;
        private DialogueUIManager _dialogueUIManager;
        private DialogueManager _dialogueManager;

        //Input system
        private PlayerActionControls _playerActionControls;

    
        // Start is called before the first frame update
        private void Start()
        {
            _dialogueUIManager = DialogueUIManager.Instance;
            _dialogueManager = DialogueManager.Instance;
            DialogueHandleUpdate.Instance.OnUpdateCanvas();
            Setup();
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                InteractionHandler.Instance.OnShowBubble();
                DialogueHandleUpdate.Instance.OnUpdateJson(json);
                InteractionHandler.Instance.OnLookingAt(gameObject, true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            InteractionHandler.Instance.OnHideBubble();
            DialogueHandleUpdate.Instance.OnUnloadJson();
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
            switch (direction)
            {
                case InteractableDirection.Up:
                    SetDirection(1.5f, 10, 160);
                    break;
                case InteractableDirection.Down:
                    SetDirection(1.5f, 190, 160);
                    break;
                case InteractableDirection.Left:
                    SetDirection(1.5f, 100, 160);
                    break;
                case InteractableDirection.Right:
                    SetDirection(1.5f, 280, 160);
                    break;
            }
            
            _polygonCollider.isTrigger = true;

            if (_spriteRenderer.sprite.name == "outside copy_0")
            {
                _boxCollider.offset = offset;
                _boxCollider.size = size;
            }
        }

        private void SetDirection(float radius, int offset, int angle)
        {
            _arcCollider2D.Radius = radius;
            _arcCollider2D.OffsetRotation = offset;
            _arcCollider2D.TotalAngle = angle;
        }
        
    }
}




