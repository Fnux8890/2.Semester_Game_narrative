using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private PlayerActionControls _playerActionControls;

    [SerializeField]
    private float moveSpeed;

    public Animator animator;

    private void Awake()
    {
        _playerActionControls = new PlayerActionControls();
    }

    private void OnEnable()
    {
        _playerActionControls.Enable();
    }

    private void OnDisable()
    {
        _playerActionControls.Disable();
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        // read movement value
        float movementInputHorizontal = _playerActionControls.Land.MoveHorizontal.ReadValue<float>();
        float movementInputVertical = _playerActionControls.Land.MoveVertical.ReadValue<float>();
        // Move the player
        Vector3 currentPosition = transform.position;
        currentPosition.x += movementInputHorizontal * moveSpeed * Time.deltaTime;
        currentPosition.y += movementInputVertical * moveSpeed * Time.deltaTime;
        transform.position = currentPosition;
        // Animate player
        animator.SetFloat("Horizontal", movementInputHorizontal);
        animator.SetFloat("Vertical", movementInputVertical);
        animator.SetFloat("Speed", moveSpeed);
    }
}
