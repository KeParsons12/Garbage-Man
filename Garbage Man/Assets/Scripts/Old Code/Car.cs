using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    //Car Stats
    [Header("Car Stats")]
    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteeringAngle;

    private float currentBreakForce;
    private float currentSteeringAngle;

    //Inputs
    [Header("Inputs")]
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    private float horizontalInput;
    private float verticalInput;
    private bool isBreaking;

    //Wheels
    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider FrontLeftWheelCol;
    [SerializeField] private WheelCollider FrontRightWheelCol;
    [SerializeField] private WheelCollider RearLeftWheelCol;
    [SerializeField] private WheelCollider RearRightWheelCol;

    [Header("Wheel Models")]
    [SerializeField] private Transform FrontLeftWheelTrans;
    [SerializeField] private Transform FrontRightWheelTrans;
    [SerializeField] private Transform RearLeftWheelTrans;
    [SerializeField] private Transform RearRightWheelTrans;

    private void Update()
    {
        GetInput();
        UpdateWheels();
        HandleSteering();
    }

    private void FixedUpdate()
    {
        HandleMotor();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        CalculateMotorTorque(FrontLeftWheelCol);
        CalculateMotorTorque(FrontRightWheelCol);

        currentBreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        CalculateBreakTorque(FrontLeftWheelCol);
        CalculateBreakTorque(FrontRightWheelCol);
        CalculateBreakTorque(RearLeftWheelCol);
        CalculateBreakTorque(RearRightWheelCol);
    }

    private void CalculateMotorTorque(WheelCollider wheelCollider)
    {
        wheelCollider.motorTorque = verticalInput * motorForce;
    }

    private void CalculateBreakTorque(WheelCollider wheelCollider)
    {
        wheelCollider.brakeTorque = currentBreakForce;
    }

    private void HandleSteering()
    {
        currentSteeringAngle = maxSteeringAngle * horizontalInput;
        FrontLeftWheelCol.steerAngle = currentSteeringAngle;
        FrontRightWheelCol.steerAngle = currentSteeringAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(FrontLeftWheelCol, FrontLeftWheelTrans);
        UpdateSingleWheel(FrontRightWheelCol, FrontRightWheelTrans);
        UpdateSingleWheel(RearLeftWheelCol, RearLeftWheelTrans);
        UpdateSingleWheel(RearRightWheelCol, RearRightWheelTrans);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
