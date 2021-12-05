using UnityEngine;
using UnityEngine.InputSystem;

public class MovementPS : PlayerState
{
    Transform cross_vector;

    public MovementPS(Vector2 c, Transform t, Rigidbody r, Transform l) : base(c, t, r)
    {
        cross_vector = l;
    }

    public override void WASD(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            current_input = context.ReadValue<Vector2>();
            rbody.useGravity = true;
        }

        else if (context.canceled)
        {
            current_input = Vector2.zero;

            rbody.useGravity = false;
        }
    }

    public override void StateExit(PlayerState next_state)
    {
        rbody.useGravity = true;

        base.StateExit(next_state);
    }


}
