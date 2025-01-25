using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float _moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool _readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float _startYScale;

    [Header("Keybinding")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask Walkable;
    public bool _grounded;

    public Transform orientation;

    private float _horizontalInput;
    private float _verticalInput;

    private Vector3 _moveDirection;

    private Rigidbody _rb;

    public MovementState state;
    public enum MovementState
    {
        Walking,
        Sprinting,
        Crouching,
        Air
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;

        _readyToJump = true;
        _startYScale = transform.localScale.y;
    }

    private void Update()
    {
        // ground check
        _grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, Walkable);

        MyInput();
        SpeedControl();
        StateHandler();

        // handle drag
        if (_grounded)
            _rb.drag = groundDrag;
        else
            _rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if(Input.GetKey(jumpKey) && _readyToJump && _grounded)
        {
            _readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // start crouch
        if (Input.GetKeyDown(crouchKey))
        {
            var transform1 = transform;
            var localScale = transform1.localScale;
            localScale = new Vector3(localScale.x, crouchYScale, localScale.z);
            transform1.localScale = localScale;
            _rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
            
        // stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            var transform1 = transform;
            var localScale = transform1.localScale;
            localScale = new Vector3(localScale.x, _startYScale, localScale.z);
            transform1.localScale = localScale;
        }
    }

    private void StateHandler()
    {
        switch (_grounded)
        {
            // Mode - Crouching
            case true when Input.GetKey(crouchKey):
                state = MovementState.Crouching;
                _moveSpeed = crouchSpeed;
                break;
            // Mode - Sprinting
            case true when Input.GetKey(sprintKey):
                state = MovementState.Sprinting;
                _moveSpeed = sprintSpeed;
                break;
            // Mode - Walking
            case true:
                state = MovementState.Walking;
                _moveSpeed = walkSpeed;
                break;
            // Mode - Air
            default:
                state = MovementState.Air;
                break;
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;

        // on ground
        if(_grounded)
            _rb.AddForce(_moveDirection.normalized * (_moveSpeed * 10f), ForceMode.Force);

        // in air
        else if(!_grounded)
            _rb.AddForce(_moveDirection.normalized * (_moveSpeed * 10f * airMultiplier), ForceMode.Force);
        
    }

    private void SpeedControl()
    {
        // limiting speed on ground or in air
            var velocity = _rb.velocity;
            var flatVel = new Vector3(velocity.x, 0f, velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > _moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * _moveSpeed;
                _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
            }
    }

    private void Jump()
    {
        // reset y velocity
        var velocity = _rb.velocity;
        velocity = new Vector3(velocity.x, 0f, velocity.z);
        _rb.velocity = velocity;

        _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        _readyToJump = true;
    }

}