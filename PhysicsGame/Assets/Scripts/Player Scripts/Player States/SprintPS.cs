using UnityEngine;
using UnityEngine.InputSystem;

public class SprintPS : PlayerState
{

    public SprintPS(Vector2 c, Transform t, Rigidbody r ) : base(c, t, r)
    {
        StateID = 4;
    }

    public override void WASD(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isKeyDown = true;

            rbody.useGravity = true;
        }

        else if (context.performed)
        {
            current_input = context.ReadValue<Vector2>();

            isKeyDown = true;
        }

        else if (context.canceled)
        {
            current_input = Vector2.zero;

            isKeyDown = false;

            rbody.useGravity = false;
        }
    }

    public override void InFixedUpdate()
    {
        base.InFixedUpdate();

        if (!isKeyDown)
        {
            StateLibrary.library.PlayerStateMachine.SwapState("IdlePS");
        }
    }

    public override void StateStart()
    {
        isKeyDown = true;

        DASH_MULTIPLIER = 1.5f;
    }

    public override void StateExit(PlayerState next_state)
    {
        rbody.useGravity = true;

        DASH_MULTIPLIER = 1f;

        base.StateExit(next_state);
    }

    public override void Shift(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            isKeyDown = true;

            StateLibrary.library.PlayerStateMachine.SwapState("MovementPS");
        }
    }


}
