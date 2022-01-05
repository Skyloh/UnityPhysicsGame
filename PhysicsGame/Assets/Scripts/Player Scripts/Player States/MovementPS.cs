using UnityEngine;
using UnityEngine.InputSystem;

public class MovementPS : PlayerState
{
    // Transform cross_vector;

    public MovementPS(Vector2 c, Transform t, Rigidbody r /*, Transform l*/ ) : base(c, t, r)
    {
        // cross_vector = l;
        StateID = 1;

        isKeyDown = true; // not needed bc of the thing in statestart but i wanna just be sure
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
    }

    public override void StateExit(PlayerState next_state)
    {
        rbody.useGravity = true;

        base.StateExit(next_state);
    }


}
