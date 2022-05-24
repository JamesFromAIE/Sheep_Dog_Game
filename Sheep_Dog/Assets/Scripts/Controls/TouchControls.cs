// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Controls/TouchControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @TouchControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @TouchControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""TouchControls"",
    ""maps"": [
        {
            ""name"": ""TouchPC"",
            ""id"": ""e7e75935-03ca-41e0-8bf6-87595c63f342"",
            ""actions"": [
                {
                    ""name"": ""TouchInput"",
                    ""type"": ""PassThrough"",
                    ""id"": ""3ab59a3c-95fd-4b35-8b1b-cfbf4645a780"",
                    ""expectedControlType"": ""Touch"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TouchPress"",
                    ""type"": ""Button"",
                    ""id"": ""a2a923d5-ccc8-4e03-86ae-ccd22b85f716"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TouchSelect"",
                    ""type"": ""Button"",
                    ""id"": ""65533521-e500-4e20-a4c1-440afb5702f2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TouchPosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""5494e37a-3318-42be-8192-ad3e8b34fbaf"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseSelect"",
                    ""type"": ""Button"",
                    ""id"": ""058c80e4-e259-4a2d-a43c-491941e7e75e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseMove"",
                    ""type"": ""Button"",
                    ""id"": ""022aa71a-865f-4268-9fc5-c7c6c4cda4f7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SelectDog1"",
                    ""type"": ""Button"",
                    ""id"": ""cf45488a-e76a-4b7d-b734-7e70d4cd0f08"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SelectDog2"",
                    ""type"": ""Button"",
                    ""id"": ""d20a5e82-9380-4f7a-867e-6086413a12e6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""Button"",
                    ""id"": ""222c9e14-8886-4e11-99a3-673214dcc060"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PausePC"",
                    ""type"": ""Button"",
                    ""id"": ""9ea52440-0e82-49a5-b045-b6ea7ad263fe"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PauseMobile"",
                    ""type"": ""Button"",
                    ""id"": ""f22b82b3-9476-41d8-accf-4366e7c4f83f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c13027a9-6c3c-40d3-a81a-572e40ebb4e8"",
                    ""path"": ""<Touchscreen>/primaryTouch"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TouchInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4c6f7b06-911d-499a-ae4d-f3e29993a661"",
                    ""path"": ""<Touchscreen>/primaryTouch/press"",
                    ""interactions"": ""Press(pressPoint=0.2,behavior=2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TouchPress"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4d919e63-1974-4214-9cc0-8729def02acc"",
                    ""path"": ""<Touchscreen>/primaryTouch/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TouchPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5499d55b-560c-463a-9ee0-6b2a31833901"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseSelect"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""03c125b5-c731-4940-83cd-a4aebdb0fdf7"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectDog1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0b757274-3363-4a9b-86bb-531eaccb2d7c"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectDog2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7445939f-6092-42d9-94f0-9e2c1c70ba15"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""37b25110-2413-4317-aacc-b90c49e22399"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""74a53c79-96a8-4e3a-a20b-c20c511df812"",
                    ""path"": ""<Touchscreen>/primaryTouch/tap"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TouchSelect"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""83458ac5-b59b-4e36-8092-462918c9a2e9"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PausePC"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""82ae60c7-efe2-4082-ac63-66d1193f4017"",
                    ""path"": ""*/{Back}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PauseMobile"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // TouchPC
        m_TouchPC = asset.FindActionMap("TouchPC", throwIfNotFound: true);
        m_TouchPC_TouchInput = m_TouchPC.FindAction("TouchInput", throwIfNotFound: true);
        m_TouchPC_TouchPress = m_TouchPC.FindAction("TouchPress", throwIfNotFound: true);
        m_TouchPC_TouchSelect = m_TouchPC.FindAction("TouchSelect", throwIfNotFound: true);
        m_TouchPC_TouchPosition = m_TouchPC.FindAction("TouchPosition", throwIfNotFound: true);
        m_TouchPC_MouseSelect = m_TouchPC.FindAction("MouseSelect", throwIfNotFound: true);
        m_TouchPC_MouseMove = m_TouchPC.FindAction("MouseMove", throwIfNotFound: true);
        m_TouchPC_SelectDog1 = m_TouchPC.FindAction("SelectDog1", throwIfNotFound: true);
        m_TouchPC_SelectDog2 = m_TouchPC.FindAction("SelectDog2", throwIfNotFound: true);
        m_TouchPC_MousePosition = m_TouchPC.FindAction("MousePosition", throwIfNotFound: true);
        m_TouchPC_PausePC = m_TouchPC.FindAction("PausePC", throwIfNotFound: true);
        m_TouchPC_PauseMobile = m_TouchPC.FindAction("PauseMobile", throwIfNotFound: true);
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

    // TouchPC
    private readonly InputActionMap m_TouchPC;
    private ITouchPCActions m_TouchPCActionsCallbackInterface;
    private readonly InputAction m_TouchPC_TouchInput;
    private readonly InputAction m_TouchPC_TouchPress;
    private readonly InputAction m_TouchPC_TouchSelect;
    private readonly InputAction m_TouchPC_TouchPosition;
    private readonly InputAction m_TouchPC_MouseSelect;
    private readonly InputAction m_TouchPC_MouseMove;
    private readonly InputAction m_TouchPC_SelectDog1;
    private readonly InputAction m_TouchPC_SelectDog2;
    private readonly InputAction m_TouchPC_MousePosition;
    private readonly InputAction m_TouchPC_PausePC;
    private readonly InputAction m_TouchPC_PauseMobile;
    public struct TouchPCActions
    {
        private @TouchControls m_Wrapper;
        public TouchPCActions(@TouchControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @TouchInput => m_Wrapper.m_TouchPC_TouchInput;
        public InputAction @TouchPress => m_Wrapper.m_TouchPC_TouchPress;
        public InputAction @TouchSelect => m_Wrapper.m_TouchPC_TouchSelect;
        public InputAction @TouchPosition => m_Wrapper.m_TouchPC_TouchPosition;
        public InputAction @MouseSelect => m_Wrapper.m_TouchPC_MouseSelect;
        public InputAction @MouseMove => m_Wrapper.m_TouchPC_MouseMove;
        public InputAction @SelectDog1 => m_Wrapper.m_TouchPC_SelectDog1;
        public InputAction @SelectDog2 => m_Wrapper.m_TouchPC_SelectDog2;
        public InputAction @MousePosition => m_Wrapper.m_TouchPC_MousePosition;
        public InputAction @PausePC => m_Wrapper.m_TouchPC_PausePC;
        public InputAction @PauseMobile => m_Wrapper.m_TouchPC_PauseMobile;
        public InputActionMap Get() { return m_Wrapper.m_TouchPC; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TouchPCActions set) { return set.Get(); }
        public void SetCallbacks(ITouchPCActions instance)
        {
            if (m_Wrapper.m_TouchPCActionsCallbackInterface != null)
            {
                @TouchInput.started -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnTouchInput;
                @TouchInput.performed -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnTouchInput;
                @TouchInput.canceled -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnTouchInput;
                @TouchPress.started -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnTouchPress;
                @TouchPress.performed -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnTouchPress;
                @TouchPress.canceled -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnTouchPress;
                @TouchSelect.started -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnTouchSelect;
                @TouchSelect.performed -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnTouchSelect;
                @TouchSelect.canceled -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnTouchSelect;
                @TouchPosition.started -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnTouchPosition;
                @TouchPosition.performed -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnTouchPosition;
                @TouchPosition.canceled -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnTouchPosition;
                @MouseSelect.started -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnMouseSelect;
                @MouseSelect.performed -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnMouseSelect;
                @MouseSelect.canceled -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnMouseSelect;
                @MouseMove.started -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnMouseMove;
                @MouseMove.performed -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnMouseMove;
                @MouseMove.canceled -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnMouseMove;
                @SelectDog1.started -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnSelectDog1;
                @SelectDog1.performed -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnSelectDog1;
                @SelectDog1.canceled -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnSelectDog1;
                @SelectDog2.started -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnSelectDog2;
                @SelectDog2.performed -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnSelectDog2;
                @SelectDog2.canceled -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnSelectDog2;
                @MousePosition.started -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnMousePosition;
                @PausePC.started -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnPausePC;
                @PausePC.performed -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnPausePC;
                @PausePC.canceled -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnPausePC;
                @PauseMobile.started -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnPauseMobile;
                @PauseMobile.performed -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnPauseMobile;
                @PauseMobile.canceled -= m_Wrapper.m_TouchPCActionsCallbackInterface.OnPauseMobile;
            }
            m_Wrapper.m_TouchPCActionsCallbackInterface = instance;
            if (instance != null)
            {
                @TouchInput.started += instance.OnTouchInput;
                @TouchInput.performed += instance.OnTouchInput;
                @TouchInput.canceled += instance.OnTouchInput;
                @TouchPress.started += instance.OnTouchPress;
                @TouchPress.performed += instance.OnTouchPress;
                @TouchPress.canceled += instance.OnTouchPress;
                @TouchSelect.started += instance.OnTouchSelect;
                @TouchSelect.performed += instance.OnTouchSelect;
                @TouchSelect.canceled += instance.OnTouchSelect;
                @TouchPosition.started += instance.OnTouchPosition;
                @TouchPosition.performed += instance.OnTouchPosition;
                @TouchPosition.canceled += instance.OnTouchPosition;
                @MouseSelect.started += instance.OnMouseSelect;
                @MouseSelect.performed += instance.OnMouseSelect;
                @MouseSelect.canceled += instance.OnMouseSelect;
                @MouseMove.started += instance.OnMouseMove;
                @MouseMove.performed += instance.OnMouseMove;
                @MouseMove.canceled += instance.OnMouseMove;
                @SelectDog1.started += instance.OnSelectDog1;
                @SelectDog1.performed += instance.OnSelectDog1;
                @SelectDog1.canceled += instance.OnSelectDog1;
                @SelectDog2.started += instance.OnSelectDog2;
                @SelectDog2.performed += instance.OnSelectDog2;
                @SelectDog2.canceled += instance.OnSelectDog2;
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
                @PausePC.started += instance.OnPausePC;
                @PausePC.performed += instance.OnPausePC;
                @PausePC.canceled += instance.OnPausePC;
                @PauseMobile.started += instance.OnPauseMobile;
                @PauseMobile.performed += instance.OnPauseMobile;
                @PauseMobile.canceled += instance.OnPauseMobile;
            }
        }
    }
    public TouchPCActions @TouchPC => new TouchPCActions(this);
    public interface ITouchPCActions
    {
        void OnTouchInput(InputAction.CallbackContext context);
        void OnTouchPress(InputAction.CallbackContext context);
        void OnTouchSelect(InputAction.CallbackContext context);
        void OnTouchPosition(InputAction.CallbackContext context);
        void OnMouseSelect(InputAction.CallbackContext context);
        void OnMouseMove(InputAction.CallbackContext context);
        void OnSelectDog1(InputAction.CallbackContext context);
        void OnSelectDog2(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
        void OnPausePC(InputAction.CallbackContext context);
        void OnPauseMobile(InputAction.CallbackContext context);
    }
}
