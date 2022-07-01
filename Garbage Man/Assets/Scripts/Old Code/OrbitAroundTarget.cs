using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitAroundTarget : MonoBehaviour
{
    public Transform targetPos;
    public float orbitSpeed;

    private void Update()
    {
        transform.RotateAround(targetPos.position, Vector3.up ,orbitSpeed * Time.deltaTime);
    }
}
