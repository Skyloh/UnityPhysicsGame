using UnityEngine;
using UnityEngine.InputSystem;

public class PrejumpPS : PlayerState
{

    int frame_timer = 0;

    public PrejumpPS(Vector2 c, Transform t, Rigidbody r) : base(c, t, r)
    {

    }

    // this is a jumpsquat state, so many things are disabled.

    public override void StateStart()
    {
        Debug.Log("In prejump timer...");
        frame_timer = 0;
    }

    public override void Jump(InputAction.CallbackContext context)
    {
        // remove implementation
    }

    // Lclick and Rclick are disabled

    public override void InFixedUpdate()
    {
        rbody.isKinematic = true; // STOP MOVING PLEASE

        frame_timer++;

        if (frame_timer > PREJUMP_DURATION)
        {
            rbody.isKinematic = false;

            rbody.AddForce(Vector3.up * JUMP_FORCE, ForceMode.Impulse);

            StateLibrary.library.PlayerStateMachine.SwapState("AirbornePS"); // since we dont check for isGrounded, we manually swap to airborne.
        }
    }
}
