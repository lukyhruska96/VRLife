using Assets.Scripts.Core.Utils;
using Assets.Scripts.Core.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Assets.Scripts.ElementScripts.Room
{
    public class PlayerControls : MonoBehaviour
    {
        private static readonly Vector3 headMin = new Vector3(-90f, -30f, -30f);
        private static readonly Vector3 headMax = new Vector3(90f, 90f, 90f);

        public float lookSensitivity = 0.01f;
        public Animator animator;

        private InputController controller;
        private static PlayerControls current;
        private IAvatar _avatar;

        private bool _freeLook = false;
        private Vector3 _headAngles;

        public void Awake()
        {
            current = this;
            controller = new InputController();
        }

        public void Start()
        {
            controller.BasicMovements.Look.performed += OnLook;
            controller.BasicMovements.Movement.performed += OnMove;
            controller.BasicMovements.Movement.canceled += OnMove;
            controller.BasicMovements.Freelook.performed += OnFreeLook;
            controller.BasicMovements.Freelook.canceled += OnFreeLookStop;
            controller.BasicMovements.Sprint.performed += OnSprint;
            controller.BasicMovements.Sprint.canceled += OnSprint;
            controller.HUD.OpenMenu.performed += OnOpenMenu;
            _avatar = GetComponent<PlayerState>().Avatar;
            _headAngles = _avatar.GetHead().transform.localRotation.eulerAngles;
        }

        public void OnEnable()
        {
            controller.Enable();
        }

        public void OnDisable()
        {
            controller.Disable();
        }

        public void Update()
        {
            
        }

        public void LateUpdate()
        {
            if(_avatar.GetHead() != null && _headAngles != null)
            {
                _avatar.GetHead().transform.localRotation = Quaternion.Euler(_headAngles);
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
            Debug.Log(_headAngles);
        }

        public void OnMove(CallbackContext ctx)
        {
            Vector2 direction = ctx.ReadValue<Vector2>();
            Debug.Log("OnMove");
            Debug.Log(direction);
            animator.SetFloat("vertical", direction.y);
            animator.SetFloat("horizontal", direction.x);
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
    }
}
