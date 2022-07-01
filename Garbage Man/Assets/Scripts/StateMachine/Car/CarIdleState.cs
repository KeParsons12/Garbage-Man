using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarIdleState : CarBaseState
{
    public CarIdleState(CarStateMachine currentContext, CarStateFactory carStateFactory) : base (currentContext, carStateFactory)
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

    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        //If input is greater than zero car is put in drive
        if(_ctx.VerticalInput > 0)
        {
            SwitchStates(_factory.Drive());
        }
        //If input is less than zero car is put into reverse
        else if(_ctx.VerticalInput < 0)
        {
            SwitchStates(_factory.Reverse());
        }

        if(_ctx.IsBreaking)
        {
            SwitchStates(_factory.Break());
        }
    }

    public override void InitializeSubState()
    {

    }
}
