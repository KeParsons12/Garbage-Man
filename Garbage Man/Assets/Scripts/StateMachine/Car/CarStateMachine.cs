using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WheelDriveType
{
    frontWheelDrive,
    rearWheelDrive,
    allWheelDrive
};

public class CarStateMachine : MonoBehaviour
{

    //States variables
    [Header("States")]
    private CarBaseState _currentState;
    private CarBaseState _lastState;
    private CarStateFactory _states;

    //Car Stats
    [Header("Car Stats")]
    [SerializeField] [Tooltip("What wheels will be the driving force of the car.")] private WheelDriveType _drivingType;
    [SerializeField] [Tooltip("Force applied when driving. More force = faster driving")] private float _motorForce;
    [SerializeField] [Tooltip("Force applied when breaking. More force = faster breaking")] private float _breakForce;
    [SerializeField] [Tooltip("The max angle the wheels can turn")] private float _maxSteeringAngle;
    [SerializeField] [Tooltip("Set the model wheels offset Z rotation")] private Quaternion _offset;
    private float _currentSteeringAngle;

    [Header("Car Physics")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] [Tooltip("Sets the center of mass for the car")] private Vector3 _centerOfMass;

    //Wheels
    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider _frontLeftWheelCol;
    [SerializeField] private WheelCollider _frontRightWheelCol;
    [SerializeField] private WheelCollider _rearLeftWheelCol;
    [SerializeField] private WheelCollider _rearRightWheelCol;

    [Header("Wheel Models")]
    [SerializeField] private Transform _frontLeftWheelTrans;
    [SerializeField] private Transform _frontRightWheelTrans;
    [SerializeField] private Transform _rearLeftWheelTrans;
    [SerializeField] private Transform _rearRightWheelTrans;

    //Inputs
    [Header("Inputs")]
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    private float _horizontalInput;
    private float _verticalInput;
    private bool _isBreaking;

    //Getter and setter
    public CarBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public CarBaseState LastState { get { return _lastState ; } set { _lastState = value; } }
    public CarStateFactory States { get { return _states; } } 

    public Rigidbody Rigidbody { get { return _rb; } }

    public WheelCollider FrontLeftWheelCol { get { return _frontLeftWheelCol; } }
    public WheelCollider FrontRightWheelCol { get { return _frontRightWheelCol; } }
    public WheelCollider RearLeftWheelCol { get { return _rearLeftWheelCol; } }
    public WheelCollider RearRightWheelCol { get { return _rearRightWheelCol; } }

    public float HorizontalInput { get { return _horizontalInput; } }
    public float VerticalInput { get { return _verticalInput; } }
    public bool IsBreaking { get { return _isBreaking; } }

    public WheelDriveType DrivingType { get { return _drivingType; } }

    public float MotorForce { get { return _motorForce; } }
    public float BreakForce { get { return _breakForce; } }

    private void Awake()
    {
        //set states
        _states = new CarStateFactory(this);
        _currentState = _states.Idle();
        _lastState = _states.Idle();
        _currentState.EnterState();

        //Set center of mass
        _rb.centerOfMass = _centerOfMass;
    }

    private void Update()
    {
        GetInput();
        UpdateWheels();
        HandleSteering();

        _currentState.UpdateState();
    }

    private void FixedUpdate()
    {
        _currentState.FixedUpdateState();
    }

    private void GetInput()
    {
        _horizontalInput = Input.GetAxis(HORIZONTAL);
        _verticalInput = Input.GetAxis(VERTICAL);
        _isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleSteering()
    {
        //Turns wheels
        _currentSteeringAngle = _maxSteeringAngle * _horizontalInput;
        FrontLeftWheelCol.steerAngle = _currentSteeringAngle;
        FrontRightWheelCol.steerAngle = _currentSteeringAngle;
    }

    //Animation
    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos; //position of wheel collider
        Quaternion rot; //rotation of wheel collider
        wheelCollider.GetWorldPose(out pos, out rot); //set pos and rot of wheel collider
        wheelTransform.rotation = rot * _offset; //set the wheel Model rotation
        wheelTransform.position = pos; //set the wheel Model Position
    }

    private void UpdateWheels()
    {
        //Update each wheel
        UpdateSingleWheel(FrontLeftWheelCol, _frontLeftWheelTrans);
        UpdateSingleWheel(FrontRightWheelCol, _frontRightWheelTrans);
        UpdateSingleWheel(RearLeftWheelCol, _rearLeftWheelTrans);
        UpdateSingleWheel(RearRightWheelCol, _rearRightWheelTrans);
    }


    //Draw gizmos
    private void OnDrawGizmos()
    {
        // Draw a yellow sphere at the Center of mass position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + _centerOfMass, 0.25f);
    }
}
