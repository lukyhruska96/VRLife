using Assets.Scripts.Core.Utils;
using System;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using VrLifeAPI;
using VrLifeAPI.Client.API;
using VrLifeAPI.Client.Applications.ObjectApp;
using VrLifeAPI.Client.Core.Character;
using VrLifeAPI.Client.Core.Wrappers;
using VrLifeClient;
using static UnityEngine.InputSystem.InputAction;

namespace Assets.Scripts.ElementScripts.Room
{
    public class PlayerControls : MonoBehaviour
    {
        private AppInfo _info = new AppInfo(ulong.MaxValue, "PlayerControls", null, 
            new AppVersion(new int[] { 1, 0, 0 }), AppType.APP_GLOBAL);
        private IClosedAPI _api;
        private IAvatar _avatar;
        public static PlayerControls current = null;
        public float speed = 0.5f;
        public float maxSpeed = 10;
        private static readonly Vector3 headMin = new Vector3(-90f, -30f, -30f);
        private static readonly Vector3 headMax = new Vector3(90f, 90f, 90f);

        public float lookSensitivity = 0.01f;
        public Animator animator;

        private InputController controller = null;

        private bool _freeLook = false;
        private Vector3 _headAngles;
        private Vector3 _lookingVector;
        private float _horizontalMovement = 0f;

        private IObjectAppInstance _currentInstance = null;

        private Vector3? _pointAt = null;

        public event Action<Vector3> Selected;

        public void Awake()
        {

        }

        public void Start()
        {
            _api = VrLifeCore.GetClosedAPI(_info);
            controller.BasicMovements.Look.performed += OnLook;
            controller.BasicMovements.Movement.performed += OnMove;
            controller.BasicMovements.Movement.canceled += OnMove;
            controller.BasicMovements.Freelook.performed += OnFreeLook;
            controller.BasicMovements.Freelook.canceled += OnFreeLookStop;
            controller.BasicMovements.Sprint.performed += OnSprint;
            controller.BasicMovements.Sprint.canceled += OnSprint;
            controller.BasicMovements.Select.performed += OnSelect;
            controller.HUD.OpenMenu.performed += OnOpenMenu;
            controller.ObjectAPI.ExitPlacing.performed += OnEscPressed;
            _avatar = _api.GlobalAPI.Players.GetMainAvatar();
            _headAngles = _avatar.GetHead().transform.localRotation.eulerAngles;
        }

        public void OnEnable()
        {
            current = this;
            if(controller == null)
            {
                controller = new InputController();
            }
            controller.Enable();
        }

        public void OnDisable()
        {
            controller.Disable();
        }

        public void LateUpdate()
        {
            if(_avatar.GetHead() != null && _headAngles != null)
            {
                _avatar.GetHead().transform.localRotation = Quaternion.Euler(_headAngles);
                _lookingVector = _avatar.GetHead().transform.up;
            }
            if(_pointAt.HasValue)
            {
                GameObject arm = _avatar.GetSkeletonParts()[(int)SkeletonEnum.R_ARM];
                arm.transform.localEulerAngles = Vector3.zero;
                GameObject upArm = _avatar.GetSkeletonParts()[(int)SkeletonEnum.R_UPPER_ARM];
                Vector3 currentVector = arm.transform.position - upArm.transform.position;
                Vector3 requiredVector = _pointAt.Value - upArm.transform.position;
                upArm.transform.rotation = Quaternion.FromToRotation(currentVector, requiredVector) * upArm.transform.rotation;
                arm.transform.localEulerAngles = new Vector3(140, 0, 0);
            }
        }

        public void Update()
        {
            if(_currentInstance != null)
            {
                Ray r = new Ray(_avatar.GetHead().transform.position, _lookingVector);
                _currentInstance.PlayerPointAt(_api.Services.User.UserId.Value, r);
            }
        }

        public Vector3 LookingAt()
        {
            return _lookingVector;
        }

        public void PointAt(Vector3? location)
        {
            _pointAt = location;
        }

        private void FixedUpdate()
        {
            Vector3 right = _avatar.GetSkeletonParts()[(int)SkeletonEnum.BODY_LOCATION].transform.right.normalized;
            Rigidbody rig = _avatar.GetGameObject().GetComponent<Rigidbody>();
            if (rig.velocity.sqrMagnitude < maxSpeed)
            {
                rig.AddForce(right * Time.deltaTime * speed * 1000 * _horizontalMovement);
            }
        }

        public void OnLook(CallbackContext ctx)
        {
            Vector2 delta = ctx.ReadValue<Vector2>();
            float headX = 0f;
            if(_freeLook)
            {
                headX = -delta.x;
            }
            else
            {
                _avatar.GetGameObject().transform.Rotate(new Vector3(0, delta.x, 0) * lookSensitivity);
            }
            Vector3 headRotate = new Vector3(headX, 0f, delta.y) * lookSensitivity;
            _headAngles = (_headAngles + headRotate).ClampAngles(headMin, headMax);
        }

        public void OnMove(CallbackContext ctx)
        {
            Vector2 direction = ctx.ReadValue<Vector2>();
            animator.SetFloat("vertical", direction.y);
            //animator.SetFloat("horizontal", direction.x);
            _horizontalMovement = direction.x;
        }

        public void OnFreeLook(CallbackContext ctx)
        {
            _freeLook = true;
        }
        public void OnFreeLookStop(CallbackContext ctx)
        {
            _freeLook = false;
            _headAngles.x = 0;
        }

        public void OnSprint(CallbackContext ctx)
        {
            bool val = ctx.ReadValueAsButton();
            animator.SetBool("run", ctx.ReadValueAsButton());
        }

        public void OnOpenMenu(CallbackContext ctx)
        {
            MenuControl.current.ExitMenu.AddListener(ExitMenu);
            controller.Disable();
            Cursor.visible = true;
            MenuControl.current.OpenMenu();
        }

        public void ExitMenu()
        {
            Cursor.visible = false;
            controller.Enable();
        }

        private void OnEscPressed(CallbackContext ctx)
        {
            _pointAt = null;
        }

        private void OnSelect(CallbackContext ctx)
        {
            if(_currentInstance != null)
            {
                Ray r = new Ray(_avatar.GetHead().transform.position, _lookingVector);
                _currentInstance.PlayerSelect(_api.Services.User.UserId.Value, r);
            }
            Selected?.Invoke(_lookingVector);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.layer == 8)
            {
                Vector3 pos = GetComponent<Rigidbody>().position;
                pos.y += 0.001f;
                GetComponent<Rigidbody>().position = pos;
                _currentInstance = collision.gameObject.GetComponent<ObjectAppInstanceHolder>()?.GetInstance();
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if(collision.gameObject.layer == 8)
            {
                _currentInstance = null;
            }
        }
    }
}
