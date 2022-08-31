using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody _rb;

    [Header("Moving Platform")]
    [SerializeField] private GameObject[] _waypoints;
    private int _currentWaypointIndex = 0;

    [SerializeField] [Tooltip("How fast the platform moves.")] private float _moveSpeed;
    [SerializeField] [Tooltip("Determines how long the platform will wait until moving to next waypoint.")] private float _waitTime;
    [SerializeField] [Tooltip("Determines how close the platform needs to get to the waypoint position.")] private float _waypointReachedRadius; //How close the platform needs to get to the waypoint

    private float _currentWaitTime;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //Calculate remaining distance to waypoint
        float remainingDistance = Vector3.Distance(transform.position, _waypoints[_currentWaypointIndex].transform.position);

        //If the platform reaches the destination increase index
        if(remainingDistance <= _waypointReachedRadius)
        {
            //Increase index by 1
            _currentWaypointIndex++;

            //If index is larger then waypoints length the reset to 0
            if(_currentWaypointIndex >= _waypoints.Length)
            {
                _currentWaypointIndex = 0;
            }

            //Reset wait time
            _currentWaitTime = Time.time + _waitTime;
        }

        if(Time.time >= _currentWaitTime)
        {
            //Move platform toward current waypoint index 
            transform.position = Vector3.MoveTowards(_rb.transform.position, _waypoints[_currentWaypointIndex].transform.position, _moveSpeed * Time.deltaTime);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            other.gameObject.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            other.gameObject.transform.SetParent(null);
        }
    }
}
