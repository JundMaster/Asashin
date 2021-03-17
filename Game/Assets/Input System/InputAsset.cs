// GENERATED AUTOMATICALLY FROM 'Assets/Input System/InputAsset.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputAsset : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputAsset()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputAsset"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""9b293857-3cd4-4704-8de2-82884e82434a"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""44549ca5-34e9-404b-98f5-341f58903a8f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Camera"",
                    ""type"": ""PassThrough"",
                    ""id"": ""e3ab7f5d-3c77-4819-bbf0-1a56e10c697e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""2301109e-6ae8-4059-bdc3-3041efc5c1c6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Roll"",
                    ""type"": ""Button"",
                    ""id"": ""681cf73b-baed-44c5-b16f-87b1e3b4acdd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MeleeLightAttack"",
                    ""type"": ""Button"",
                    ""id"": ""f44a81a4-5247-4401-a2ef-e3a999208251"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MeleeStrongAttack"",
                    ""type"": ""Button"",
                    ""id"": ""82997286-c894-4325-85df-2f65c9eb2e2e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Target"",
                    ""type"": ""Button"",
                    ""id"": ""74ee5ea7-3319-4eb6-853d-56147609d105"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ChangeTargetLeft"",
                    ""type"": ""Button"",
                    ""id"": ""65e138df-cbe3-4973-898b-1223c202c938"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ChangeTargetRight"",
                    ""type"": ""Button"",
                    ""id"": ""0c3642c7-afb0-472d-b3b1-d3b593b889a2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""UseItem"",
                    ""type"": ""Button"",
                    ""id"": ""7be16984-a0d5-4af4-9d3d-e0faf0f197b6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ChangeItemNext"",
                    ""type"": ""Button"",
                    ""id"": ""26d5e81d-047a-407d-ada4-b47ae94696f2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ChangeItemBefore"",
                    ""type"": ""Button"",
                    ""id"": ""8f5a4c24-7251-4f03-a78e-d07b8073fb2d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PauseGame"",
                    ""type"": ""Button"",
                    ""id"": ""985bc3e8-d2f7-49f3-9bba-282c76986550"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Block"",
                    ""type"": ""Button"",
                    ""id"": ""2fb1ce9f-efc9-411a-878f-0c430e905b49"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Walk"",
                    ""type"": ""Button"",
                    ""id"": ""a0d13d58-fe16-4aba-87ea-6b3052d22ba1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""65e46497-20f2-472c-bc67-f41e9a0d717e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""2d242454-b56d-4007-8ee0-4cd0018fac8b"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""7b6e37ed-6b36-4e12-858b-55cf7d0a7910"",
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
                    ""id"": ""664e4298-1805-4e23-b809-75a78a263672"",
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
                    ""id"": ""9c2941e5-ac7f-4430-81df-81194d7fb33c"",
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
                    ""id"": ""af332eb2-938a-4b35-816c-7cd681332af7"",
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
                    ""id"": ""8f96b064-de01-4742-87fb-8580a497eae0"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dd3fcc40-a8cd-4853-9aa2-b9a2d3409e0d"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8490b7bb-5241-43ab-b8bc-3d5b8b8a2ca9"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6bd5a6c4-6971-4713-a5e1-e1507359237e"",
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
                    ""id"": ""820fefe4-025d-40de-8aa8-3f39b8eb7aae"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5207432b-27ed-4bbb-8abd-ff4108b5f130"",
                    ""path"": ""<Keyboard>/leftAlt"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""77b07661-ffbc-4ebf-92e3-f6c640a0ce99"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3d31c0b8-2353-4a3a-a708-46272ffdf3e2"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MeleeLightAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""43975de7-2cd8-418b-af16-b34ae8cee816"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MeleeLightAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b68f94b1-5091-4118-853d-7580a4c7f611"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MeleeStrongAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4bf42146-e3c5-4815-a6da-ad91707abc22"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MeleeStrongAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7c4e81f6-db08-41b2-a4bf-5ee010778adb"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Target"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""abd76c28-cc9e-46cc-841d-3445f88c5a1f"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Target"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""412be4d3-612a-485a-bd5f-331adeac05bd"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeTargetLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bc66ac04-017f-4a80-8913-cd354e457b3d"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeTargetLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0ca08a31-fd4a-4e75-b7d5-a8fad91c4842"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeTargetRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7c4b1d47-ddca-4eb8-a024-81c50dc8ecb6"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeTargetRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c7a46ead-a177-47af-b3bb-3635a31c7d90"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UseItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""59a059d2-0a8b-4aa7-a845-26e3cd43d480"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UseItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f450a62e-b955-4b76-8d2d-79eeeb7f0b6f"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeItemNext"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2590269c-4653-4ba5-9f3e-414be48eb2cd"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeItemNext"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""449da060-96f1-4e9e-9678-c8b65d50a78b"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeItemBefore"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""44743b10-4548-452e-abea-ddad3ccbc504"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeItemBefore"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7bf568ce-ac42-48a0-9e0f-d0339176cc46"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PauseGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7b790849-9903-4ebd-92a0-061bafc28d5e"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PauseGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9dff0e23-a906-48d1-be5a-97c707caec69"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Block"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4ba93bed-25ff-40f1-b53a-738ee03cf7b2"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Block"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5a19b08e-9aec-43bf-ad7d-2fbbf5be6fab"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aa2e4f7c-a712-480a-bcab-d5705494aeb2"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b95aed07-181d-47af-a377-c73508d3148c"",
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
                    ""id"": ""a2dead6f-944d-46ec-a8a9-92b5c2b6840b"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""GamePaused"",
            ""id"": ""29252d06-4802-4e84-99e9-98ab3fb5cc76"",
            ""actions"": [
                {
                    ""name"": ""UnpauseGame"",
                    ""type"": ""Button"",
                    ""id"": ""7e0c9cd2-e1bb-46ef-b341-d2422b3531ce"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""201fade6-100a-4d1f-9965-b91986b7e489"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UnpauseGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e6be8580-269f-4113-9493-ce03942f57eb"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UnpauseGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_Movement = m_Gameplay.FindAction("Movement", throwIfNotFound: true);
        m_Gameplay_Camera = m_Gameplay.FindAction("Camera", throwIfNotFound: true);
        m_Gameplay_Jump = m_Gameplay.FindAction("Jump", throwIfNotFound: true);
        m_Gameplay_Roll = m_Gameplay.FindAction("Roll", throwIfNotFound: true);
        m_Gameplay_MeleeLightAttack = m_Gameplay.FindAction("MeleeLightAttack", throwIfNotFound: true);
        m_Gameplay_MeleeStrongAttack = m_Gameplay.FindAction("MeleeStrongAttack", throwIfNotFound: true);
        m_Gameplay_Target = m_Gameplay.FindAction("Target", throwIfNotFound: true);
        m_Gameplay_ChangeTargetLeft = m_Gameplay.FindAction("ChangeTargetLeft", throwIfNotFound: true);
        m_Gameplay_ChangeTargetRight = m_Gameplay.FindAction("ChangeTargetRight", throwIfNotFound: true);
        m_Gameplay_UseItem = m_Gameplay.FindAction("UseItem", throwIfNotFound: true);
        m_Gameplay_ChangeItemNext = m_Gameplay.FindAction("ChangeItemNext", throwIfNotFound: true);
        m_Gameplay_ChangeItemBefore = m_Gameplay.FindAction("ChangeItemBefore", throwIfNotFound: true);
        m_Gameplay_PauseGame = m_Gameplay.FindAction("PauseGame", throwIfNotFound: true);
        m_Gameplay_Block = m_Gameplay.FindAction("Block", throwIfNotFound: true);
        m_Gameplay_Walk = m_Gameplay.FindAction("Walk", throwIfNotFound: true);
        m_Gameplay_Sprint = m_Gameplay.FindAction("Sprint", throwIfNotFound: true);
        // GamePaused
        m_GamePaused = asset.FindActionMap("GamePaused", throwIfNotFound: true);
        m_GamePaused_UnpauseGame = m_GamePaused.FindAction("UnpauseGame", throwIfNotFound: true);
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

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_Movement;
    private readonly InputAction m_Gameplay_Camera;
    private readonly InputAction m_Gameplay_Jump;
    private readonly InputAction m_Gameplay_Roll;
    private readonly InputAction m_Gameplay_MeleeLightAttack;
    private readonly InputAction m_Gameplay_MeleeStrongAttack;
    private readonly InputAction m_Gameplay_Target;
    private readonly InputAction m_Gameplay_ChangeTargetLeft;
    private readonly InputAction m_Gameplay_ChangeTargetRight;
    private readonly InputAction m_Gameplay_UseItem;
    private readonly InputAction m_Gameplay_ChangeItemNext;
    private readonly InputAction m_Gameplay_ChangeItemBefore;
    private readonly InputAction m_Gameplay_PauseGame;
    private readonly InputAction m_Gameplay_Block;
    private readonly InputAction m_Gameplay_Walk;
    private readonly InputAction m_Gameplay_Sprint;
    public struct GameplayActions
    {
        private @InputAsset m_Wrapper;
        public GameplayActions(@InputAsset wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Gameplay_Movement;
        public InputAction @Camera => m_Wrapper.m_Gameplay_Camera;
        public InputAction @Jump => m_Wrapper.m_Gameplay_Jump;
        public InputAction @Roll => m_Wrapper.m_Gameplay_Roll;
        public InputAction @MeleeLightAttack => m_Wrapper.m_Gameplay_MeleeLightAttack;
        public InputAction @MeleeStrongAttack => m_Wrapper.m_Gameplay_MeleeStrongAttack;
        public InputAction @Target => m_Wrapper.m_Gameplay_Target;
        public InputAction @ChangeTargetLeft => m_Wrapper.m_Gameplay_ChangeTargetLeft;
        public InputAction @ChangeTargetRight => m_Wrapper.m_Gameplay_ChangeTargetRight;
        public InputAction @UseItem => m_Wrapper.m_Gameplay_UseItem;
        public InputAction @ChangeItemNext => m_Wrapper.m_Gameplay_ChangeItemNext;
        public InputAction @ChangeItemBefore => m_Wrapper.m_Gameplay_ChangeItemBefore;
        public InputAction @PauseGame => m_Wrapper.m_Gameplay_PauseGame;
        public InputAction @Block => m_Wrapper.m_Gameplay_Block;
        public InputAction @Walk => m_Wrapper.m_Gameplay_Walk;
        public InputAction @Sprint => m_Wrapper.m_Gameplay_Sprint;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                @Camera.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCamera;
                @Camera.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCamera;
                @Camera.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCamera;
                @Jump.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Roll.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRoll;
                @Roll.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRoll;
                @Roll.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRoll;
                @MeleeLightAttack.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMeleeLightAttack;
                @MeleeLightAttack.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMeleeLightAttack;
                @MeleeLightAttack.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMeleeLightAttack;
                @MeleeStrongAttack.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMeleeStrongAttack;
                @MeleeStrongAttack.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMeleeStrongAttack;
                @MeleeStrongAttack.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMeleeStrongAttack;
                @Target.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnTarget;
                @Target.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnTarget;
                @Target.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnTarget;
                @ChangeTargetLeft.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnChangeTargetLeft;
                @ChangeTargetLeft.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnChangeTargetLeft;
                @ChangeTargetLeft.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnChangeTargetLeft;
                @ChangeTargetRight.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnChangeTargetRight;
                @ChangeTargetRight.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnChangeTargetRight;
                @ChangeTargetRight.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnChangeTargetRight;
                @UseItem.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnUseItem;
                @UseItem.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnUseItem;
                @UseItem.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnUseItem;
                @ChangeItemNext.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnChangeItemNext;
                @ChangeItemNext.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnChangeItemNext;
                @ChangeItemNext.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnChangeItemNext;
                @ChangeItemBefore.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnChangeItemBefore;
                @ChangeItemBefore.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnChangeItemBefore;
                @ChangeItemBefore.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnChangeItemBefore;
                @PauseGame.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPauseGame;
                @PauseGame.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPauseGame;
                @PauseGame.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPauseGame;
                @Block.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnBlock;
                @Block.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnBlock;
                @Block.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnBlock;
                @Walk.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnWalk;
                @Walk.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnWalk;
                @Walk.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnWalk;
                @Sprint.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSprint;
                @Sprint.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSprint;
                @Sprint.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSprint;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Camera.started += instance.OnCamera;
                @Camera.performed += instance.OnCamera;
                @Camera.canceled += instance.OnCamera;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Roll.started += instance.OnRoll;
                @Roll.performed += instance.OnRoll;
                @Roll.canceled += instance.OnRoll;
                @MeleeLightAttack.started += instance.OnMeleeLightAttack;
                @MeleeLightAttack.performed += instance.OnMeleeLightAttack;
                @MeleeLightAttack.canceled += instance.OnMeleeLightAttack;
                @MeleeStrongAttack.started += instance.OnMeleeStrongAttack;
                @MeleeStrongAttack.performed += instance.OnMeleeStrongAttack;
                @MeleeStrongAttack.canceled += instance.OnMeleeStrongAttack;
                @Target.started += instance.OnTarget;
                @Target.performed += instance.OnTarget;
                @Target.canceled += instance.OnTarget;
                @ChangeTargetLeft.started += instance.OnChangeTargetLeft;
                @ChangeTargetLeft.performed += instance.OnChangeTargetLeft;
                @ChangeTargetLeft.canceled += instance.OnChangeTargetLeft;
                @ChangeTargetRight.started += instance.OnChangeTargetRight;
                @ChangeTargetRight.performed += instance.OnChangeTargetRight;
                @ChangeTargetRight.canceled += instance.OnChangeTargetRight;
                @UseItem.started += instance.OnUseItem;
                @UseItem.performed += instance.OnUseItem;
                @UseItem.canceled += instance.OnUseItem;
                @ChangeItemNext.started += instance.OnChangeItemNext;
                @ChangeItemNext.performed += instance.OnChangeItemNext;
                @ChangeItemNext.canceled += instance.OnChangeItemNext;
                @ChangeItemBefore.started += instance.OnChangeItemBefore;
                @ChangeItemBefore.performed += instance.OnChangeItemBefore;
                @ChangeItemBefore.canceled += instance.OnChangeItemBefore;
                @PauseGame.started += instance.OnPauseGame;
                @PauseGame.performed += instance.OnPauseGame;
                @PauseGame.canceled += instance.OnPauseGame;
                @Block.started += instance.OnBlock;
                @Block.performed += instance.OnBlock;
                @Block.canceled += instance.OnBlock;
                @Walk.started += instance.OnWalk;
                @Walk.performed += instance.OnWalk;
                @Walk.canceled += instance.OnWalk;
                @Sprint.started += instance.OnSprint;
                @Sprint.performed += instance.OnSprint;
                @Sprint.canceled += instance.OnSprint;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);

    // GamePaused
    private readonly InputActionMap m_GamePaused;
    private IGamePausedActions m_GamePausedActionsCallbackInterface;
    private readonly InputAction m_GamePaused_UnpauseGame;
    public struct GamePausedActions
    {
        private @InputAsset m_Wrapper;
        public GamePausedActions(@InputAsset wrapper) { m_Wrapper = wrapper; }
        public InputAction @UnpauseGame => m_Wrapper.m_GamePaused_UnpauseGame;
        public InputActionMap Get() { return m_Wrapper.m_GamePaused; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GamePausedActions set) { return set.Get(); }
        public void SetCallbacks(IGamePausedActions instance)
        {
            if (m_Wrapper.m_GamePausedActionsCallbackInterface != null)
            {
                @UnpauseGame.started -= m_Wrapper.m_GamePausedActionsCallbackInterface.OnUnpauseGame;
                @UnpauseGame.performed -= m_Wrapper.m_GamePausedActionsCallbackInterface.OnUnpauseGame;
                @UnpauseGame.canceled -= m_Wrapper.m_GamePausedActionsCallbackInterface.OnUnpauseGame;
            }
            m_Wrapper.m_GamePausedActionsCallbackInterface = instance;
            if (instance != null)
            {
                @UnpauseGame.started += instance.OnUnpauseGame;
                @UnpauseGame.performed += instance.OnUnpauseGame;
                @UnpauseGame.canceled += instance.OnUnpauseGame;
            }
        }
    }
    public GamePausedActions @GamePaused => new GamePausedActions(this);
    public interface IGameplayActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnCamera(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnRoll(InputAction.CallbackContext context);
        void OnMeleeLightAttack(InputAction.CallbackContext context);
        void OnMeleeStrongAttack(InputAction.CallbackContext context);
        void OnTarget(InputAction.CallbackContext context);
        void OnChangeTargetLeft(InputAction.CallbackContext context);
        void OnChangeTargetRight(InputAction.CallbackContext context);
        void OnUseItem(InputAction.CallbackContext context);
        void OnChangeItemNext(InputAction.CallbackContext context);
        void OnChangeItemBefore(InputAction.CallbackContext context);
        void OnPauseGame(InputAction.CallbackContext context);
        void OnBlock(InputAction.CallbackContext context);
        void OnWalk(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
    }
    public interface IGamePausedActions
    {
        void OnUnpauseGame(InputAction.CallbackContext context);
    }
}
