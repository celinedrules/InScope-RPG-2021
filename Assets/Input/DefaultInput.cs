// GENERATED AUTOMATICALLY FROM 'Assets/Input/DefaultInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @DefaultInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @DefaultInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""DefaultInput"",
    ""maps"": [
        {
            ""name"": ""Default"",
            ""id"": ""b17ec199-89ac-48e8-a6d1-7176748e4f5a"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Button"",
                    ""id"": ""ec2c2a3f-d04c-490b-90cd-0e0c66296784"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Action 1"",
                    ""type"": ""Button"",
                    ""id"": ""4c1945c4-4acb-4b8b-94dc-205cc09d0812"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Action 2"",
                    ""type"": ""Button"",
                    ""id"": ""0c18424d-8e10-48a4-ad5a-74a9756cc107"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Action 3"",
                    ""type"": ""Button"",
                    ""id"": ""f32f6d89-1086-4ab2-b05a-d81bf66b3d05"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""39f1f2e3-f796-4c5f-8afd-303081bfaa73"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""b7ed07d2-7b1c-4313-8409-98dea6296c2c"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""e4306d0b-5637-4125-b4f9-d541c0b03a24"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""75b11d97-6eb8-4549-a960-52a00e09736a"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""cc761655-c16c-4e80-adf5-e8be40a05290"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""41a2e1d6-4cee-41d4-84ac-c0a71749fbfd"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""9983fd13-36bb-4e83-814d-9537517257d0"",
                    ""path"": ""<Keyboard>/numpad1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action 1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""21a97be8-0ab3-45f8-9873-9aa37a6598b9"",
                    ""path"": ""<Keyboard>/numpad2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action 2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7498aa17-8525-4654-91f5-12f8797d1269"",
                    ""path"": ""<Keyboard>/numpad3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action 3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b17a6af0-6023-4d74-92bf-71156f22647a"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Default
        m_Default = asset.FindActionMap("Default", throwIfNotFound: true);
        m_Default_Movement = m_Default.FindAction("Movement", throwIfNotFound: true);
        m_Default_Action1 = m_Default.FindAction("Action 1", throwIfNotFound: true);
        m_Default_Action2 = m_Default.FindAction("Action 2", throwIfNotFound: true);
        m_Default_Action3 = m_Default.FindAction("Action 3", throwIfNotFound: true);
        m_Default_Interact = m_Default.FindAction("Interact", throwIfNotFound: true);
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

    // Default
    private readonly InputActionMap m_Default;
    private IDefaultActions m_DefaultActionsCallbackInterface;
    private readonly InputAction m_Default_Movement;
    private readonly InputAction m_Default_Action1;
    private readonly InputAction m_Default_Action2;
    private readonly InputAction m_Default_Action3;
    private readonly InputAction m_Default_Interact;
    public struct DefaultActions
    {
        private @DefaultInput m_Wrapper;
        public DefaultActions(@DefaultInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Default_Movement;
        public InputAction @Action1 => m_Wrapper.m_Default_Action1;
        public InputAction @Action2 => m_Wrapper.m_Default_Action2;
        public InputAction @Action3 => m_Wrapper.m_Default_Action3;
        public InputAction @Interact => m_Wrapper.m_Default_Interact;
        public InputActionMap Get() { return m_Wrapper.m_Default; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DefaultActions set) { return set.Get(); }
        public void SetCallbacks(IDefaultActions instance)
        {
            if (m_Wrapper.m_DefaultActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnMovement;
                @Action1.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnAction1;
                @Action1.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnAction1;
                @Action1.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnAction1;
                @Action2.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnAction2;
                @Action2.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnAction2;
                @Action2.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnAction2;
                @Action3.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnAction3;
                @Action3.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnAction3;
                @Action3.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnAction3;
                @Interact.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnInteract;
            }
            m_Wrapper.m_DefaultActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Action1.started += instance.OnAction1;
                @Action1.performed += instance.OnAction1;
                @Action1.canceled += instance.OnAction1;
                @Action2.started += instance.OnAction2;
                @Action2.performed += instance.OnAction2;
                @Action2.canceled += instance.OnAction2;
                @Action3.started += instance.OnAction3;
                @Action3.performed += instance.OnAction3;
                @Action3.canceled += instance.OnAction3;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
            }
        }
    }
    public DefaultActions @Default => new DefaultActions(this);
    public interface IDefaultActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnAction1(InputAction.CallbackContext context);
        void OnAction2(InputAction.CallbackContext context);
        void OnAction3(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
    }
}
