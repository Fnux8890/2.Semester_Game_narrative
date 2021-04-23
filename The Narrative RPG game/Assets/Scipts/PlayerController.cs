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
    
    Vector2 movementRead;
    private Vector2 currentPosition;

    private Rigidbody2D rb;

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
        rb = transform.GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        // read movement value
        float movementInputHorizontal = _playerActionControls.Land.MoveHorizontal.ReadValue<float>();
        float movementInputVertical = _playerActionControls.Land.MoveVertical.ReadValue<float>();

        currentPosition.x = movementInputHorizontal;
        currentPosition.y = movementInputVertical;

        // Animate player
        animator.SetFloat("Horizontal", movementInputHorizontal);
        animator.SetFloat("Vertical", movementInputVertical);
        animator.SetFloat("Speed", currentPosition.sqrMagnitude);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + currentPosition * (moveSpeed * Time.deltaTime));
    }
}
