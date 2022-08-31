using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashPickUp : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] [Tooltip("The position the object will be placed at")] private Transform _pickupPos;
    private GameObject _pickUpObject;
    private Collider _colObject;
    private PlayerActions _inputActions;

    [Header("Variables")]
    [SerializeField] [Tooltip("The tag for being able to pickup an object.")] private string _pickupTag;
    [SerializeField] [Tooltip("The higher the number the farther the object will be thrown")] private float _throwForce;
    [SerializeField] [Tooltip("This determines how high the object will arc in the air. Lower number = lower throw")] private float _heightOffset;
    private bool _interactInput;

    private void Awake()
    {
        //Set player inputs
        _inputActions = new PlayerActions();
    }

    private void Update()
    {
        TryPickUpTrash();
    }

    private void TryPickUpTrash()
    {
        //Read the button value
        _interactInput = _inputActions.PlayerControls.Jump.WasPressedThisFrame();

        if(_interactInput)
        {
            if(_pickUpObject == null)
            {
                PickUpObject();
            }
            else
            {
                ThrowObject();
            }
        }

        if(_pickUpObject != null)
        {
            MoveObject();
        }
    }

    private void PickUpObject()
    {
        if(_colObject == null) { return; }

        //set the pick up object
        _pickUpObject = _colObject.gameObject;

        PickupStats pickupStats = _pickUpObject.GetComponent<PickupStats>();

        //The pickup object has already been collected
        if(pickupStats.HasTrash)
        {
            //Add points for picking up object
            ScoreManager.instance.AddPoint(pickupStats.PointValue);

            //Empty Trash
            pickupStats.HasTrash = false;
        }
    }

    private void ThrowObject()
    {
        //The direction then force
        Vector3 dir = _pickUpObject.transform.position - transform.position;
        Vector3 force = new Vector3(dir.x,dir.y * _heightOffset, dir.z);

        //get rigidbody
        Rigidbody rb = _pickUpObject.GetComponent<Rigidbody>();

        //Throw the object
        rb.AddForce(force * _throwForce, ForceMode.Impulse);

        //random rotation
        

        //Use gravity 
        rb.useGravity = true;

        //unfreeze rotation
        rb.constraints = RigidbodyConstraints.None;

        //Reset pick up object
        _pickUpObject = null;
    }

    private void MoveObject()
    {
        //get rigidbody
        Rigidbody rb = _pickUpObject.GetComponent<Rigidbody>();

        //Turn off gravity
        rb.useGravity = false;

        //Set the trash position to the pickupPos
        //rb.MovePosition(_pickupPos.position);
        _pickUpObject.transform.position = _pickupPos.position;

        //set rotation
        _pickUpObject.transform.rotation = _pickupPos.rotation;

        //Freeze Rotation
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(_pickupTag))
        {
            _colObject = other;   
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag(_pickupTag))
        {
            _colObject = null;
        }
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }
}
