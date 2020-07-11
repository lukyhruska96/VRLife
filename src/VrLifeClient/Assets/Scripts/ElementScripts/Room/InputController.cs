// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/ElementScripts/Room/InputController.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputController : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputController()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputController"",
    ""maps"": [
        {
            ""name"": ""BasicMovements"",
            ""id"": ""02f1f3e6-a568-4b1f-a966-9e06f86ba5e6"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""9f74b65f-b7a0-4830-b1f7-4668a4d2f8a2"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""2e35ca57-2848-44d6-878f-24e5936da87f"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Free look"",
                    ""type"": ""Button"",
                    ""id"": ""bbcd7b0c-37c2-4f23-89bb-8714b48b85c9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""18b0eb92-d2c8-41ad-859d-19f5c571edaf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""ecbf684c-bff4-455d-94d9-4d205d4ede2b"",
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
                    ""id"": ""8152d090-df18-4de8-a61c-5db6842109a8"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e6cae89d-baab-41be-9572-36f16f8bd1e6"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ef1240eb-4262-4310-9ec5-e6002a152ec2"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""25b90874-49c3-467c-bd1a-34abd103407f"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrows"",
                    ""id"": ""59e16318-b64d-40d7-9a1e-df6f6267b8b5"",
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
                    ""id"": ""004e0726-dadc-4b90-a89d-75093efd1375"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""dcad06a6-cef3-4fc6-98d4-a8195c4716a8"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ddf51ada-f375-4c3e-9d28-41e656bcab39"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""3cbd1560-880b-44b4-b5fe-bee11955accc"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left Stick"",
                    ""id"": ""ebcaa41c-b2c4-4b95-b4eb-9319d09aaee0"",
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
                    ""id"": ""6703abbb-0bae-4731-b49e-7ac2b6ec8adf"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""beea1837-12d8-4d84-b748-caa14fd89d65"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""5392d6c8-55e6-43b0-a174-a8c5c5ba1385"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""27d1413e-807f-4bac-a943-ab4790ea7757"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left Oculus Controller"",
                    ""id"": ""1cda06da-1190-419b-90a6-27aae1541a2c"",
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
                    ""id"": ""18da8005-9d4a-415c-b1fd-934c149f204c"",
                    ""path"": ""<OculusTouchController>{LeftHand}/thumbstick/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Oculus VR"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""38e37eb7-e27c-46bd-859f-df148a1654c5"",
                    ""path"": ""<OculusTouchController>{LeftHand}/thumbstick/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Oculus VR"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ff0ce71d-b12f-4c9a-927a-fc83efa71af7"",
                    ""path"": ""<OculusTouchController>{LeftHand}/thumbstick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Oculus VR"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""a9a0246c-0f36-4c43-9dbd-f4104d6b8204"",
                    ""path"": ""<OculusTouchController>{LeftHand}/thumbstick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Oculus VR"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Right Stick"",
                    ""id"": ""63221353-d5a5-4e5e-ad84-31422f67a2f5"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""7caef724-df5f-4843-acda-9e5f5261c867"",
                    ""path"": ""<Gamepad>/rightStick/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ec67f179-18d2-4e1f-8756-5904ff241342"",
                    ""path"": ""<Gamepad>/rightStick/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""33a84705-b71f-4c80-abf8-c0582d7898ac"",
                    ""path"": ""<Gamepad>/rightStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4a5dd3a9-d4d5-4a60-b121-8db4c5cd622d"",
                    ""path"": ""<Gamepad>/rightStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""263c0936-bb82-4862-8a76-0f0052af1140"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Oculus Headset"",
                    ""id"": ""79a11143-e96d-4d5b-bef4-533360f77af6"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""21d93a90-eea1-49c5-aeb2-31d669beec8e"",
                    ""path"": ""<XRController>/deviceRotation/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Oculus VR"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""99f8ac4a-c6cf-4885-9f10-b4d3e4a2825a"",
                    ""path"": ""<XRController>/deviceRotation/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Oculus VR"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a14effaa-d111-4ebd-b683-142636847c2b"",
                    ""path"": ""<XRController>/deviceRotation/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Oculus VR"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""b3c6be88-0750-4dda-bcd5-946692dfa38b"",
                    ""path"": ""<XRController>/deviceRotation/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Oculus VR"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""711c0bbb-b743-44e5-9cab-5826fa81204f"",
                    ""path"": ""<Keyboard>/leftAlt"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Free look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""80adaf3a-054c-4d03-ab15-fa0d9b66ca9f"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d341f3a6-3d39-44c6-81f5-e639fcb82600"",
                    ""path"": ""<Gamepad>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f972a3fb-661a-48ad-b089-25dbdeb72f7c"",
                    ""path"": ""<OculusTouchController>{LeftHand}/thumbstickClicked"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Oculus VR"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""HUD"",
            ""id"": ""2769241e-a3a0-43e9-aece-9ec667283d68"",
            ""actions"": [
                {
                    ""name"": ""OpenMenu"",
                    ""type"": ""Button"",
                    ""id"": ""1f712a28-fbd2-44cb-a173-9e2ae1b7f9a6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""656a69d7-7ab5-4fd9-988d-6d8dddda7bc6"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OpenMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b8591383-0cd8-4adc-b737-5e59c7ae1431"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""OpenMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""867c2690-cd5f-4687-8a1c-822035f45497"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""OpenMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3ad38b6d-2b13-4184-a542-2ac35b3964a5"",
                    ""path"": ""<OculusTouchController>{LeftHand}/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Oculus VR"",
                    ""action"": ""OpenMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard and mouse"",
            ""bindingGroup"": ""Keyboard and mouse"",
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
        },
        {
            ""name"": ""Oculus VR"",
            ""bindingGroup"": ""Oculus VR"",
            ""devices"": []
        }
    ]
}");
        // BasicMovements
        m_BasicMovements = asset.FindActionMap("BasicMovements", throwIfNotFound: true);
        m_BasicMovements_Movement = m_BasicMovements.FindAction("Movement", throwIfNotFound: true);
        m_BasicMovements_Look = m_BasicMovements.FindAction("Look", throwIfNotFound: true);
        m_BasicMovements_Freelook = m_BasicMovements.FindAction("Free look", throwIfNotFound: true);
        m_BasicMovements_Sprint = m_BasicMovements.FindAction("Sprint", throwIfNotFound: true);
        // HUD
        m_HUD = asset.FindActionMap("HUD", throwIfNotFound: true);
        m_HUD_OpenMenu = m_HUD.FindAction("OpenMenu", throwIfNotFound: true);
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

    // BasicMovements
    private readonly InputActionMap m_BasicMovements;
    private IBasicMovementsActions m_BasicMovementsActionsCallbackInterface;
    private readonly InputAction m_BasicMovements_Movement;
    private readonly InputAction m_BasicMovements_Look;
    private readonly InputAction m_BasicMovements_Freelook;
    private readonly InputAction m_BasicMovements_Sprint;
    public struct BasicMovementsActions
    {
        private @InputController m_Wrapper;
        public BasicMovementsActions(@InputController wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_BasicMovements_Movement;
        public InputAction @Look => m_Wrapper.m_BasicMovements_Look;
        public InputAction @Freelook => m_Wrapper.m_BasicMovements_Freelook;
        public InputAction @Sprint => m_Wrapper.m_BasicMovements_Sprint;
        public InputActionMap Get() { return m_Wrapper.m_BasicMovements; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BasicMovementsActions set) { return set.Get(); }
        public void SetCallbacks(IBasicMovementsActions instance)
        {
            if (m_Wrapper.m_BasicMovementsActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_BasicMovementsActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_BasicMovementsActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_BasicMovementsActionsCallbackInterface.OnMovement;
                @Look.started -= m_Wrapper.m_BasicMovementsActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_BasicMovementsActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_BasicMovementsActionsCallbackInterface.OnLook;
                @Freelook.started -= m_Wrapper.m_BasicMovementsActionsCallbackInterface.OnFreelook;
                @Freelook.performed -= m_Wrapper.m_BasicMovementsActionsCallbackInterface.OnFreelook;
                @Freelook.canceled -= m_Wrapper.m_BasicMovementsActionsCallbackInterface.OnFreelook;
                @Sprint.started -= m_Wrapper.m_BasicMovementsActionsCallbackInterface.OnSprint;
                @Sprint.performed -= m_Wrapper.m_BasicMovementsActionsCallbackInterface.OnSprint;
                @Sprint.canceled -= m_Wrapper.m_BasicMovementsActionsCallbackInterface.OnSprint;
            }
            m_Wrapper.m_BasicMovementsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Freelook.started += instance.OnFreelook;
                @Freelook.performed += instance.OnFreelook;
                @Freelook.canceled += instance.OnFreelook;
                @Sprint.started += instance.OnSprint;
                @Sprint.performed += instance.OnSprint;
                @Sprint.canceled += instance.OnSprint;
            }
        }
    }
    public BasicMovementsActions @BasicMovements => new BasicMovementsActions(this);

    // HUD
    private readonly InputActionMap m_HUD;
    private IHUDActions m_HUDActionsCallbackInterface;
    private readonly InputAction m_HUD_OpenMenu;
    public struct HUDActions
    {
        private @InputController m_Wrapper;
        public HUDActions(@InputController wrapper) { m_Wrapper = wrapper; }
        public InputAction @OpenMenu => m_Wrapper.m_HUD_OpenMenu;
        public InputActionMap Get() { return m_Wrapper.m_HUD; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(HUDActions set) { return set.Get(); }
        public void SetCallbacks(IHUDActions instance)
        {
            if (m_Wrapper.m_HUDActionsCallbackInterface != null)
            {
                @OpenMenu.started -= m_Wrapper.m_HUDActionsCallbackInterface.OnOpenMenu;
                @OpenMenu.performed -= m_Wrapper.m_HUDActionsCallbackInterface.OnOpenMenu;
                @OpenMenu.canceled -= m_Wrapper.m_HUDActionsCallbackInterface.OnOpenMenu;
            }
            m_Wrapper.m_HUDActionsCallbackInterface = instance;
            if (instance != null)
            {
                @OpenMenu.started += instance.OnOpenMenu;
                @OpenMenu.performed += instance.OnOpenMenu;
                @OpenMenu.canceled += instance.OnOpenMenu;
            }
        }
    }
    public HUDActions @HUD => new HUDActions(this);
    private int m_KeyboardandmouseSchemeIndex = -1;
    public InputControlScheme KeyboardandmouseScheme
    {
        get
        {
            if (m_KeyboardandmouseSchemeIndex == -1) m_KeyboardandmouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard and mouse");
            return asset.controlSchemes[m_KeyboardandmouseSchemeIndex];
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
    private int m_OculusVRSchemeIndex = -1;
    public InputControlScheme OculusVRScheme
    {
        get
        {
            if (m_OculusVRSchemeIndex == -1) m_OculusVRSchemeIndex = asset.FindControlSchemeIndex("Oculus VR");
            return asset.controlSchemes[m_OculusVRSchemeIndex];
        }
    }
    public interface IBasicMovementsActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnFreelook(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
    }
    public interface IHUDActions
    {
        void OnOpenMenu(InputAction.CallbackContext context);
    }
}
