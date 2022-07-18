using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] [Tooltip("This is how close the AI needs to get to the goal marker to reach it.")] private float distanceToGoal;
    [Tooltip("The location the AI will try to move towards")] private Vector3 _currentGoalPos;

    private void Awake()
    {
        //Get components
        _agent = GetComponent<NavMeshAgent>();

        //Set AI Waypoints
        //_goalLocations = GameObject.FindGameObjectsWithTag("Waypoint");
        //_goalLocations = _waypointHolder.GetComponentsInChildren<Transform>();

        //Sets the size of the array
        _goalLocations = new Transform[_waypointHolder.transform.childCount];
        //Loops through the waypoints 
        for (int i = 0; i < _waypointHolder.transform.childCount; i++)
        {
            //Sets the children to the correct waypoint
            _goalLocations[i] = _waypointHolder.transform.GetChild(i).GetComponent<Transform>();
        }

        //Set speed
        _agent.speed = _moveSpeed;

        //Set random goal pos
        _currentGoalPos = _goalLocations[Random.Range(0, _goalLocations.Length)].position;
    }

    private void Update()
    {
        MoveToGoal();
        ReachedGoal();
    }

    private void ReachedGoal()
    {
        //If AI is within certain distance then stop moving
        if(_agent.remainingDistance < distanceToGoal)
        {
            //Stop AI
            _agent.isStopped = true;

            //Pick a new goal after sometime
            Invoke("PickNewGoal", Random.Range(0.25f, 5f));
        }
    }

    private void PickNewGoal()
    {
        //Choose new goal
        _currentGoalPos = _goalLocations[Random.Range(0, _goalLocations.Length)].position;

        //Temp REMOVE LATER
        _agent.speed = Random.Range(_moveSpeed, _maxMoveSpeed);

        //Reset isStopped
        _agent.isStopped = false;
        
    }

    private void MoveToGoal()
    {
        //Move to the chosen goal
        if(!_agent.isStopped)
        {
            //move towards goal
            _agent.SetDestination(_currentGoalPos);
        }
    }

    private void StopMoving()
    {
        //Keep goal just stop moving towards it
        _agent.isStopped = true;
    }

    private void FleeFromObject()
    {
        //Flee from object/area pick new goal
    }

}
