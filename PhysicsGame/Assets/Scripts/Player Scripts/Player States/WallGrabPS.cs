﻿using UnityEngine;
using UnityEngine.InputSystem;

public class WallGrabPS : PlayerState
{
    public WallGrabPS(Vector2 current_input, Transform player, Rigidbody rbody) : base (current_input, player, rbody)
    {
        StateID = 5;
    }

    public override void StateStart()
    {
        allow_action = false;
        allow_rotation = false;

        rbody.isKinematic = true;

    }

    public override void StateExit(PlayerState next_state)
    {
        allow_rotation = true;
        allow_action = true; // just in case

        rbody.isKinematic = false;

        base.StateExit(next_state);
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
            Debug.Log("activated");
            StateLibrary.library.PlayerStateMachine.SwapState("WallDropPS");
        }
    }
}

