using UnityEngine;
using UnityEngine.InputSystem;

public class AirbornePS : PlayerState
{
    public AirbornePS(Vector2 c, Transform t, Rigidbody r) : base(c, t, r)
    {

    }

    public override void InFixedUpdate()
    {
        if (isGrounded())
        {
            StateLibrary.library.PlayerStateMachine.SwapState("MovementPS");
        }

        Vector3 movement = transform.right * current_input.x * 0.5f + transform.forward * current_input.y;

        rbody.AddForce(movement * MOVE_SPEED * 0.25f);

    }

    public override void Jump(InputAction.CallbackContext context)
    {
        // remove implementation
    }
}
