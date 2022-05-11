using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashProjectile : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;

    private void Awake()
    {
        Shoot(speed);
    }

    private void Shoot(float _speed)
    { 
        rb.AddForce(transform.up * _speed * 100f, ForceMode.Impulse);
    }
}
