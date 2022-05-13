using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WallGrabPS : PlayerState
{
    public WallGrabPS(Vector2 current_input, Transform player, Rigidbody rbody) : base (current_input, player, rbody)
    {
        StateID = 5;
    }

    public override void InFixedUpdate()
    {
        // pass
    }

    public override void WASD(InputAction.CallbackContext context)
    {
        // pass
    }

    public override void Jump(InputAction.CallbackContext context)
    {
        if (allow_action && context.performed)
        {
            StateLibrary.library.PlayerStateMachine.SwapState("WallGetupPS");
        }
    }
    public override void Shift(InputAction.CallbackContext context)
    {
        if (allow_action && context.performed)
        {
            StateLibrary.library.PlayerStateMachine.SwapState("WallDropPS");
        }
    }
}

