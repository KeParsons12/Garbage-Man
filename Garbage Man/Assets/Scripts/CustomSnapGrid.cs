using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CustomSnapGrid : MonoBehaviour
{
    [SerializeField] [Tooltip("The size of the grid to snap objects to.")] private Vector3 gridSize = new Vector3(1,1,1);

    private void OnDrawGizmos()
    {
        SnapToGrid();    
    }

    private void SnapToGrid()
    {
        var pos = new Vector3(
            Mathf.Round(this.transform.position.x / this.gridSize.x) * this.gridSize.x,
            Mathf.Round(this.transform.position.y / this.gridSize.y) * this.gridSize.y,
            Mathf.Round(this.transform.position.z / this.gridSize.z) * this.gridSize.z
            );

        this.transform.position = pos;
    }
}
