//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.2.0
//     from Assets/Inputs/PlayerActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerActions"",
    ""maps"": [
        {
            ""name"": ""CarControls"",
            ""id"": ""b9c0ae7f-f249-41eb-abfb-e4cf7b3cd858"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""7b4ffd85-5d59-4580-9ed0-24bef13dc861"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Value"",
                    ""id"": ""bb85f34d-f055-4661-a9de-5dced071420f"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""5184f061-eeda-4fd5-805b-ececf3e8666d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Power Slide"",
                    ""type"": ""Button"",
                    ""id"": ""a3e68813-dee1-42b5-bc4d-4b06c40d9a54"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WS"",
                    ""id"": ""826ffea4-c9ff-4daf-b6a0-6a3ffaf38b11"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""ab937e58-c1e6-486e-81a4-b19002952e3d"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""d931f0ec-2be3-40e1-a4f3-8165630043e0"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""RT LT"",
                    ""id"": ""e2902bd3-a218-43dd-ac5c-5f7cde2c41b4"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": ""AxisDeadzone"",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""bfb1a6b1-4e75-4cf8-a4d1-86137ef34917"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""1a8584ed-7da8-456a-8182-19b52e8e9a8f"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""AD"",
                    ""id"": ""d94fa698-a866-4b00-8138-a6193e0bc0d6"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""257d51fa-8eba-405d-a51a-404684670b26"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""8244a2cc-8829-44c6-893f-52256e122f72"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left Stick"",
                    ""id"": ""958f8852-93b9-436f-b376-2a5aea1229ad"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": ""AxisDeadzone"",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""63f45700-e684-494b-90fe-93136be80742"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""4bdfb3c9-b7f5-4a78-b083-3ca7fddbb409"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""7de4842f-2021-4154-8242-8b5ba284c9be"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""603a3e06-0f63-4a4c-b424-abfa2ed87ca2"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""515e7c20-ded0-4ed9-9175-a2364c005f9b"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Power Slide"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5f94f612-a31c-4426-9d33-43a588915d67"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Power Slide"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // CarControls
        m_CarControls = asset.FindActionMap("CarControls", throwIfNotFound: true);
        m_CarControls_Move = m_CarControls.FindAction("Move", throwIfNotFound: true);
        m_CarControls_Rotate = m_CarControls.FindAction("Rotate", throwIfNotFound: true);
        m_CarControls_Interact = m_CarControls.FindAction("Interact", throwIfNotFound: true);
        m_CarControls_PowerSlide = m_CarControls.FindAction("Power Slide", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // CarControls
    private readonly InputActionMap m_CarControls;
    private ICarControlsActions m_CarControlsActionsCallbackInterface;
    private readonly InputAction m_CarControls_Move;
    private readonly InputAction m_CarControls_Rotate;
    private readonly InputAction m_CarControls_Interact;
    private readonly InputAction m_CarControls_PowerSlide;
    public struct CarControlsActions
    {
        private @PlayerActions m_Wrapper;
        public CarControlsActions(@PlayerActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_CarControls_Move;
        public InputAction @Rotate => m_Wrapper.m_CarControls_Rotate;
        public InputAction @Interact => m_Wrapper.m_CarControls_Interact;
        public InputAction @PowerSlide => m_Wrapper.m_CarControls_PowerSlide;
        public InputActionMap Get() { return m_Wrapper.m_CarControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CarControlsActions set) { return set.Get(); }
        public void SetCallbacks(ICarControlsActions instance)
        {
            if (m_Wrapper.m_CarControlsActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_CarControlsActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_CarControlsActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_CarControlsActionsCallbackInterface.OnMove;
                @Rotate.started -= m_Wrapper.m_CarControlsActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_CarControlsActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_CarControlsActionsCallbackInterface.OnRotate;
                @Interact.started -= m_Wrapper.m_CarControlsActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_CarControlsActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_CarControlsActionsCallbackInterface.OnInteract;
                @PowerSlide.started -= m_Wrapper.m_CarControlsActionsCallbackInterface.OnPowerSlide;
                @PowerSlide.performed -= m_Wrapper.m_CarControlsActionsCallbackInterface.OnPowerSlide;
                @PowerSlide.canceled -= m_Wrapper.m_CarControlsActionsCallbackInterface.OnPowerSlide;
            }
            m_Wrapper.m_CarControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @PowerSlide.started += instance.OnPowerSlide;
                @PowerSlide.performed += instance.OnPowerSlide;
                @PowerSlide.canceled += instance.OnPowerSlide;
            }
        }
    }
    public CarControlsActions @CarControls => new CarControlsActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface ICarControlsActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnRotate(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnPowerSlide(InputAction.CallbackContext context);
    }
}