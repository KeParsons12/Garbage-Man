using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody _rb;

    [Header("Forces")]
    [SerializeField] [Tooltip("The strength or how fast the object will move to return to origin. Larger value = faster return to origin")] private float _springStrength;
    [SerializeField] [Tooltip("How hard is it for the object to return to origin. Larger value = slower return speed, Lower value = easier to return.")] private float _springDampening;
    [Tooltip("The axis over which the oscillator applies torque. Within range [0, 1].")]
    [SerializeField] private Vector3 _forceScale = Vector3.one;
    [Tooltip("The starting location or origin of this object.")] private Vector3 _origin;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _origin = transform.position;
    }

    private void FixedUpdate()
    {
        Vector3 x = transform.position - _origin;
        Vector3 force = HookesLaw(x, _rb.velocity);
        _rb.AddForce(Vector3.Scale(force, _forceScale));
    }

    private Vector3 HookesLaw(Vector3 displacement, Vector3 velocity)
    {
        Vector3 springForce = (_springStrength * displacement) + (_springDampening * velocity);
        springForce = -springForce;
        return(springForce);
    }
}
