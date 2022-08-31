using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerFloat : MonoBehaviour
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

    [Header("Spring")]
    //Keeps player floating
    [SerializeField] [Tooltip("How hard the spring will push off the ground. Large value: large force pushing off ground, Low value: small force pushing off ground.")] private float _springStrength;
    [SerializeField] [Tooltip("Dampens the spring. Large value = slower the spring returns to 0, Lower value = faster the spring returns to 0.")] private float _springDamper;
    [SerializeField] [Tooltip("How high off the ground the player sits.")] private float _floatHeight;

    [Header("Upright")]
    //Keeps player upright
    [SerializeField] [Tooltip("How fast the player will return to upright position.")] private float _uprightSpringStrength;
    [SerializeField] [Tooltip("Dampens the upright Spring.")] private float _uprightSpringDamper;
    private RaycastHit _hitBL;
    private RaycastHit _hitBR;
    private RaycastHit _hitFL;
    private RaycastHit _hitFR;

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

            if (CalculateSlopeAngle() >= _maxSlopeAngle)
            {
                _currentNumOfJump = _numOfJumps;
            }
            else
            {
                if(!_hasJumped)
                {
                    _currentNumOfJump = 0;
                }
            }
        }


        _prevGrounded = _isGrounded;

        //Moving Platform
        SetPlatform(_rayGroundHit, "Moving Platform");

        //Changes player state
        HandleState();
    }

    private void FixedUpdate()
    {
        //Apply Gravity
        HandleGravity();

        //Keeps player off ground
        HandleFloat();

        //Keeps player upright
        HandleUpright();

        //Move player
        HandleMovement();
    }

    private void HandleState()
    {
        //State - Sprinting
        if (_isGrounded && _sprint.ReadValue<float>() >= 0.1f)
        {
            state = MovemoventState.sprinting;
            _moveSpeed = _sprintSpeed;
        }
        //State - Walking
        else if (_isGrounded)
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

                //Calculates the plane of the slope
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

        if(OnSlope())
        {
            //Handle Rotation
            //Rotate player in move direction
            Quaternion toRotate = Quaternion.LookRotation(moveDir, _rayGroundHit.normal);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, _turnSmoothTime * 100f * Time.deltaTime);
        }
        else
        {
            //Handle Rotation
            //Rotate player in move direction
            Quaternion toRotate = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, _turnSmoothTime * 100f * Time.deltaTime);
        }
    }

    private void HandleJump(InputAction.CallbackContext context)
    {
        if (_currentNumOfJump >= _numOfJumps || CalculateSlopeAngle() >= _maxSlopeAngle)
        { return; }

        if(Time.timeScale != 0f)
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
        if(OnSlope())
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
            _playerRigidbody.AddForce(slideDir * _slopeForce);
        }
    }

    private void HandleFloat()
    {
        //Fire Raycast
        if (Physics.Raycast(_groundRayPos.position, Vector3.down, out _rayGroundHit, _maxRayDist, _groundLayer))
        {
            //Hit ground
            _isGrounded = true;

            //Particle
            if(!_dustParticle.isPlaying && !_dustParticlePlayed)
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

        //Check if on ground
        if (!_isGrounded)
            return;

        //Player rigidbody
        Vector3 vel = _playerRigidbody.velocity;
        Vector3 rayDir = transform.TransformDirection(Vector3.down);

        //Accounts to add down force to hitting something with a rigidbody
        Vector3 otherVel = Vector3.zero;
        Rigidbody hitBody = _rayGroundHit.rigidbody;
        if (hitBody != null)
        {
            otherVel = hitBody.velocity;
        }

        float rayDirVel = Vector3.Dot(rayDir, vel);
        float otherDirVel = Vector3.Dot(rayDir, otherVel);

        //Relative Velocity
        float relVel = rayDirVel - otherDirVel;

        //Distance
        float x = _rayGroundHit.distance - _floatHeight;

        //Calculate spring force
        float springForce = (x * _springStrength) - (relVel * _springDamper);

        //Float force
        _playerRigidbody.AddForce(rayDir * springForce);

        //Add down force on object you hit
        if (hitBody != null)
        {
            hitBody.AddForceAtPosition(rayDir * -springForce * 0.1f, _rayGroundHit.point, ForceMode.Force);
        }
    }

    private void HandleUpright()
    {
        //if (!Physics.Raycast(_groundRayPos.position, Vector3.down, out _rayHit, _maxRayDist + 1f, _groundLayer))
        //    return;

        //Physics.Raycast(_frontLeft.position, Vector3.down, out _hitFL, _maxRayDist, _groundLayer);
        //Physics.Raycast(_frontRight.position, Vector3.down, out _hitFR, _maxRayDist, _groundLayer);
        //Physics.Raycast(_backLeft.position, Vector3.down, out _hitBL, _maxRayDist, _groundLayer);
        //Physics.Raycast(_backRight.position, Vector3.down, out _hitBR, _maxRayDist, _groundLayer);

        //Debug.DrawRay(_frontLeft.position, Vector3.down * _maxRayDist);
        //Debug.DrawRay(_frontRight.position, Vector3.down* _maxRayDist);
        //Debug.DrawRay(_backLeft.position, Vector3.down * _maxRayDist);
        //Debug.DrawRay(_backRight.position, Vector3.down * _maxRayDist);

        // Get the vectors that connect the raycast hit points
        //Vector3 a = _hitBR.point - _hitBL.point;
        //Vector3 b = _hitFR.point - _hitBR.point;
        //Vector3 c = _hitFL.point - _hitFR.point;
        //Vector3 d = _hitBL.point - _hitFL.point;

        //// Get the normal at each corner
        //Vector3 crossBA = Vector3.Cross(b, a);
        //Vector3 crossCB = Vector3.Cross(c, b);
        //Vector3 crossDC = Vector3.Cross(d, c);
        //Vector3 crossAD = Vector3.Cross(a, d);

        //// Calculate composite normal
        //Vector3 upDir = (crossBA + crossCB + crossDC + crossAD).normalized;

        if(OnSlope())
        {
            //On slope
            Vector3 uprightDir = _rayGroundHit.normal;
            CalculateUpright(uprightDir);
        }
        else
        {
            //Too steep of slope
            //Set upright direction to vector3.up
            CalculateUpright(Vector3.up);
        }
    }

    private void CalculateUpright(Vector3 uprightDir)
    {
        Quaternion deltaQuat = Quaternion.FromToRotation(_playerRigidbody.transform.up, uprightDir);

        Debug.DrawRay(transform.position, uprightDir, Color.red);

        Vector3 axis;
        float angle;
        deltaQuat.ToAngleAxis(out angle, out axis);

        _playerRigidbody.AddTorque(-_playerRigidbody.angularVelocity * _uprightSpringDamper, ForceMode.Acceleration);

        _playerRigidbody.AddTorque(axis.normalized * angle * _uprightSpringStrength, ForceMode.Acceleration);
    }

    private float CalculateSlopeAngle()
    {
        float angleOfSlope = Vector3.Angle(Vector3.up, _rayGroundHit.normal);
        return angleOfSlope;
    }

    private bool OnSlope()
    {
        if(_isGrounded)
        {
            bool slopeCheck = CalculateSlopeAngle() < _maxSlopeAngle && CalculateSlopeAngle() != 0;
            return (slopeCheck);
        }
        return false;
    }

    private void SetPlatform(RaycastHit rayHit, string tag)
    {
        try
        {
            if(rayHit.transform.CompareTag(tag))
            {
                transform.SetParent(rayHit.transform);
            }
        }
        catch
        {
            transform.SetParent(null);
        }
    }

    private void PlayParticle(ParticleSystem particleSystem, Transform particlePos)
    {
        if(!particleSystem.isPlaying && _isGrounded && Time.timeScale != 0f)
        {
            Vector3 pos = _rayGroundHit.point + new Vector3(0f, 0.25f, 0f);
            particlePos.position = pos;
            particleSystem.Play();
        }
    }

    private void StopParticle(ParticleSystem particleSystem)
    {
        if(particleSystem.isPlaying)
        {
            particleSystem.Stop();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(_groundRayPos.position, Vector3.down * _maxRayDist);
    }

    private void EnableControls()
    {
        _move.Enable();
        _jump.Enable();
    }

    private void DisableControls()
    {
        _move.Disable();
        _jump.Disable();

        Invoke("EnableControls", 3f);
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
