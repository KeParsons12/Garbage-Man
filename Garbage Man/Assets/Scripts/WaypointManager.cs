using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WaypointManager : MonoBehaviour
{
    [SerializeField] private LineRenderer _wireLine;
    [SerializeField] private GameObject _wirePrefab;

    [SerializeField] private int _numOfPositions;

    [SerializeField] private bool _connectFirstAndLast;

    public void AddWirePosition()
    {
        //if child count is zero instantiate at transform parent
        if (transform.childCount == 0)
        {
            Instantiate(_wirePrefab, transform.position, transform.rotation, transform);
        }
        //else instantiante at last created child transform
        else
        {
            //Instantiates a new wire position object at the transform of this object
            Instantiate(_wirePrefab, transform.GetChild(_numOfPositions - 1).transform.position, transform.GetChild(_numOfPositions - 1).transform.rotation, transform);
        }


        //Increase number of positions
        _numOfPositions++;
    }

    public void RemoveWirePosition()
    {
        //Remove object from scene
        var objectToDestroy = transform.GetChild(_numOfPositions - 1);
        DestroyImmediate(objectToDestroy.gameObject);

        //Decrease number of positions
        _numOfPositions--;
    }

    private void OnDrawGizmos()
    {
        foreach (Transform t in transform)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(t.position, 0.25f);
        }

        Gizmos.color = Color.green;
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
        }

        if(_connectFirstAndLast)
        {
            //Draw the first and last waypoint line
            Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position, transform.GetChild(0).position);
        }
    }
}
