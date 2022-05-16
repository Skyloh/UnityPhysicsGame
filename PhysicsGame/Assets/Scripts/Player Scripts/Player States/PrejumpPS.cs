using UnityEngine;
using UnityEngine.InputSystem;

public class PrejumpPS : PlayerState
{

    int frame_timer = 0;

    float initial_velo = 0f;

    // KNOWN *BUG*

    // if you enter a movement state and IMMEDIATELY exit it by jumping, you gain more speed initially
    // than if you ran for a bit then jumped. I kinda like this, but it is technically a bug and
    // doesn't make physical sense.

    public PrejumpPS(Vector2 c, Transform t, Rigidbody r) : base(c, t, r)
    {
        StateID = 3;
    }

    // this is a jumpsquat state, so many things are disabled.

    public override void StateStart()
    {
        initial_velo = getXYVelo();
        
        frame_timer = 0;
    }

    public override void WASD(InputAction.CallbackContext context)
    {
        // pass
    }

    public override void Jump(InputAction.CallbackContext context)
    {
        // remove implementation
    }


    public override void InFixedUpdate()
    {
        frame_timer++;

        if (frame_timer > PREJUMP_DURATION)
        {
            // rbody.isKinematic = false;

            Vector3 movement = transform.right * current_input.x + transform.forward * current_input.y;

            rbody.AddForce(Vector3.up * JUMP_FORCE + movement * initial_velo, ForceMode.Impulse);

            StateLibrary.library.PlayerStateMachine.SwapState("AirbornePS"); // since we dont check for isGrounded, we manually swap to airborne.
        }
    }
}
