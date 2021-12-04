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

        Vector3 movement = transform.right * current_input.x + transform.forward * current_input.y;

        if (!(rbody.velocity.magnitude > MAX_VELO))
        {
            rbody.AddForce(movement * MOVE_SPEED * StateMultiplier());
        }
    }

    public override void WASD(InputAction.CallbackContext context)
    {
        // remove implementation
    }

    public override void Jump(InputAction.CallbackContext context)
    {
        // remove implementation
    }
}
