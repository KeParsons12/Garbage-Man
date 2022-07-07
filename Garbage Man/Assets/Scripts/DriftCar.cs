using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class DriftCar : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody _rb;
    private PlayerActions _inputActions;

    [Header("Car Stats")]
    [SerializeField] [Tooltip("Center of mass for the object")] private Vector3 _centerOfMass;
    [SerializeField] [Tooltip("Acceleration of car")] private float _moveSpeed = 10f;
    [SerializeField] [Tooltip("Max speed object is able to go")] private float _maxMoveSpeed = 15f;
    [SerializeField] [Tooltip("How fast the car rotates")] private float _steeringAngle = 15f;

    [Header("Wheels")]
    [SerializeField] private Transform _frontLeftWheel;
    [SerializeField] private Transform _frontRightWheel;
    [SerializeField] private Transform _rearLeftWheel;
    [SerializeField] private Transform _rearRightWheel;

    [Header("Ground Detection")]
    [SerializeField] [Tooltip("Is the car grounded or not (you shouldn't need to change this value, it should auto detect)")] private bool _isGrounded;
    [SerializeField] [Tooltip("The position that the ground box cast will be centered at")] private Transform _groundRayPoint;
    [SerializeField] [Tooltip("The size of the box cast")] private Vector3 _boxCastSize;
    [SerializeField] [Tooltip("The rotation of the box cast")] private Vector3 _boxCastRot;
    [SerializeField] [Tooltip("The distance of the raycast")] private float _raycastDist;
    [SerializeField] [Tooltip("The layer that the ray will hit and give feedback")] private LayerMask _hitLayer;



    private void Awake()
    {
        //Set rigidbody
        _rb = GetComponent<Rigidbody>();

        //Set player inputs
        _inputActions = new PlayerActions();

        //Set center of mass
        _rb.centerOfMass = _centerOfMass;
    }

    private void Update()
    {
        HandleGround(_groundRayPoint);
        HandleSteering();
        UpdateWheels();
    }

    private void FixedUpdate()
    {
        HandleMove();
    }

    public void HandleMove()
    {
        float verticalInput = _inputActions.CarControls.Move.ReadValue<float>();
        Vector3 moveForce = _rb.transform.forward * _moveSpeed * verticalInput;

        if(_isGrounded)
        {
            _rb.AddForce(moveForce, ForceMode.Acceleration);
        }

        //Max speed
        moveForce = Vector3.ClampMagnitude(moveForce, _maxMoveSpeed);
    }

    private void HandleSteering()
    {
        float horizontalInput = _inputActions.CarControls.Rotate.ReadValue<float>();

        Quaternion rot = Quaternion.Euler(transform.up * horizontalInput * _steeringAngle * Time.deltaTime);
        _rb.MoveRotation(rot * _rb.rotation);
    }

    private void HandleGround(Transform rayPoint)
    {
        RaycastHit hit;

        if (Physics.BoxCast(_groundRayPoint.position, _boxCastSize * 0.5f, Vector3.down, out hit, Quaternion.Euler(_boxCastRot), _raycastDist, _hitLayer))
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(_frontLeftWheel);
        UpdateSingleWheel(_frontRightWheel);
        UpdateSingleWheel(_rearLeftWheel);
        UpdateSingleWheel(_rearRightWheel);
    }

    private void UpdateSingleWheel(Transform wheel)
    {
        float verticalInput = _inputActions.CarControls.Move.ReadValue<float>();
        float rpm = 1f * _rb.velocity.magnitude;

        wheel.RotateAround(wheel.GetComponent<Renderer>().bounds.center, wheel.right, rpm);
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }



    /* //For debugging
    private void OnDrawGizmos()
    {
        //Center of mass sphere
        Gizmos.DrawSphere(transform.position + _centerOfMass, 0.25f);

        //Draw ground ray
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_groundRayPoint.position, _boxCastSize);
    }
    */
}
