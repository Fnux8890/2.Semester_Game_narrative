using System;
using GameSystems.Dialogue;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace PlayerControl
{
public class PlayerController : MonoBehaviour
{

    private PlayerActionControls _playerActionControls;

    [SerializeField]
    private float moveSpeed;

    public Animator animator;
    
    Vector2 movementRead;

    private Rigidbody2D rb;
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");

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

    private void Update()
    {
        // read movement value
        var movement = _playerActionControls.Land.Movement.ReadValue<Vector2>();
        // Move the player
        Vector2 currentPosition = transform.position;
        currentPosition += movement * (moveSpeed * Time.deltaTime);
        transform.position = currentPosition;

        // Animate player
        animator.SetFloat(Horizontal, movement.x);
        animator.SetFloat(Vertical, movement.y);
        animator.SetFloat(Speed, movement.magnitude);
    }
    

    private void Interact()
    {
        Debug.Log("You interacted with something");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        SceneManager.LoadScene("OutsideHerosHome");
    }
}
    
}




