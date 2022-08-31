using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncy : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody _rb;

    [Header("Forces")]
    [SerializeField] private float _bounceForce;

    private void OnTriggerEnter(Collider other)
    {
        HandleBounce(other);
    }

    private void HandleBounce(Collider col)
    {
        if(col.gameObject.GetComponent<Rigidbody>() != null)
        {
            Rigidbody colRb = col.gameObject.GetComponent<Rigidbody>();
            float impactForce = colRb.velocity.magnitude;
            Vector3 bounceDir = _bounceForce * impactForce * Vector3.up;

            colRb.AddForce(bounceDir);
        }
    }
}
