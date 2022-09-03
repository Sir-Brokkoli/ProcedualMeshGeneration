using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerManager playerManager;
    private PlayerLocomotion playerLocomotion;
    private PlayerInputActions inputActions;

    public bool sprint_Input;
    public bool jump_Input;
    public bool fire_Input_Down;
    public bool fire_Input_Held;
    public bool fire_Input_Released;
    public bool aim_Input;

    [SerializeField]
    private Vector2 movementInput;
    [SerializeField]
    private Vector2 cameraInput;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void Update()
    {
        HandleMoveInput();
    }

    private void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerInputActions();
            Initialize();
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Initialize()
    {
        print("Initialize Player Input Handler");
        inputActions.Locomotion.Movement.performed += i => print("Move");//movementInput = i.ReadValue<Vector2>();
        inputActions.Locomotion.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
    }

    public void HandleMoveInput()
    {
        playerLocomotion.HandleMovement(movementInput);
    }

    public void HandleCameraInput()
    {

    }
}
