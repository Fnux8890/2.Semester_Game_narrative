using System;
using GameSystems.Dialogue;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControl
{
public class PlayerController : MonoBehaviour
{

    private PlayerActionControls _playerActionControls;
    private DialogueManager _dialogueManager;

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private TextAsset json;


    private void Awake()
    {
        _playerActionControls = new PlayerActionControls();
        _playerActionControls.Land.Interact.performed += ctx => Interact();
    }

    private void OnEnable()
    {
        _playerActionControls.Enable();
    }

    private void OnDisable()
    {
        _playerActionControls.Disable();
    }

    private void Update()
    {
        // read movement value
        var movement = _playerActionControls.Land.Movement.ReadValue<Vector2>();
        // Move the player
        Vector2 currentPosition = transform.position;
        currentPosition += movement * (moveSpeed * Time.deltaTime);
        transform.position = currentPosition;
    }

    private void Interact()
    {
        Debug.Log("You interacted with something");
    }
    
    private void LoadJson()
    {
        if (Input.GetKeyUp("y") && _dialogueManager == null)
        {
            try
            {
                
                _dialogueManager = new DialogueManager(json);
            }
            finally
            {
                Debug.Log("Dialogue loaded");
            }
        }

        if (Input.GetKeyUp("u") && _dialogueManager != null)
        {
            try
            {
                _dialogueManager = null;
            }
            finally
            {
                Debug.Log($"Dialogue disposed {_dialogueManager?.Equals(null)}");
            }
        }

        if (Input.GetKeyUp("h") && _dialogueManager != null)
        {
            Debug.Log(_dialogueManager.Dialogue.nodes[2].Text);
        }
    }
}
    
}




