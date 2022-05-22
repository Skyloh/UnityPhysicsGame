using UnityEngine;
using UnityEngine.InputSystem;

public class WallGetupPS : PlayerState
{
    // see WallGrabPS
    // even simpler, this is just an animation
    // one entrance, one exit.
    // although, it exists into Airborne because I felt that would be prudent :>

    public WallGetupPS(Vector2 current_input, Transform player, Rigidbody rbody) : base(current_input, player, rbody)
    {
        StateID = 7;
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
        rbody.isKinematic = false;

        base.StateExit(next_state);
    }

    public override void InFixedUpdate()
    {
        if (allow_action)
        {
            isGrounded();

            transform.position += transform.forward * 0.9f + transform.up * (1.95f - (GroundInfo.distance - 0.01f));

            isKeyDown = false;

            StateLibrary.library.PlayerStateMachine.SwapState("IdlePS");
        }
    }

    public override void WASD(InputAction.CallbackContext context)
    {
        // pass
    }

    public override void Jump(InputAction.CallbackContext context)
    {
        // pass
    }
    public override void Shift(InputAction.CallbackContext context)
    {
        // pass
    }
}
