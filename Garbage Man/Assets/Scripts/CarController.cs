using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Code from one minute games. 
/// </summary>
public class CarController : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody rb;

    [Header("Car Stats")]
    public float moveSpeed = 5f;
    public float maxSpeed = 20f;
    public float steerAngle = 20f;
    public float drag = 0.98f;
    public float traction = 1f;

    [Header("Wheels")]
    public bool isGrounded;
    public int wheelsOnGround;
    public List<WheelCollider> allWheels;

    [Header("Debug")]
    public bool isDebug;

    private Vector3 moveForce;
    private float horizontalInput;
    private float verticalInput;

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        //If we are not on the ground
        if(IsGround())
        {
            HandleMove();
            HandleSteering();
        }

        CalculateDrag();
        CalculateTraction();
    }

    private void HandleMove()
    {
        //Vector of direction to move over time
        moveForce += transform.forward * moveSpeed * verticalInput * Time.deltaTime;
        //MaxSpeed
        moveForce = Vector3.ClampMagnitude(moveForce, maxSpeed);
        //Moves position of car by vector moveForce
        //rb.position += moveForce * Time.deltaTime;
        rb.AddForce(moveForce);
    }

    private void HandleSteering()
    {
        //Steering
        transform.Rotate(Vector3.up * horizontalInput * moveForce.magnitude * steerAngle * Time.deltaTime);
    }

    private void CalculateDrag()
    {
        //Calculate Drag
        moveForce *= drag;
    }

    private void CalculateTraction()
    {
        //Traction
        //Keeps us from sliding around like on ice
        if(isDebug)
        {
            Debug.DrawRay(rb.position, moveForce.normalized * 10);
            Debug.DrawRay(rb.position, transform.forward * 10, Color.blue);
        }

        moveForce = Vector3.Lerp(moveForce.normalized, transform.forward, traction * Time.deltaTime) * moveForce.magnitude;
    }

    private bool IsGround()
    {
        wheelsOnGround = 0;
        foreach (WheelCollider wheel in allWheels)
        {
            if (wheel.isGrounded)
            {
                wheelsOnGround++;
            }
        }
        if (wheelsOnGround == 4)
        {
            isGrounded = true;
            return isGrounded;
        }
        else
        {
            isGrounded = false;
            return isGrounded;
        }
    }

}
