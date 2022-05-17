using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A sphere is actually driving the car. The car is just a model
/// </summary>
public class ArcadeCarController : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody rb;

    [Header("Car Model")]
    public Transform rightFrontWheel;
    public Transform leftFrontWheel;
    public float maxWheelTurn = 25f;
    public Vector3 difference;

    [Header("Car Stats")]
    public float accelSpeed;
    public float forwardAccelSpeed = 10f;
    public float reverseAccelSpeed = 5f;
    public float turnStrength = 180f;
    private Vector3 moveForce;

    [Header("Forces On Car")]
    public float gravityForce = 10f;
    public float dragOnGround = 3f;
    public float dragOffGround = 0.1f;

    [Header("Ground Stats")]
    public LayerMask layerGround;
    public Transform[] groundRayPoint;
    public float snapToGroundLevel = 0.1f;
    public float groundRayLength = 0.5f;
    public bool isGrounded;
    private Vector3 normalAccumulator;

    [Header("VFXs")]
    public ParticleSystem[] exhaustParticles;
    public TrailRenderer[] skidMarks;

    [Header("SFXs")]
    public AudioSource startSound;
    public AudioSource runningSound;

    [Header("Inputs")]
    private float speedInput;
    private float turnInput;

    private void Start()
    {
        //Seperate the sphere and the car model
        rb.transform.parent = null;
    }

    private void Update()
    {
        //Player Inputs
        HandleInputs();

        if (IsGround())
        {
            //Steering Car
            HandleTurn();
        }

        //Makes the model follow the sphere
        transform.position = rb.transform.position + difference;
    }

    private void FixedUpdate()
    {
        if(IsGround())
        {
            //Moves sphere
            HandleMove();
        }
        else
        {
            //Apply gravity when car is in air
            ApplyGravity();
        }
    }

    public void HandleMove()
    {
        //if trying to move
        if(Mathf.Abs(accelSpeed) > 0)
        {
            //Adds a force in the forward vector of the car model * accelSpeed
            moveForce = transform.forward * accelSpeed;
            //moveForce = Vector3.ClampMagnitude(moveForce, maxSpeed * 1000f);
            rb.AddForce(moveForce);

            //Audio
            runningSound.pitch = Mathf.Clamp01(accelSpeed);
        }
    }

    public void HandleTurn()
    {
        //Rotates the car model
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime, 0f));
       
        rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, rightFrontWheel.localRotation.eulerAngles.z);
        leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, leftFrontWheel.localRotation.eulerAngles.z);
    }

    public void HandleInputs()
    {
        //Moving Forward / Reverse
        speedInput = Input.GetAxis("Vertical");
        //Input Forward
        if (speedInput > 0)
        {
            accelSpeed = speedInput * forwardAccelSpeed * 100f;
            //Exhaust
            HandleExhaustParticle(false);
        }
        //Input Reverse
        else if(speedInput < 0)
        {
            accelSpeed = speedInput * reverseAccelSpeed * 100f;
            HandleSkidMarks(false);
            //Exhaust
            HandleExhaustParticle(false);
        }
        else
        {
            accelSpeed = 0;
            //Exhaust
            HandleExhaustParticle(true);
        }

        //Steering Car Right / Left
        turnInput = Input.GetAxis("Horizontal");

        //Car is turning
        if(Mathf.Abs(turnInput) > 0)
        {
            HandleSkidMarks(true);
        }
        else
        {
            HandleSkidMarks(false);
        }

    }

    public void ApplyGravity()
    {
        //apply a force in the negative
        rb.AddForce(Vector3.down * gravityForce * 100f);
    }

    public bool IsGround()
    {
        for (int i = 0; i < groundRayPoint.Length; i++)
        {
            //Draws Rays
            Debug.DrawRay(groundRayPoint[i].position, -groundRayPoint[i].up, Color.blue);

            RaycastHit hit;
            if (Physics.Raycast(groundRayPoint[i].position, -groundRayPoint[i].up, out hit, groundRayLength, layerGround))
            {
                //If ray hits ground then we are on the ground
                isGrounded = true;
                //Sets the drag to be ground drag
                rb.drag = dragOnGround;
                //Rotates car to the rotation of the ground
                //TODO Change this to be four points
                normalAccumulator = Vector3.Lerp(normalAccumulator, hit.normal, snapToGroundLevel);
                transform.rotation = Quaternion.LookRotation(Vector3.Cross(transform.right, normalAccumulator));
            }
            else
            {
                //ray hit non ground
                isGrounded = false;
                //Set the drag to be off ground drag
                rb.drag = dragOffGround;
            }
        }

        

        return isGrounded;
    }

    private void HandleExhaustParticle(bool isStopped)
    {
        foreach(ParticleSystem exhaust in exhaustParticles)
        {
            exhaust.enableEmission = isStopped;
        }
    }

    private void HandleSkidMarks(bool isSkid)
    {
        foreach(TrailRenderer skids in skidMarks)
        {
            skids.emitting = isSkid;
        }
    }
}
