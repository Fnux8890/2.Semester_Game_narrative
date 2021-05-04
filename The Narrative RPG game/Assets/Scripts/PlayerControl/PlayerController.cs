using System;
using System.Linq;
using UnityEngine;
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
    
    //Input system
    private PlayerActionControls _playerActionControls;
    
    // animator
    private static readonly int Vertical = Animator.StringToHash("MoveY");
    private static readonly int Horizontal = Animator.StringToHash("MoveX");
    private static readonly int Moving = Animator.StringToHash("Moving");

    private void Start()
    {
        _grid = GameObject.Find("Grid").GetComponent<Grid>();
        tilemap = GameObject.Find("Grid").transform.Find("StartingLevelGround").GetComponent<Tilemap>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        _playerActionControls = new PlayerActionControls();
        _playerActionControls.Land.Sprint.performed += _ => SprintAction(true, 9);
        _playerActionControls.Land.Sprint.canceled += _ => SprintAction(false, 5);
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


    public void Interact()
    {
        Debug.Log("You interacted with something");
    }
    
    private void SprintAction(bool isSprinting, int speed)
    {
        _isSprinting = isSprinting;
        _moveSpeed = speed;
    }
    
    private void SetSpeed(int onPath, int offPath)
    {
        _moveSpeed = Array.Exists(path,
            element => element.name == _tile.name)
            ? onPath
            : offPath;
    }
    
    
}
    
}




