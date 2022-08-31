using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Componets")]
    //Components of the player
    [SerializeField] private Rigidbody _playerRigidbody;
    private PlayerActions _playerInputActions;

    [Header("Character Stats")]
    //All the player stats
    private float _moveSpeed;
    [SerializeField] [Tooltip("How fast the player moves while walking when given input.")] private float _walkSpeed;
    [SerializeField] [Tooltip("How fast the player moves while sprinting when given input.")] private float _sprintSpeed;
    [SerializeField] [Tooltip("How high the player can jump.")] private float _jumpHeight;
    [SerializeField] private int _numOfJumps;
    [SerializeField] private int _currentNumOfJump;
    private bool _hasJumped;
    [SerializeField] private float _colImpactVelocity;

    [Header("Gravity")]
    //Gravity on player
    [SerializeField] [Tooltip("How much the world is pulling on player.")] private float _gravity;
    private float _ySpeed;

    [Header("Slopes")]
    [SerializeField] private float _maxSlopeAngle;
    [SerializeField] private float _slopeForce;

    [Header("Camera")]
    //Camera
    [SerializeField] [Tooltip("The camera in the scene.")] private Transform _cam;
    [SerializeField] [Tooltip("How fast the player turns in the direction of movement")] private float _turnSmoothTime;

    [Header("Grounded")]
    //Raycast
    [SerializeField] [Tooltip("The position of the ground ray.")] private Transform _groundRayPos;
    [SerializeField] [Tooltip("How far the raycast shoots.")] private float _maxRayDist;
    [SerializeField] [Tooltip("The layers that the ray can hit.")] private LayerMask _groundLayer;
    private bool _isGrounded;
    private RaycastHit _rayGroundHit;
    private bool _prevGrounded;

    [Header("Character States")]
    public MovemoventState state;
    public enum MovemoventState
    {
        idle,
        walking,
        sprinting,
        crouching,
        flying
    }

    [Header("Move Inputs")]
    //Movement inputs
    private InputAction _move;
    private Vector2 _moveInput;
    private InputAction _sprint;

    [Header("Jump Inputs")]
    //Jumping inputs
    private InputAction _jump;

    [Header("Particles")]
    [SerializeField] private Transform _dustParticlePos;
    [SerializeField] private ParticleSystem _dustParticle;
    private bool _dustParticlePlayed;
    [SerializeField] private Transform _walkParticlePos;
    [SerializeField] private ParticleSystem _walkParticle;

    [Header("Sounds")]
    [SerializeField] private AudioSource _walkSound;
    [SerializeField] private AudioSource _landSound;


    private void Awake()
    {
        //Get Rigidbody
        _playerRigidbody = GetComponent<Rigidbody>();

        //Inputs
        _playerInputActions = new PlayerActions();
    }

    private void Update()
    {
        _moveInput = _move.ReadValue<Vector2>();

        if (_isGrounded && Time.timeScale != 0f)
        {
            if (_moveInput.magnitude != 0f)
            {
                if (!_walkSound.isPlaying)
                {
                    _walkSound.Play();
                }
            }

            if (!_prevGrounded)
            {
                if (!_landSound.isPlaying)
                {
                    _landSound.Play();

                    _hasJumped = false;
                }
            }

            if (!_hasJumped)
            {
                _currentNumOfJump = 0;
            }
        }


        _prevGrounded = _isGrounded;

        //Check player is grounded
        HandleGrounded();

        //Moving Platform
        SetPlatform(_rayGroundHit);

        //Changes player state
        HandleState();
    }

    private void FixedUpdate()
    {
        //Apply Gravity
        HandleGravity();

        //Move player
        HandleMovement();

        PushOffSlope();
    }

    private void HandleState()
    {
        //State - Sprinting
        if(_isGrounded && _sprint.ReadValue<float>() >= 0.1f)
        {
            state = MovemoventState.sprinting;
            _moveSpeed = _sprintSpeed;
        }
        //State - Walking
        else if(_isGrounded)
        {
            state = MovemoventState.walking;
            _moveSpeed = _walkSpeed;
        }
        //State - Flying
        else
        {
            state = MovemoventState.flying;
        }

        //State - Crouching
    }

    private void HandleMovement()
    {
        //Calculate input direction
        Vector3 inputDir = new Vector3(_moveInput.x, 0, _moveInput.y);

        //Calculate Camera direction
        Vector3 camForward = _cam.forward;
        Vector3 camRight = _cam.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward = camForward.normalized;
        camRight = camRight.normalized;


        if (inputDir.magnitude >= 0.1f)
        {
            PlayParticle(_walkParticle, _walkParticlePos);

            if (OnSlope())
            { 
                //Defualt move in relation of camera direction
                Vector3 camDirMove = camForward * inputDir.z + camRight * inputDir.x;

                Vector3 projectOnPlane = Vector3.ProjectOnPlane(camDirMove, _rayGroundHit.normal).normalized;

                AddForceToMove(projectOnPlane, _moveSpeed);
            }
            else
            {
                //Defualt move in relation of camera direction
                Vector3 camDirMove = camForward * inputDir.z + camRight * inputDir.x;

                AddForceToMove(camDirMove, _moveSpeed);
            }
        }
        else
        {
            StopParticle(_walkParticle);
        }
    }

    private void AddForceToMove(Vector3 moveDir, float moveSpeed)
    {
        //Move player with force
        _playerRigidbody.AddForce(moveDir * moveSpeed, ForceMode.Acceleration);

        //Calculate input direction
        Vector3 inputDir = new Vector3(_moveInput.x, 0, _moveInput.y);

        //Calculate Camera direction
        Vector3 camForward = _cam.forward;
        Vector3 camRight = _cam.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward = camForward.normalized;
        camRight = camRight.normalized;

        //Defualt move in relation of camera direction
        Vector3 camDirMove = camForward * inputDir.z + camRight * inputDir.x;

        //Handle Rotation
        //Rotate player in move direction
        Quaternion toRotate = Quaternion.LookRotation(camDirMove, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, _turnSmoothTime * 100f * Time.deltaTime);
    }

    private void HandleJump(InputAction.CallbackContext context)
    {
        if (_currentNumOfJump >= _numOfJumps || CalculateSlopeAngle() >= _maxSlopeAngle)
        { return; }

        if (Time.timeScale != 0f)
        {
            //Count # of jumps
            _currentNumOfJump++;

            _hasJumped = true;

            //Apply jump force to rigidbody
            _playerRigidbody.AddForce(_jumpHeight * Vector3.up, ForceMode.VelocityChange);
        }
    }

    private void HandleGravity()
    {
        if (OnSlope())
        {
            //if on slope stop gravity
            _playerRigidbody.useGravity = false;
        }
        else
        {
            //if not on slope use gravity to fall 
            _playerRigidbody.useGravity = true;
        }

        //Force player downward
        _playerRigidbody.AddForce(CalculateGravityForce() * Vector2.up);
    }

    private float CalculateGravityForce()
    {
        if (_isGrounded)
        {
            _ySpeed = 0f;
        }
        else
        {
            if (Time.timeScale != 0f)
            {
                _ySpeed = _gravity;
            }
        }

        return _ySpeed;
    }

    private void PushOffSlope()
    {
        if (CalculateSlopeAngle() >= _maxSlopeAngle)
        {
            var normal = _rayGroundHit.normal;
            var yInverse = 1f - normal.y;
            Vector3 slideDir = new Vector3(yInverse * normal.x, 0f, yInverse * normal.z);
            _playerRigidbody.AddForce(slideDir * _slopeForce, ForceMode.VelocityChange);
            _playerRigidbody.AddForce(Vector3.down * _slopeForce / 4f, ForceMode.VelocityChange);
        }
    }

    private void HandleGrounded()
    {
        //Fire Raycast
        if (Physics.Raycast(_groundRayPos.position, Vector3.down, out _rayGroundHit, _maxRayDist, _groundLayer))
        {
            //Hit ground
            _isGrounded = true;

            //Particle
            if (!_dustParticle.isPlaying && !_dustParticlePlayed)
            {
                PlayParticle(_dustParticle, _dustParticlePos);
                _dustParticlePlayed = true;
            }
        }
        else
        {
            //Did not hit ground
            _isGrounded = false;

            _dustParticlePlayed = false;
        }
    }

    private float CalculateSlopeAngle()
    {
        float angleOfSlope = Vector3.Angle(Vector3.up, _rayGroundHit.normal);
        return angleOfSlope;
    }

    private bool OnSlope()
    {
        if (_isGrounded)
        {
            bool slopeCheck = CalculateSlopeAngle() < _maxSlopeAngle && CalculateSlopeAngle() != 0;
            return (slopeCheck);
        }
        return false;
    }

    private void SetPlatform(RaycastHit rayHit)
    {
        try
        {
            MovingPlatform movingPlatform = rayHit.transform.GetComponent<MovingPlatform>();
            transform.SetParent(movingPlatform.transform);
        }
        catch
        {
            transform.SetParent(null);
        }
    }

    private void PlayParticle(ParticleSystem particleSystem, Transform particlePos)
    {
        if (!particleSystem.isPlaying && _isGrounded && Time.timeScale != 0f)
        {
            Vector3 pos = _rayGroundHit.point + new Vector3(0f, 0.25f, 0f);
            particlePos.position = pos;
            particleSystem.Play();
        }
    }

    private void StopParticle(ParticleSystem particleSystem)
    {
        if (particleSystem.isPlaying)
        {
            particleSystem.Stop();
        }
    }

    private void OnEnable()
    {
        //Movin player inputs
        _move = _playerInputActions.PlayerControls.Move;
        _move.Enable();

        //Sprint
        _sprint = _playerInputActions.PlayerControls.Sprint;
        _sprint.Enable();

        //Jumping player inputs
        _jump = _playerInputActions.PlayerControls.Jump;
        _jump.Enable();
        _jump.performed += HandleJump;
    }

    private void OnDisable()
    {
        _move.Disable();
        _sprint.Disable();
        _jump.Disable();
    }
}
