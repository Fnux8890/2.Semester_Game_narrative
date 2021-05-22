using System;
using System.Linq;
using GameSystems.CustomEventSystems.Interaction;
using GameSystems.CustomEventSystems.Tutorial;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace PlayerControl
{
public class PlayerController : MonoBehaviour
{
    // Public unity stuff
    public Animator animator;
    public Tilemap tilemap;
    public Sprite[] path;
    
    // Private unity stuff
    private Grid _grid;
    private Rigidbody2D _rb;
    private Vector3Int _lPos;
    private Tile _tile;
    
    // private variables
    [SerializeField]
    private float _moveSpeed = 5;
    private bool _isSprinting = false;
    private GameObject _lookingAt;
    private bool _playerInRange;
    
    //Input system
    private PlayerActionControls _playerActionControls;
    
    // animator
    private static readonly int Vertical = Animator.StringToHash("MoveY");
    private static readonly int Horizontal = Animator.StringToHash("MoveX");
    private static readonly int Moving = Animator.StringToHash("Moving");

    private void Start()
    {
        if (GameObject.Find("Grid").GetComponent<Grid>() != null &&
            GameObject.Find("Grid").transform.Find("Ground") != null)
        {
            _grid = GameObject.Find("Grid").GetComponent<Grid>();
            tilemap = GameObject.Find("Grid").transform.Find("Ground").GetComponent<Tilemap>();
        }
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        _playerActionControls = PlayerActionControlsManager.Instance.PlayerControls;
        _playerActionControls.Land.Sprint.performed += _ => SprintAction(true, 9);
        _playerActionControls.Land.Sprint.canceled += _ => SprintAction(false, 5);
        _playerActionControls.Land.Movement.started += ctx => 
            TutorialHandler.Instance.OnTutorialButtonPressed(ctx);
        _playerActionControls.Land.Movement.canceled += ctx => 
            TutorialHandler.Instance.OnTutorialButtonPressed(ctx);
        _playerActionControls.Land.Interact.started += ctx => 
            TutorialHandler.Instance.OnTutorialButtonPressed(ctx);
        _playerActionControls.Land.Interact.canceled += ctx => 
            TutorialHandler.Instance.OnTutorialButtonPressed(ctx);
        _playerActionControls.Land.Interact.performed += Interact;
        InteractionHandler.Instance.LookingAt += LookingAt;
        
    }
    

    private void Update()
    {
        if (_lPos != _grid.WorldToCell(transform.position))
        {
            _lPos = _grid.WorldToCell(transform.position);
            _tile = (Tile) tilemap.GetTile(_lPos);
            if (_isSprinting)
            {
                SetSpeed(9,6);
            }
            else
            {
                SetSpeed(5,4);
            }
        }
    }


    private void FixedUpdate()
    {
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        var change = Vector2.zero;
        // read movement value
        change = _playerActionControls.Land.Movement.ReadValue<Vector2>();
        if (change != Vector2.zero)
        {
            // Move the player
            Vector2 currentPosition = transform.position;
            currentPosition += change * (_moveSpeed * Time.deltaTime);
            _rb.MovePosition(currentPosition);
            //transform.position = currentPosition;


            // Animate player
            animator.SetFloat(Horizontal, change.x);
            animator.SetFloat(Vertical, change.y);
            animator.SetBool(Moving, true);
        }
        else
        {
            animator.SetBool(Moving, false);
        }
        
    }

    private void LookingAt(GameObject lookingAt, bool inRange)
    {
        _lookingAt = lookingAt;
        _playerInRange = inRange;
    }
    
    private void Interact(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && _lookingAt != null && _playerInRange)
        {
            InteractionHandler.Instance.OnInteract();
        }
    }
    
    private void SprintAction(bool isSprinting, int speed)
    {
        _isSprinting = isSprinting;
        _moveSpeed = speed;
    }
    
    private void SetSpeed(int onPath, int offPath)
    {
        _moveSpeed = Array.Exists(path,
            element =>
            {
                if (_tile!=null)
                {
                 return element.name == _tile.name;
                }
                return false;
            })
            ? onPath
            : offPath;
    }
    
    
}
    
}




