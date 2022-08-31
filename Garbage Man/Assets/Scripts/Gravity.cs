using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] [Tooltip("Negative number forces downward, Positive number forces upward.")] private float _gravity;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rb.AddForce(Vector3.up * _gravity, ForceMode.Force);
    }
}
