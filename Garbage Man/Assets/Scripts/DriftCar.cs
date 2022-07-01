using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriftCar : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody _rb;

    [Header("Car Stats")]
    [SerializeField] [Tooltip("Center of mass for the object")] private Vector3 _centerOfMass;
    [SerializeField] [Tooltip("Acceleration of car")] private float _moveSpeed = 10f;
    [SerializeField] [Tooltip("Max speed object is able to go")] private float _maxMoveSpeed = 15f;
    [SerializeField] [Tooltip("How fast the car rotates")] private float _steeringAngle = 15f;

    [Header("Ground Detection")]
    [SerializeField] [Tooltip("Is the car grounded or not (you shouldn't need to change this value, it should auto detect)")] private bool _isGrounded;
    [SerializeField] private Transform _groundRayPoint;
    [SerializeField] [Tooltip("The distance of the raycast")] private float _raycastDist;
    [SerializeField] [Tooltip("The layer that the ray will hit and give feedback")] private LayerMask _hitLayer;


    private Vector3 _moveForce;
    private float _verticalInput;
    private float _horizontalInput;

    private void Start()
    {
        _rb.centerOfMass = _centerOfMass;
    }

    private void Update()
    {
        GetInputs();
        HandleGround(_groundRayPoint);
        HandleSteering();
    }

    private void FixedUpdate()
    {
        HandleMove();
    }

    private void GetInputs()
    {
        _verticalInput = Input.GetAxis("Vertical");
        _horizontalInput = Input.GetAxis("Horizontal");
    }

    private void HandleMove()
    {
        if(_isGrounded)
        {
            _moveForce = _rb.transform.forward * _moveSpeed * _verticalInput;
            _rb.AddForce(_moveForce, ForceMode.Acceleration);
        }

        //Max speed
        _moveForce = Vector3.ClampMagnitude(_moveForce, _maxMoveSpeed);
    }

    private void HandleSteering()
    {
        //Old
        //rb.transform.Rotate(rb.transform.up * _horizontalInput * _steeringAngle * Time.deltaTime);

        Quaternion rot = Quaternion.Euler(transform.up * _horizontalInput * _steeringAngle * Time.deltaTime);
        _rb.MoveRotation(rot * _rb.rotation);
    }

    private void HandleGround(Transform rayPoint)
    {
        //Send ray and see if car is on ground
        RaycastHit hit;

        if (Physics.Raycast(rayPoint.position, -rayPoint.up, out hit, _raycastDist, _hitLayer))
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }
    }

    private void OnDrawGizmos()
    {
        //Center of mass sphere
        Gizmos.DrawSphere(transform.position + _centerOfMass, 0.25f);

        //Ground Raycast
        Gizmos.DrawRay(_groundRayPoint.position, Vector3.down * _raycastDist);
    }
}
