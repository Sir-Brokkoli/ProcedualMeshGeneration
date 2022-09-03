// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Player/PlayerInputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""Locomotion"",
            ""id"": ""7df42369-de24-41d7-a299-d44095dd2762"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""d1a1c740-944b-4199-8664-452c544aa2a7"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Camera"",
                    ""type"": ""PassThrough"",
                    ""id"": ""9e5632c1-49f1-4363-90d1-58a10bc99532"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""329ea86d-5fbb-48b4-a5fd-74840a5237c8"",
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
                    ""id"": ""616fa45a-4f61-4b24-9682-70197ffe0c3a"",
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
                    ""id"": ""bc919d7e-d1ad-4def-9462-bbb98502c68a"",
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
                    ""id"": ""10a16852-d888-4f26-9b82-b2832de449d7"",
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
                    ""id"": ""9503b819-373a-4731-a2bb-b518eeb6a971"",
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
                    ""id"": ""85240155-3baf-4b73-8b3d-d53b7bacf544"",
                    ""path"": ""<HID::SHANWAN Trust Gamepad>/hat"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""18d0def1-b458-4fa4-a21a-3543df21f199"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Locomotion
        m_Locomotion = asset.FindActionMap("Locomotion", throwIfNotFound: true);
        m_Locomotion_Movement = m_Locomotion.FindAction("Movement", throwIfNotFound: true);
        m_Locomotion_Camera = m_Locomotion.FindAction("Camera", throwIfNotFound: true);
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

    // Locomotion
    private readonly InputActionMap m_Locomotion;
    private ILocomotionActions m_LocomotionActionsCallbackInterface;
    private readonly InputAction m_Locomotion_Movement;
    private readonly InputAction m_Locomotion_Camera;
    public struct LocomotionActions
    {
        private @PlayerInputActions m_Wrapper;
        public LocomotionActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Locomotion_Movement;
        public InputAction @Camera => m_Wrapper.m_Locomotion_Camera;
        public InputActionMap Get() { return m_Wrapper.m_Locomotion; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(LocomotionActions set) { return set.Get(); }
        public void SetCallbacks(ILocomotionActions instance)
        {
            if (m_Wrapper.m_LocomotionActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnMovement;
                @Camera.started -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnCamera;
                @Camera.performed -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnCamera;
                @Camera.canceled -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnCamera;
            }
            m_Wrapper.m_LocomotionActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Camera.started += instance.OnCamera;
                @Camera.performed += instance.OnCamera;
                @Camera.canceled += instance.OnCamera;
            }
        }
    }
    public LocomotionActions @Locomotion => new LocomotionActions(this);
    public interface ILocomotionActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnCamera(InputAction.CallbackContext context);
    }
}
