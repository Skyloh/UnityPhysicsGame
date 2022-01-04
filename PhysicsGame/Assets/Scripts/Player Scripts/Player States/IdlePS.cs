using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IdlePS : PlayerState
{
    public IdlePS(Vector2 c, Transform t, Rigidbody r) : base(c, t, r)
    {
        StateID = 0;
    }

    public override void InFixedUpdate()
    {
    }

    public override void WASD(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            StateLibrary.library.PlayerStateMachine.SwapState("MovementPS");
        }
    }
}
