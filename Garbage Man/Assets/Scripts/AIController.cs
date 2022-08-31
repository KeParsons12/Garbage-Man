using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] [Tooltip("Parent object that holds on the waypoints for the AI to move between")] private GameObject _waypointHolder;
    [SerializeField] [Tooltip("The goal locations the object/AI can move to")] private Transform[] _goalLocations;
    [Tooltip("AI nav mesh on the object or AI")] private NavMeshAgent _agent;

    [Header("Stats")]
    [SerializeField] [Tooltip("How fast the AI moves.")] private float _moveSpeed;
    [SerializeField] [Tooltip("How fast the AI moves.")] private float _maxMoveSpeed;
    [SerializeField] [Tooltip("This is how close the AI needs to get to the goal marker to reach it.")] private float _distanceToGoal;
    private int _randomLocation;
    [Tooltip("The location the AI will try to move towards")] private Vector3 _currentGoalPos;
    [SerializeField] private float _wakeUpTime;

    [SerializeField] private bool _goalReached;
    [SerializeField] private bool _hasPickedNewGoal;

    [Header("Ground")]
    [SerializeField] private float _rayDist;
    [SerializeField] private LayerMask _groundLayer;

    //public
    public bool _isBeingHeld;
    public bool _isUp;

    private void Awake()
    {
        //Get components
        _agent = GetComponent<NavMeshAgent>();

        //Sets the size of the array
        _goalLocations = new Transform[_waypointHolder.transform.childCount];
        //Loops through the waypoints 
        for (int i = 0; i < _waypointHolder.transform.childCount; i++)
        {
            //Sets the children to the correct waypoint
            _goalLocations[i] = _waypointHolder.transform.GetChild(i).GetComponent<Transform>();
        }

        //Set speed to default move speed
        _agent.speed = _moveSpeed;

        //Set random goal pos
        //_randomLocation = Random.Range(0, _goalLocations.Length);
        //_currentGoalPos = _goalLocations[_randomLocation].position;
        PickNewRandomGoal();

        //Is being held false
        _isBeingHeld = false;
    }

    private void Update()
    {
        if(_agent.enabled)
        {
            GoalReached();
        }

        if(_isBeingHeld || !_isUp)
        {
            _agent.enabled = false;
        }

        if (!_goalReached)
        {
            _hasPickedNewGoal = false;
        }

        //Agent is disabled
        if (!_agent.enabled && !_isBeingHeld && IsGrounded())
        {
            Invoke("GetUp", _wakeUpTime);
        }
    }

    private void GoalReached()
    {
        if(_distanceToGoal >= _agent.remainingDistance)
        {
            _goalReached = true;

            PickNewRandomGoal();
        }
        else
        {
            _goalReached = false;
        }
    }

    private void PickNewRandomGoal()
    {
        if(!_hasPickedNewGoal)
        {
            _randomLocation = Random.Range(0, _goalLocations.Length);

            _currentGoalPos = _goalLocations[_randomLocation].position;

            _agent.SetDestination(_currentGoalPos);

            _hasPickedNewGoal = true;
        }
    }

    public void CancelGetUp()
    {
        CancelInvoke("GetUp");

        _isUp = false;
    }

    private void GetUp()
    {
        if (!_isUp)
        {
            _agent.enabled = true;

            _isUp = true;
        }
    }

    private bool IsGrounded()
    {
        if(Physics.Raycast(transform.position, Vector3.down, _rayDist, _groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
