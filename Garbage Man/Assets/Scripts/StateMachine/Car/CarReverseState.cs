using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarReverseState : CarBaseState
{
    public CarReverseState(CarStateMachine currentContext, CarStateFactory carStateFactory) : base (currentContext, carStateFactory)
    {

    }

    public override void EnterState()
    {

    }

    public override void UpdateState()
    {
        //Checking to switch state each frame
        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {
        HandleMotor(_ctx.MotorForce, _ctx.DrivingType);
    }

    public override void ExitState()
    {
        HandleMotor(0f, _ctx.DrivingType);
    }

    public override void CheckSwitchStates()
    {
        //If the car is not moving go to idle
        if (_ctx.Rigidbody.velocity.magnitude == 0)
        {
            SwitchStates(_factory.Idle());
        }

        //If vertical input is greater than zero then player wants to break
        if (_ctx.IsBreaking)
        {
            SwitchStates(_factory.Break());
        }

        //If motor torque is positive go to drive
        if(_ctx.FrontLeftWheelCol.motorTorque > 0)
        {
            SwitchStates(_factory.Drive());
        }
    }

    public override void InitializeSubState()
    {

    }

    private void HandleMotor(float force, WheelDriveType wheelDrive)
    {
        switch (wheelDrive)
        {
            case WheelDriveType.frontWheelDrive:
                CalculateMotorTorque(_ctx.FrontLeftWheelCol, force);
                CalculateMotorTorque(_ctx.FrontRightWheelCol, force);
                break;

            case WheelDriveType.rearWheelDrive:
                CalculateMotorTorque(_ctx.RearLeftWheelCol, force);
                CalculateMotorTorque(_ctx.RearRightWheelCol, force);
                break;

            case WheelDriveType.allWheelDrive:
                CalculateMotorTorque(_ctx.FrontLeftWheelCol, force);
                CalculateMotorTorque(_ctx.FrontRightWheelCol, force);
                CalculateMotorTorque(_ctx.RearLeftWheelCol, force);
                CalculateMotorTorque(_ctx.RearRightWheelCol, force);
                break;

            default:
                CalculateMotorTorque(_ctx.FrontLeftWheelCol, force);
                CalculateMotorTorque(_ctx.FrontRightWheelCol, force);
                break;
        }
    }

    private void CalculateMotorTorque(WheelCollider wheelCollider, float force)
    {
        wheelCollider.motorTorque = _ctx.VerticalInput * force;
    }
}
