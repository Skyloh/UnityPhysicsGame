using UnityEngine;
using UnityEngine.InputSystem;

public class PrejumpPS : PlayerState
{

    int frame_timer = 0;

    float initial_velo = 0f;

    public PrejumpPS(Vector2 c, Transform t, Rigidbody r) : base(c, t, r)
    {
        StateID = 3;
    }

    // this is a jumpsquat state, so many things are disabled.

    public override void StateStart()
    {
        initial_velo = rbody.velocity.magnitude;
        
        frame_timer = 0;

        rbody.isKinematic = true; // STOP MOVING PLEASE
    }

    public override void Jump(InputAction.CallbackContext context)
    {
        // remove implementation
    }

    // Lclick and Rclick are disabled

    public override void InFixedUpdate()
    {
        frame_timer++;

        if (frame_timer > PREJUMP_DURATION)
        {
            rbody.isKinematic = false;

            Vector3 movement = transform.right * current_input.x + transform.forward * current_input.y;

            rbody.AddForce(Vector3.up * JUMP_FORCE + movement * (initial_velo + 2), ForceMode.Impulse);

            StateLibrary.library.PlayerStateMachine.SwapState("AirbornePS"); // since we dont check for isGrounded, we manually swap to airborne.
        }
    }
}
