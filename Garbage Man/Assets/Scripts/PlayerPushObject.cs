using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class PlayerPushObject : MonoBehaviour
{
    [Header("Inputs")]
    private PlayerActions _playerInputActions;
    private InputAction _push;

    [SerializeField] private Rigidbody _objectToPush;

    [SerializeField] [Tooltip("How far to push the object. Higher value = further push.")] private float _forceMultiplier;

    private void Awake()
    {
        //Inputs
        _playerInputActions = new PlayerActions();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.attachedRigidbody != null)
        {
            _objectToPush = other.gameObject.GetComponentInParent<Rigidbody>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _objectToPush = null;
    }

    private void PushObject(InputAction.CallbackContext context)
    {
        if (_objectToPush == null)
            return;

        if(_objectToPush.GetComponent<NavMeshAgent>() != null)
        {
            _objectToPush.GetComponent<NavMeshAgent>().enabled = false;
        }

        if(_objectToPush.GetComponent<AIController>() != null)
        {
            //Cancel Get up for ai
            _objectToPush.GetComponent<AIController>().CancelGetUp();
        }

        //Get rigidbody
        Rigidbody rb = _objectToPush;

        //Unfreeze object
        rb.constraints = RigidbodyConstraints.None;

        //Gets the direction
        Vector3 dir = transform.forward + transform.up;
        //Adds a force in the opposite direction of player and object
        rb.AddForce(_forceMultiplier * dir * 1000f);
    }

    private void OnEnable()
    {
        _push = _playerInputActions.PlayerControls.Push;
        _push.Enable();
        _push.performed += PushObject;
    }

    private void OnDisable()
    {
        _push.Disable();
    }
}
