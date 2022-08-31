using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleButton : MonoBehaviour
{
    [Header("Button Stats")]
    [SerializeField] [Tooltip("How close does the button need to get to the pressed position to trigger.")] private float _threshold;
    [SerializeField] [Tooltip("The position the button needs to be to trigger.")] private Vector3 _pressedPos;
    [SerializeField] [Tooltip("Is the button currently pressed.")] private bool _isPressed;
    private bool _prevPressedState;
    [SerializeField] private bool _hasBeenChecked;

    [Header("Button Parts")]
    [SerializeField] [Tooltip("The physical button that will move.")] private GameObject _buttonPush;
    private Vector3 _buttonPushOrigin;
    [SerializeField] [Tooltip("The base of the button.")] private GameObject _buttonBottom;

    [Header("Events")]
    [SerializeField] private UnityEvent _onPressed;
    [SerializeField] private UnityEvent _onReleased;

    private void Awake()
    {
        //Ignore collision between the button bottom and push button
        Physics.IgnoreCollision(_buttonPush.GetComponent<BoxCollider>(), _buttonBottom.GetComponent<BoxCollider>(), true);
    }

    private void Update()
    {
        //Check distance of button
        if (Vector3.Distance(_buttonPush.transform.position, transform.position + _pressedPos) <= _threshold)
        {
            CheckToggle();
        }
        else
        {
            _hasBeenChecked = false;
        }

        //Check if button is pressed
        if (_isPressed && _prevPressedState != _isPressed)
        {
            OnPressed();
        }

        //Check if button is not pressed
        if (!_isPressed && _prevPressedState != _isPressed)
        {
            OnReleased();
        }
    }

    private void CheckToggle()
    {
        if (_hasBeenChecked)
            return;

        _isPressed = !_isPressed;
        _hasBeenChecked = true;
    }

    private void OnPressed()
    {
        //Set the previous state of button being pressed
        _prevPressedState = _isPressed;

        //Invoke the Pressed Event
        _onPressed.Invoke();

    }

    private void OnReleased()
    {
        //Set the previous state of button being pressed
        _prevPressedState = _isPressed;

        //Invoke the Released Event
        _onReleased.Invoke();
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position + _pressedPos, _threshold);

        //Draws a line to the object for on pressed functions
        for (int i = 0; i < _onPressed.GetPersistentEventCount(); i++)
        {
            if (_onPressed.GetPersistentTarget(i) != null)
            {
                var dirName = _onPressed.GetPersistentTarget(i).name;

                if (dirName != "ButtonPush")
                {
                    var dirPos = GameObject.Find(dirName);

                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(transform.position, dirPos.transform.position);
                }
            }
        }

        //Draws a line to the object for on released functions
        for (int i = 0; i < _onReleased.GetPersistentEventCount(); i++)
        {
            if (_onReleased.GetPersistentTarget(i) != null)
            {
                var dirName = _onReleased.GetPersistentTarget(i).name;

                if (dirName != "ButtonPush" && dirName != null)
                {
                    var dirPos = GameObject.Find(dirName);

                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(transform.position, dirPos.transform.position);
                }
            }
        }
    }
}
