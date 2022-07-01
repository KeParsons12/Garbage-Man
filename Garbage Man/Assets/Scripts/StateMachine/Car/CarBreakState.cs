using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBreakState : CarBaseState
{
    public CarBreakState(CarStateMachine currentContext, CarStateFactory carStateFactory) : base (currentContext, carStateFactory)
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
        //Apply breaking to car
        ApplyBreaking(_ctx.BreakForce);
    }

    public override void ExitState()
    {
        ApplyBreaking(0f);
    }

    public override void CheckSwitchStates()
    {
        //If the car is not moving go to idle
        if (_ctx.Rigidbody.velocity.magnitude <= 0.5f)
        {
            SwitchStates(_factory.Idle());
        }

        //Input forward
        if(_ctx.VerticalInput > 0)
        {
            SwitchStates(_factory.Drive());
        }
    }

    public override void InitializeSubState()
    {

    }

    private void ApplyBreaking(float force)
    {
        CalculateBreakTorque(_ctx.FrontLeftWheelCol, force);
        CalculateBreakTorque(_ctx.FrontRightWheelCol, force);
        CalculateBreakTorque(_ctx.RearLeftWheelCol, force);
        CalculateBreakTorque(_ctx.RearRightWheelCol, force);
    }

    private void CalculateBreakTorque(WheelCollider wheelCollider, float force)
    {
        wheelCollider.brakeTorque = force;
    }
}
