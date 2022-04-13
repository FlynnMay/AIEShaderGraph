using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManagerTPS : MonoBehaviour
{
    // TPS inputs
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool aim;
    public bool shoot;
    public bool run;

    public bool movement;

    // mouse lockers
    public bool cursorLocked = true;
    public bool cursorInputLook = true;

    #region Functions We Call On Button Press

    public void MoveInput(Vector2 moveDir)
    {
        move = moveDir;
    }

    public void LookInput(Vector2 lookDir)
    {
        look = lookDir;
    }

    public void AimInput(bool aimState)
    {
        aim = aimState;
    }

    public void JumpInput(bool jumpState)
    {
        jump = jumpState;
    }

    public void ShootInput(bool shootState)
    {
        shoot = shootState;
    }

    public void RunInput(bool runState)
    {
        run = runState;
    }
    #endregion

    #region Functions Created For Connecting To Unity Input System

    public void OnMove(InputValue a_value)
    {
        MoveInput(a_value.Get<Vector2>());
    }

    public void OnLook(InputValue a_value)
    {
        if (cursorInputLook)
            LookInput(a_value.Get<Vector2>());
    }

    public void OnAim(InputValue a_value)
    {
        AimInput(a_value.isPressed);
    }

    public void OnJump(InputValue a_value)
    {
        JumpInput(a_value.isPressed);
    }

    public void OnShoot(InputValue a_value)
    {
        ShootInput(a_value.isPressed);
    }

    public void OnRun(InputValue a_value)
    {
        RunInput(a_value.isPressed);
    }
    #endregion

    void OnApplicationFocus(bool focus)
    {
        SetCursorState(cursorLocked);
    }

    void SetCursorState(bool state)
    {
        Cursor.lockState = state ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
