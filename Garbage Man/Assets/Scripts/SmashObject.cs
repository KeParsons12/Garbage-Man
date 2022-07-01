using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashObject : MonoBehaviour
{
    [Header("References")]
    private Rigidbody _rb;

    [Header("Variables")]
    [SerializeField] [Tooltip("The multiplier that allows the object to be pushed. A higher value will make the object be pushed further")] private float _forceMultiplier;
    [SerializeField] [Tooltip("The tag of the object that can make contact")] private string _colTag;

    private float _forceToApply;
    private Vector3 _dir;
    private Vector3 _pos;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag(_colTag))
        {
            //Gets the direction
            _dir = transform.position - collision.transform.position;

            //Gets the position of the contact point
            _pos = collision.GetContact(0).point;

            //Calculates how much force should be applied
            _forceToApply = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;

            //Adds a force in the direction and position of the contact
            _rb.AddForceAtPosition(_forceMultiplier * _forceToApply * _dir, _pos);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, _dir * 3f);
        Gizmos.DrawSphere(_pos, 0.1f);
    }
}
