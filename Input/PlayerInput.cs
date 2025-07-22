using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    PhoneActions _inputActions;

    //movement Axis
    Vector2 Axis => _inputActions.Gameplay.Movement.ReadValue<Vector2>();

    //Jump buffer
    public bool HasJumpInputBuffer { get; set; }

    //Jump
    public bool Jump => _inputActions.Gameplay.Jump.WasPerformedThisFrame();
    public bool StopJump => _inputActions.Gameplay.Jump.WasReleasedThisFrame();

    // run
    public bool Run => _inputActions.Gameplay.Run.IsPressed();
    public bool StopRun => _inputActions.Gameplay.Run.WasReleasedThisFrame();

    //dash
    public bool Dash => _inputActions.Gameplay.Dash.WasPerformedThisFrame();

    // Light
    public bool Light => _inputActions.Gameplay.Light.IsPressed();
    public bool StopLight => _inputActions.Gameplay.Light.WasReleasedThisFrame();

    // Wave
    public bool Wave => _inputActions.Gameplay.Wave.IsPressed();
    public bool StopWave => _inputActions.Gameplay.Wave.WasReleasedThisFrame();



    // attack
    public bool Attack => _inputActions.Gameplay.Attack.WasPressedThisFrame();
    public bool StopAttack => _inputActions.Gameplay.Attack.WasReleasedThisFrame();

    //movement
    public bool Move => AxisX != 0f;

    //X Axis movement
    public float AxisX => Axis.x;

    private void Awake()
    {
        //_inputActions = new PhoneActions();

    }

    void OnEnable()
    {
        _inputActions.Gameplay.Jump.canceled += delegate
        {
            HasJumpInputBuffer = false;
        };
    }

    public void EnableInputActions()
    {
        _inputActions.Gameplay.Enable();
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void DisableInputActions()
    {
        _inputActions.Gameplay.Disable();
        Cursor.lockState = CursorLockMode.None;
    }
    void AttackPreformed(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }
    void AttackCanceld(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }
}
