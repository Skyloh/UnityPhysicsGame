using UnityEngine;
using UnityEngine.InputSystem;

public class AirbornePS : PlayerState
{
    public AirbornePS(Vector2 c, Transform t, Rigidbody r) : base(c, t, r)
    {
        StateID = 2;
    }

    public override void InFixedUpdate()
    {
        if (isGrounded())
        {
            StateLibrary.library.PlayerStateMachine.SwapState("MovementPS");
            /*
            if (isKeyDown)
            {
                StateLibrary.library.PlayerStateMachine.SwapState("MovementPS");

                return;
            }

            StateLibrary.library.PlayerStateMachine.SwapState("IdlePS");
            */
        }

        if (!(rbody.velocity.magnitude > MAX_VELO))
        {
            Vector3 movement = transform.right * current_input.x * 0.5f + transform.forward * current_input.y;

            rbody.AddForce(movement * MOVE_SPEED * 0.25f * Time.deltaTime);
        }

    }

    public override void Jump(InputAction.CallbackContext context)
    {
        // remove implementation
    }
}
