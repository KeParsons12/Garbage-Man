using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    [Header("Position")]
    public float smoothSpeedPos = 10f;
    public Vector3 offsetPos;

    [Header("Rotation")]
    public float smoothSpeedRot = 10f;
    public Vector3 offsetRot;

    private void LateUpdate()
    {
        HandlePos();
        HandleRot();
    }

    private void HandlePos()
    {
        /*
        Vector3 targetPos = target.TransformPoint(offsetPos);
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeedPos * Time.deltaTime);
        */
        
        //Brackeys
        Vector3 desiredPos = target.position + offsetPos;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeedPos * Time.deltaTime);
        transform.position = smoothedPos;
        
    }

    private void HandleRot()
    {
        transform.LookAt(target.position);
    }
}
