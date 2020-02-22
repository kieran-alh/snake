// GENERATED AUTOMATICALLY FROM 'Assets/SnakeInputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @SnakeInputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @SnakeInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""SnakeInputActions"",
    ""maps"": [
        {
            ""name"": ""SnakeControls"",
            ""id"": ""a8908f8a-081d-4902-95e6-f010583ab86c"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""cc9151a2-fb7d-4b87-8546-cb080b1abab5"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e9fb1241-a081-4f43-bd65-7b0a100c9f57"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""0d5f7c03-1912-48c9-b8cd-150f6cc1f608"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6441efff-a283-41a9-b701-a450454618a5"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""3d3337e5-6efb-4b15-afcd-6f20ae60c913"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""e5e61f7a-ac59-4f5a-b05e-d0637cd5a228"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""58de5b92-67cf-46b9-bf01-617a5ecc3ac8"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // SnakeControls
        m_SnakeControls = asset.FindActionMap("SnakeControls", throwIfNotFound: true);
        m_SnakeControls_Move = m_SnakeControls.FindAction("Move", throwIfNotFound: true);
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

    // SnakeControls
    private readonly InputActionMap m_SnakeControls;
    private ISnakeControlsActions m_SnakeControlsActionsCallbackInterface;
    private readonly InputAction m_SnakeControls_Move;
    public struct SnakeControlsActions
    {
        private @SnakeInputActions m_Wrapper;
        public SnakeControlsActions(@SnakeInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_SnakeControls_Move;
        public InputActionMap Get() { return m_Wrapper.m_SnakeControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(SnakeControlsActions set) { return set.Get(); }
        public void SetCallbacks(ISnakeControlsActions instance)
        {
            if (m_Wrapper.m_SnakeControlsActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_SnakeControlsActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_SnakeControlsActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_SnakeControlsActionsCallbackInterface.OnMove;
            }
            m_Wrapper.m_SnakeControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
            }
        }
    }
    public SnakeControlsActions @SnakeControls => new SnakeControlsActions(this);
    public interface ISnakeControlsActions
    {
        void OnMove(InputAction.CallbackContext context);
    }
}
