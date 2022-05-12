using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
     
public enum SpeedDisplay
{
    MPH,
    KPH
};

public class Speedometer : MonoBehaviour
{
    public Rigidbody rbTarget;

    public Text speedText; //Text for speed
    public RectTransform needle; //needle in the speedometer
    public float currentSpeed; //speed of rbTarget
    public float maxSpeed; //the max speed of car

    public float minSpeedNeedleAngle; //Min angle the needle can go
    public float maxSpeedNeedleAngle; //Max angle the needle can go

    public SpeedDisplay speedDisplay = new SpeedDisplay();

    private void Update()
    {
        PrintUI(speedDisplay, maxSpeed);
    }

    private void PrintUI(SpeedDisplay display, float _maxSpeed)
    {
        if(display == SpeedDisplay.MPH)
        {
            //Sets the current speed
            currentSpeed = Mathf.Round(rbTarget.velocity.magnitude * 2.237f);
            speedText.text = "MPH " + Mathf.RoundToInt(currentSpeed);

            //Convert maxSpeed to mph
            _maxSpeed = maxSpeed;
        }
        else if(display == SpeedDisplay.KPH)
        {
            //Sets the current speed
            currentSpeed = Mathf.Round(rbTarget.velocity.magnitude * 3.6f);
            speedText.text = "KPH " + Mathf.RoundToInt(currentSpeed);

            //Convert maxSpeed to kph
            _maxSpeed = maxSpeed * 1.609f;
        }
    
        //Rotate Needle
        needle.localEulerAngles = new Vector3(0f, 0f, Mathf.Lerp(minSpeedNeedleAngle, maxSpeedNeedleAngle, currentSpeed / _maxSpeed));


        //rpm = Mathf.Round((speed % 30) * 40);
        //rpmText.text = "RPM: " + rpm;
    }
}
