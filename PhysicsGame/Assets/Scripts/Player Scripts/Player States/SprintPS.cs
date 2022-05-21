using UnityEngine;
using UnityEngine.InputSystem;

public class SprintPS : MovementPS
{
    // movement and sprint are literally identical except for a few things
    // object oriented programming moment
    public SprintPS(Vector2 c, Transform t, Rigidbody r ) : base(c, t, r)
    {
        StateID = 4;
    }

    public override void StateStart()
    {
        isKeyDown = true;

        dash_multiplier = 1.5f; // this is always applied in the movement calculation, now we're just giving it a value.
    }

    public override void StateExit(PlayerState next_state)
    {
        dash_multiplier = 1f; // oops no more rights for you

        base.StateExit(next_state);
    }

    public override void Shift(InputAction.CallbackContext context)
    {
        // swapping directly to movement has a potential bug
        // if you let go of shift and WASD on the SAME FRAME (give or take a few updates)
        // you will be in the movement state animation but not moving. just a graphical thing.
        if (context.canceled)
        {
            StateLibrary.library.PlayerStateMachine.SwapState("MovementPS");
        }
    }


}
