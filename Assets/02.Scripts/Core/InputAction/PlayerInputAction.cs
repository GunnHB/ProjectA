//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/02.Scripts/Core/InputAction/PlayerInputAction.inputactions
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

public partial class @PlayerInputAction: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputAction()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputAction"",
    ""maps"": [
        {
            ""name"": ""PlayerActionMap"",
            ""id"": ""91718554-72f3-4ab8-9b02-7b3882f249cd"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""ab561e71-9611-4a90-903e-075fe49f97cb"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""LookAt"",
                    ""type"": ""PassThrough"",
                    ""id"": ""6772314f-7265-4969-a386-6c0cf44c3f64"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""2f577222-6562-47e6-8983-30f477ef4220"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""641978b0-5a74-4a76-a312-2b3d30ea489d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""b9b0c474-d86d-43de-8f4a-6ba76d4918d0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""DrawWeapon"",
                    ""type"": ""Button"",
                    ""id"": ""a72a572f-efd3-4471-910f-a68471f74546"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""d3ef5c75-f158-4f58-8e61-4723f4f3bb2b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ZoomIn"",
                    ""type"": ""Value"",
                    ""id"": ""d536393c-ea67-4fbe-b4a0-c9e48c6cee0a"",
                    ""expectedControlType"": ""Analog"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""ZoomOut"",
                    ""type"": ""Value"",
                    ""id"": ""78bdf86f-814d-46de-ab54-2a90fe55157b"",
                    ""expectedControlType"": ""Analog"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Inventory"",
                    ""type"": ""Button"",
                    ""id"": ""70fd4ef1-39a3-4b1c-a940-39f495efb6f1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Setting"",
                    ""type"": ""Button"",
                    ""id"": ""f2a04616-09a0-48aa-9115-5293b44c0de7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Focus"",
                    ""type"": ""Button"",
                    ""id"": ""95b0f95a-923b-45bb-9c53-735653b8fe12"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""d0782d8a-b58a-4c36-8510-ea4cc4d44ddb"",
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
                    ""id"": ""bf3c948a-e84c-4dee-ab3c-0b1aeb1fd0e4"",
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
                    ""id"": ""a6d61498-4d58-4807-9661-1591ffba03b9"",
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
                    ""id"": ""a2c26b97-6095-4d13-a67f-6672f3023a0c"",
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
                    ""id"": ""8015396d-770c-47df-9e5e-b37440a48303"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrow"",
                    ""id"": ""de7ec7d9-920e-4ac8-a0c0-afba3fdb8a65"",
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
                    ""id"": ""dc5fa48b-443d-42a2-bf0f-1bfa440b72c7"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ce3af1da-e3ac-4cd1-bd36-97e6d410140d"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""1d4a8f1e-67c6-4420-bf2a-9a0839cde01f"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""3ae23b6f-250e-429e-8287-74bc41a59609"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""4a92ea32-14e7-4563-ac5b-fd088a4a38b6"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LookAt"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f4145560-4f12-4b89-b650-a95ab1cf8d41"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e4441174-e389-4287-803e-b88d37715563"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7cd34ad3-d301-44f0-b8eb-db975fc028fa"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4ea63579-9946-434a-b4f4-9c05dcdfe8b3"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DrawWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aefa87d8-8117-484b-a478-f6230d4e1e71"",
                    ""path"": ""<Mouse>/scroll/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ZoomIn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b2f6f457-4994-44c1-9760-a7ee856e8319"",
                    ""path"": ""<Mouse>/scroll/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ZoomOut"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ec45656f-4a8b-49cc-af25-2f39c354c58d"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""69547a1a-dde2-44b7-afbc-06a9a32aac96"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e0ceba20-1010-4aa7-acd5-d09410d8653d"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Setting"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b7563899-9b65-443d-a0b4-d4b39425922e"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Focus"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UIActionMap"",
            ""id"": ""2fdc8122-433f-4ae0-bcd1-406bb2568167"",
            ""actions"": [
                {
                    ""name"": ""Escape"",
                    ""type"": ""Button"",
                    ""id"": ""5f1ba34a-6f91-4624-89e0-7ef4119e896e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b7c21b71-0092-402e-88c9-c36145c2e033"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Escape"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""PC"",
            ""bindingGroup"": ""PC"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // PlayerActionMap
        m_PlayerActionMap = asset.FindActionMap("PlayerActionMap", throwIfNotFound: true);
        m_PlayerActionMap_Movement = m_PlayerActionMap.FindAction("Movement", throwIfNotFound: true);
        m_PlayerActionMap_LookAt = m_PlayerActionMap.FindAction("LookAt", throwIfNotFound: true);
        m_PlayerActionMap_Sprint = m_PlayerActionMap.FindAction("Sprint", throwIfNotFound: true);
        m_PlayerActionMap_Jump = m_PlayerActionMap.FindAction("Jump", throwIfNotFound: true);
        m_PlayerActionMap_Crouch = m_PlayerActionMap.FindAction("Crouch", throwIfNotFound: true);
        m_PlayerActionMap_DrawWeapon = m_PlayerActionMap.FindAction("DrawWeapon", throwIfNotFound: true);
        m_PlayerActionMap_Attack = m_PlayerActionMap.FindAction("Attack", throwIfNotFound: true);
        m_PlayerActionMap_ZoomIn = m_PlayerActionMap.FindAction("ZoomIn", throwIfNotFound: true);
        m_PlayerActionMap_ZoomOut = m_PlayerActionMap.FindAction("ZoomOut", throwIfNotFound: true);
        m_PlayerActionMap_Inventory = m_PlayerActionMap.FindAction("Inventory", throwIfNotFound: true);
        m_PlayerActionMap_Setting = m_PlayerActionMap.FindAction("Setting", throwIfNotFound: true);
        m_PlayerActionMap_Focus = m_PlayerActionMap.FindAction("Focus", throwIfNotFound: true);
        // UIActionMap
        m_UIActionMap = asset.FindActionMap("UIActionMap", throwIfNotFound: true);
        m_UIActionMap_Escape = m_UIActionMap.FindAction("Escape", throwIfNotFound: true);
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

    // PlayerActionMap
    private readonly InputActionMap m_PlayerActionMap;
    private List<IPlayerActionMapActions> m_PlayerActionMapActionsCallbackInterfaces = new List<IPlayerActionMapActions>();
    private readonly InputAction m_PlayerActionMap_Movement;
    private readonly InputAction m_PlayerActionMap_LookAt;
    private readonly InputAction m_PlayerActionMap_Sprint;
    private readonly InputAction m_PlayerActionMap_Jump;
    private readonly InputAction m_PlayerActionMap_Crouch;
    private readonly InputAction m_PlayerActionMap_DrawWeapon;
    private readonly InputAction m_PlayerActionMap_Attack;
    private readonly InputAction m_PlayerActionMap_ZoomIn;
    private readonly InputAction m_PlayerActionMap_ZoomOut;
    private readonly InputAction m_PlayerActionMap_Inventory;
    private readonly InputAction m_PlayerActionMap_Setting;
    private readonly InputAction m_PlayerActionMap_Focus;
    public struct PlayerActionMapActions
    {
        private @PlayerInputAction m_Wrapper;
        public PlayerActionMapActions(@PlayerInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_PlayerActionMap_Movement;
        public InputAction @LookAt => m_Wrapper.m_PlayerActionMap_LookAt;
        public InputAction @Sprint => m_Wrapper.m_PlayerActionMap_Sprint;
        public InputAction @Jump => m_Wrapper.m_PlayerActionMap_Jump;
        public InputAction @Crouch => m_Wrapper.m_PlayerActionMap_Crouch;
        public InputAction @DrawWeapon => m_Wrapper.m_PlayerActionMap_DrawWeapon;
        public InputAction @Attack => m_Wrapper.m_PlayerActionMap_Attack;
        public InputAction @ZoomIn => m_Wrapper.m_PlayerActionMap_ZoomIn;
        public InputAction @ZoomOut => m_Wrapper.m_PlayerActionMap_ZoomOut;
        public InputAction @Inventory => m_Wrapper.m_PlayerActionMap_Inventory;
        public InputAction @Setting => m_Wrapper.m_PlayerActionMap_Setting;
        public InputAction @Focus => m_Wrapper.m_PlayerActionMap_Focus;
        public InputActionMap Get() { return m_Wrapper.m_PlayerActionMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActionMapActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActionMapActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionMapActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionMapActionsCallbackInterfaces.Add(instance);
            @Movement.started += instance.OnMovement;
            @Movement.performed += instance.OnMovement;
            @Movement.canceled += instance.OnMovement;
            @LookAt.started += instance.OnLookAt;
            @LookAt.performed += instance.OnLookAt;
            @LookAt.canceled += instance.OnLookAt;
            @Sprint.started += instance.OnSprint;
            @Sprint.performed += instance.OnSprint;
            @Sprint.canceled += instance.OnSprint;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Crouch.started += instance.OnCrouch;
            @Crouch.performed += instance.OnCrouch;
            @Crouch.canceled += instance.OnCrouch;
            @DrawWeapon.started += instance.OnDrawWeapon;
            @DrawWeapon.performed += instance.OnDrawWeapon;
            @DrawWeapon.canceled += instance.OnDrawWeapon;
            @Attack.started += instance.OnAttack;
            @Attack.performed += instance.OnAttack;
            @Attack.canceled += instance.OnAttack;
            @ZoomIn.started += instance.OnZoomIn;
            @ZoomIn.performed += instance.OnZoomIn;
            @ZoomIn.canceled += instance.OnZoomIn;
            @ZoomOut.started += instance.OnZoomOut;
            @ZoomOut.performed += instance.OnZoomOut;
            @ZoomOut.canceled += instance.OnZoomOut;
            @Inventory.started += instance.OnInventory;
            @Inventory.performed += instance.OnInventory;
            @Inventory.canceled += instance.OnInventory;
            @Setting.started += instance.OnSetting;
            @Setting.performed += instance.OnSetting;
            @Setting.canceled += instance.OnSetting;
            @Focus.started += instance.OnFocus;
            @Focus.performed += instance.OnFocus;
            @Focus.canceled += instance.OnFocus;
        }

        private void UnregisterCallbacks(IPlayerActionMapActions instance)
        {
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
            @LookAt.started -= instance.OnLookAt;
            @LookAt.performed -= instance.OnLookAt;
            @LookAt.canceled -= instance.OnLookAt;
            @Sprint.started -= instance.OnSprint;
            @Sprint.performed -= instance.OnSprint;
            @Sprint.canceled -= instance.OnSprint;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Crouch.started -= instance.OnCrouch;
            @Crouch.performed -= instance.OnCrouch;
            @Crouch.canceled -= instance.OnCrouch;
            @DrawWeapon.started -= instance.OnDrawWeapon;
            @DrawWeapon.performed -= instance.OnDrawWeapon;
            @DrawWeapon.canceled -= instance.OnDrawWeapon;
            @Attack.started -= instance.OnAttack;
            @Attack.performed -= instance.OnAttack;
            @Attack.canceled -= instance.OnAttack;
            @ZoomIn.started -= instance.OnZoomIn;
            @ZoomIn.performed -= instance.OnZoomIn;
            @ZoomIn.canceled -= instance.OnZoomIn;
            @ZoomOut.started -= instance.OnZoomOut;
            @ZoomOut.performed -= instance.OnZoomOut;
            @ZoomOut.canceled -= instance.OnZoomOut;
            @Inventory.started -= instance.OnInventory;
            @Inventory.performed -= instance.OnInventory;
            @Inventory.canceled -= instance.OnInventory;
            @Setting.started -= instance.OnSetting;
            @Setting.performed -= instance.OnSetting;
            @Setting.canceled -= instance.OnSetting;
            @Focus.started -= instance.OnFocus;
            @Focus.performed -= instance.OnFocus;
            @Focus.canceled -= instance.OnFocus;
        }

        public void RemoveCallbacks(IPlayerActionMapActions instance)
        {
            if (m_Wrapper.m_PlayerActionMapActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActionMapActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionMapActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionMapActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActionMapActions @PlayerActionMap => new PlayerActionMapActions(this);

    // UIActionMap
    private readonly InputActionMap m_UIActionMap;
    private List<IUIActionMapActions> m_UIActionMapActionsCallbackInterfaces = new List<IUIActionMapActions>();
    private readonly InputAction m_UIActionMap_Escape;
    public struct UIActionMapActions
    {
        private @PlayerInputAction m_Wrapper;
        public UIActionMapActions(@PlayerInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @Escape => m_Wrapper.m_UIActionMap_Escape;
        public InputActionMap Get() { return m_Wrapper.m_UIActionMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActionMapActions set) { return set.Get(); }
        public void AddCallbacks(IUIActionMapActions instance)
        {
            if (instance == null || m_Wrapper.m_UIActionMapActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_UIActionMapActionsCallbackInterfaces.Add(instance);
            @Escape.started += instance.OnEscape;
            @Escape.performed += instance.OnEscape;
            @Escape.canceled += instance.OnEscape;
        }

        private void UnregisterCallbacks(IUIActionMapActions instance)
        {
            @Escape.started -= instance.OnEscape;
            @Escape.performed -= instance.OnEscape;
            @Escape.canceled -= instance.OnEscape;
        }

        public void RemoveCallbacks(IUIActionMapActions instance)
        {
            if (m_Wrapper.m_UIActionMapActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IUIActionMapActions instance)
        {
            foreach (var item in m_Wrapper.m_UIActionMapActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_UIActionMapActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public UIActionMapActions @UIActionMap => new UIActionMapActions(this);
    private int m_PCSchemeIndex = -1;
    public InputControlScheme PCScheme
    {
        get
        {
            if (m_PCSchemeIndex == -1) m_PCSchemeIndex = asset.FindControlSchemeIndex("PC");
            return asset.controlSchemes[m_PCSchemeIndex];
        }
    }
    public interface IPlayerActionMapActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnLookAt(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
        void OnDrawWeapon(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnZoomIn(InputAction.CallbackContext context);
        void OnZoomOut(InputAction.CallbackContext context);
        void OnInventory(InputAction.CallbackContext context);
        void OnSetting(InputAction.CallbackContext context);
        void OnFocus(InputAction.CallbackContext context);
    }
    public interface IUIActionMapActions
    {
        void OnEscape(InputAction.CallbackContext context);
    }
}
