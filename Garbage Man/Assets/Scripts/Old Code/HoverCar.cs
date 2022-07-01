using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverCar : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody rb;
    public Transform[] rayPoints;

    [Header("Hover Variables")]
    [Tooltip("How much force is applied to push of ground")] public float suspensionStrength = 10f;
    [Tooltip("Length of the raycast")] public float suspensionLength = 1f;
    [Tooltip("???")] public float dampening = 50f;
    [Tooltip("Force applied pushing downward on object")] public float downForce = 10f;
    [Tooltip("What layer the raycasts hit")] public LayerMask groundLayer;
    private float lastHitDist;

    [Header("Car Variables")]
    [Tooltip("Speed of acceleration")] public float accelSpeed;
    [Tooltip("How fast the car turns")] public float turnSpeed;
    public float flipForce;
    public float flipSpeed;
    public bool isGrounded;
    [Tooltip("Drag put on rigidbody when the object is in the air")] public float dragInAir = 0f;
    [Tooltip("Drag put on rigidbody when the object is on the ground")] public float dragOnGround = 2f;

    [Header("Player Inputs")]
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    private KeyCode flipOverKey = KeyCode.F;
    private float horizontalInput;
    private float verticalInput;
    private bool flipOverInput;

    private void Update()
    {
        GetInputs();
        UpdateWheelsAnim();
        FlipCarOver();
    }

    private void FixedUpdate()
    {
        //Push object off ground at raypoints
        for (int i = 0; i < rayPoints.Length; i++)
        {
            ApplyHoverForce(rayPoints[i]);
        }

        HandleMotor();
        HandleSteering();
        HandleIsGround();
        HandleDownForce();
    }

    private void GetInputs()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);

        flipOverInput = Input.GetKeyDown(flipOverKey);
    }

    private void HandleMotor()
    {
        //Push vehicle forward or backward
        if(isGrounded)
        {
            float force = accelSpeed * verticalInput;
            rb.AddForce(force * transform.forward, ForceMode.Acceleration);
        }
    }

    private void HandleBreak()
    {
        //Stop vehicle
    }

    private void HandleSteering()
    {
        //Turn vehicle with torque
        float torque = turnSpeed * horizontalInput;
        rb.AddTorque(torque * transform.up);
    }

    private void HandleCounterForce()
    {
        //Counter force which forces car into the direction it looks

    }

    private void UpdateWheelsAnim()
    {
        //Update the wheels to be at the suspensionlength
    }

    private void HandleIsGround()
    {
        if(isGrounded)
        {
            rb.drag = dragOnGround;
        }
        else
        {
            rb.drag = dragInAir;
        }
    }

    private void HandleDownForce()
    {
        //Apply slight down force
        //Stronger force as car moves faster
        float force = downForce;
        rb.AddForce(force * -transform.up);
    }

    private void FlipCarOver()
    {
        //Flip car back to 0 on Z - axis
        if(flipOverInput  && !isGrounded)
        {
            //Push car up into the air
            rb.AddForce(flipForce * -transform.up, ForceMode.Impulse);

            //Flip car back over on the z axis
            //Return to 0 fast (not instantly)
            Vector3 eulerRotation = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, Mathf.Lerp(eulerRotation.z, 0f, flipSpeed));
        }
    }

    private void ApplyHoverForce(Transform rayPoint)
    {
        RaycastHit hit;

        Debug.DrawRay(rayPoint.position, -rayPoint.up);

        if (Physics.Raycast(rayPoint.position, -rayPoint.up, out hit, suspensionLength, groundLayer))
        {
            float forceAmount = HookesLawDamp(hit.distance);

            rb.AddForceAtPosition(rayPoint.up * forceAmount, rayPoint.position);

            isGrounded = true;
        }
        else
        {
            lastHitDist = suspensionLength * 1.1f;

            isGrounded = false;
        }
    }

    private float HookesLawDamp(float hitDistance)
    {
        float forceAmount = suspensionStrength * (suspensionLength - hitDistance) + (dampening * (lastHitDist - hitDistance));
        forceAmount = Mathf.Max(0f, forceAmount);
        lastHitDist = hitDistance;

        return forceAmount;
    }
}
