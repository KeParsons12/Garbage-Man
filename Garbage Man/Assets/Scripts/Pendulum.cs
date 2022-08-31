using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody _rb;

    [Header("Forces")]
    [SerializeField] [Tooltip("The strength or how fast the object will move to return to origin. Larger value = faster return to origin")] private float _springStrength;
    [SerializeField] [Tooltip("How hard is it for the object to return to origin. Larger value = slower return speed, Lower value = easier to return.")] private float _springDampening;
    [Tooltip("The axes over which the oscillator applies torque. Within range [0, 1].")]
    [SerializeField] private Vector3 _forceScale = Vector3.one;

    [Header("Pivot")]
    [SerializeField] [Tooltip("The local rotation about which oscillations are centered.")] private Vector3 _localEquilibriumRotation = Vector3.zero;
    [SerializeField] [Tooltip("The center about which rotations should occur.")] private Vector3 _localPivotPosition = Vector3.zero;

    [Header("Open / Close")]
    [SerializeField] [Tooltip("The rotation the object will be when open.")] private Vector3 _openedPosition;
    [SerializeField] [Tooltip("The rotation the object will be when closed.")] private Vector3 _closedPosition;
    private float _angularDisplacementMagnitude;
    private Vector3 _rotAxis;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _rb.centerOfMass = _localPivotPosition;
    }

    private void FixedUpdate()
    {
        Quaternion deltaRotation = ShortestRotation(transform.localRotation, Quaternion.Euler(_localEquilibriumRotation));
        deltaRotation.ToAngleAxis(out _angularDisplacementMagnitude, out _rotAxis);
        Vector3 angularDisplacement = _angularDisplacementMagnitude * Mathf.Deg2Rad * _rotAxis.normalized;
        Vector3 torque = AngularHookesLaw(angularDisplacement, _rb.angularVelocity);
        _rb.AddTorque(Vector3.Scale(torque, _forceScale));

        _rb.centerOfMass = _localPivotPosition;
    }

    private Vector3 AngularHookesLaw(Vector3 angularDisplacement, Vector3 angularVelocity)
    {
        Vector3 torque = (_springStrength * angularDisplacement) + (_springDampening * angularVelocity);
        torque = -torque;
        return (torque);
    }

    private static Quaternion ShortestRotation(Quaternion a, Quaternion b)
    {
        if (Quaternion.Dot(a, b) < 0)
        {
            return a * Quaternion.Inverse(Multiply(b, -1));
        }

        else return a * Quaternion.Inverse(b);
    }

    private static Quaternion Multiply(Quaternion input, float scalar)
    {
        return new Quaternion(input.x * scalar, input.y * scalar, input.z * scalar, input.w * scalar);
    }

    public void ChangeRotation(bool openDoor)
    {
        if(openDoor)
        {
            _localEquilibriumRotation = _openedPosition;
        }
        else
        {
            _localEquilibriumRotation = _closedPosition;
        }
    }

    private void OnDrawGizmos()
    {
        //Draw center of mass
        //Draw the local pivot
        //Center Of Mass = Local Pivot
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_localPivotPosition + transform.position, 0.5f);
    }
}
