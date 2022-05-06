using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RealisticCarController : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody rb;

    [Header("Player Inputs")]
    public float inputHorizontal;
    public float inputVertical;

    [Header("Car Stats")]
    public float maxAcceleration;
    public float breakForce;
    public float turnSensitivity;
    public float maxSteeringAngle;
    public Vector3 centerOfMass;

    private float _multiplier = 1000f; //This allows so the values in the inspector dont need to be so large

    public enum Axel
    {
        Front,
        Rear
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject model;
        public WheelCollider collider;
        public Axel axel;
    }

    public List<Wheel> wheels;

    private void Start()
    {
        _multiplier = 1000f;

        rb.centerOfMass = centerOfMass;
    }

    private void Update()
    {
        GetInputs();
        AnimateWheels();
    }

    private void FixedUpdate()
    {
        Move();
        Turn();
    }

    public void Move()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Rear)
            {
                //Input Forward (positive)
                if(inputVertical > 0)
                {
                    //only move with rear wheels
                    wheel.collider.motorTorque = inputVertical * maxAcceleration * _multiplier * Time.deltaTime;   
                }
            }

            //Input Reverse (negative)
            if (inputVertical < 0)
            {
                //break with all wheels
                wheel.collider.brakeTorque = inputVertical * breakForce * _multiplier * Time.deltaTime;
            }
        }
    }

    public void Turn()
    {
        foreach (var wheel in wheels)
        {
            if(wheel.axel == Axel.Front)
            {
                var _steerAngle = inputHorizontal * turnSensitivity * maxSteeringAngle;
                wheel.collider.steerAngle = Mathf.Lerp(wheel.collider.steerAngle, _steerAngle, 0.5f);
            }
        }
    }

    public void AnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;

            wheel.collider.GetWorldPose(out pos, out rot);
            wheel.model.transform.position = pos;
            wheel.model.transform.rotation = rot * Quaternion.Euler(0f,0f, 90f);
        }
    }

    public void GetInputs()
    {
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");
    }
}
