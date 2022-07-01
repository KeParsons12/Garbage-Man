using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarDebugPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CarStateMachine _carStateMachine;
    [SerializeField] private Speedometer _speedometer;

    [Header("Text boxes")]
    [SerializeField] private Text _currentStateText;
    [SerializeField] private Text _previousStateText;
    [SerializeField] private Text _velocityText;
    [SerializeField] private Text _speedText;
    [SerializeField] private Text _verticalInputText;
    [SerializeField] private Text _horizontalInputText;
    [SerializeField] private Text _fpsText;
    [SerializeField] private Text _motorForceText;
    [SerializeField] private Text _breakForceText;

    private void Update()
    {
        UpdateTextString(_currentStateText, "Current State:", _carStateMachine.CurrentState.ToString());
        UpdateTextString(_previousStateText, "Previous State:", _carStateMachine.LastState.ToString());
        UpdateTextString(_velocityText, "Velocity:", _carStateMachine.Rigidbody.velocity.magnitude.ToString());
        UpdateTextString(_speedText, "Speed:", _speedometer.currentSpeed.ToString());
        UpdateTextString(_verticalInputText, "Vertical Input:", _carStateMachine.VerticalInput.ToString());
        UpdateTextString(_horizontalInputText, "Horizontal Input:", _carStateMachine.HorizontalInput.ToString());
        UpdateTextString(_motorForceText, "Motor Torque:", _carStateMachine.FrontLeftWheelCol.motorTorque.ToString());
        UpdateTextString(_breakForceText, "Break Torque:", _carStateMachine.FrontLeftWheelCol.brakeTorque.ToString());
    }

    private void UpdateTextString(Text ctx, string header ,string text)
    {
        ctx.text = header + text;
    }
}
