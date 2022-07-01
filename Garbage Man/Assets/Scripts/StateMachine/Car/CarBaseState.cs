using UnityEngine;

public abstract class CarBaseState
{
    protected CarStateMachine _ctx;
    protected CarStateFactory _factory;
    public CarBaseState(CarStateMachine currentContext, CarStateFactory carStateFactory)
    {
        _ctx = currentContext;
        _factory = carStateFactory;
    }

    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void FixedUpdateState();

    public abstract void ExitState();

    public abstract void CheckSwitchStates();

    public abstract void InitializeSubState();

    protected void SwitchStates(CarBaseState newState)
    {
        //Set last state
        _ctx.LastState = _ctx.CurrentState;

        //Current states exits state
        ExitState();

        //new state enter state
        newState.EnterState();

        //Switch current state of context
        _ctx.CurrentState = newState;
    }
}
