public class CarStateFactory
{
    CarStateMachine context;

    public CarStateFactory(CarStateMachine currentContext)
    {
        context = currentContext;
    }

    public CarBaseState Idle()
    {
        return new CarIdleState(context, this);
    }

    public CarBaseState Drive()
    {
        return new CarDriveState(context, this);
    }

    public CarBaseState Reverse()
    {
        return new CarReverseState(context, this);
    }

    public CarBaseState Break()
    {
        return new CarBreakState(context, this);
    }
}
