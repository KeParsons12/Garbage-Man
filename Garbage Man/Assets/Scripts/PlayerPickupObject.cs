using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class PlayerPickupObject : MonoBehaviour
{
    [Header("Inputs")]
    private PlayerActions _playerInputActions;
    private InputAction _pickup;

    [Header("Stats")]
    [SerializeField] private Rigidbody _objectToPickup;
    [SerializeField] private Transform _holdArea;
    [SerializeField] private float _speed;
    private bool _isHolding;

    private void Awake()
    {
        //Inputs
        _playerInputActions = new PlayerActions();
    }

    private void FixedUpdate()
    {
        if(_isHolding && _objectToPickup != null)
        {
            Vector3 DirToHoldPoint = _holdArea.position - _objectToPickup.position;
            float DistanceToPoint = DirToHoldPoint.magnitude;

            _objectToPickup.velocity = DirToHoldPoint * _speed * DistanceToPoint;
        }

        if(_objectToPickup == null)
        {
            _isHolding = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody != null && !_isHolding)
        {
            _objectToPickup = other.gameObject.GetComponentInParent<Rigidbody>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(!_isHolding)
        {
            _objectToPickup = null;
        }
    }

    private void TryPickupObject(InputAction.CallbackContext context)
    {
        //if the object is null
        if (_objectToPickup == null)
        {
            _isHolding = false;
            return;
        }

        //If the object has a navmesh 
        if (_objectToPickup.GetComponent<NavMeshAgent>() != null)
        {
            //Disable navmesh
            _objectToPickup.GetComponent<NavMeshAgent>().enabled = false;
        }

        if(!_isHolding)
        {
            PickupObject();
        }
        else
        {
            DropObject();
        }

    }

    private void PickupObject()
    {
        //Holding object
        _isHolding = true;

        if(_objectToPickup.GetComponent<AIController>() != null)
        {
            //Tell ai he is being held
            _objectToPickup.GetComponent<AIController>()._isBeingHeld = _isHolding;

            _objectToPickup.GetComponent<AIController>().CancelGetUp();
        }
    }

    private void DropObject()
    {
        //Clear holding
        _isHolding = false;

        if(_objectToPickup.GetComponent<AIController>() != null)
        {
            //Tell ai he is not being held
            _objectToPickup.GetComponent<AIController>()._isBeingHeld = _isHolding;
        }
    }

    private void OnEnable()
    {
        _pickup = _playerInputActions.PlayerControls.Pickup;
        _pickup.Enable();
        _pickup.performed += TryPickupObject;
    }

    private void OnDisable()
    {
        _pickup.Disable();
    }
}
