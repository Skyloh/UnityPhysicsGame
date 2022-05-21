using UnityEngine;
using UnityEngine.InputSystem;

public class PrejumpPS : PlayerState
{

    int frame_timer = 0; // tracks the frame count the player has been in prejump for

    float initial_velo = 0f; // used to store the velo going into this state so it can be applied going out.

    // KNOWN *BUG*

    // if you enter a movement state and IMMEDIATELY exit it by jumping, you gain more speed initially
    // than if you ran for a bit then jumped. I kinda like this, but it is technically a bug and
    // doesn't make physical sense
    // 
    // but since when did i care about that

    public PrejumpPS(Vector2 c, Transform t, Rigidbody r) : base(c, t, r)
    {
        StateID = 3;
    }

    // this is a jumpsquat state, so many things are disabled.

    public override void StateStart()
    {
        initial_velo = getXZVelo();
        
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
            Vector3 movement = transform.right * current_input.x + transform.forward * current_input.y; // i allow more lateral movement here (no 0.5 modifier). why?

            rbody.AddForce(Vector3.up * JUMP_FORCE + movement * initial_velo, ForceMode.Impulse); // WOOOOO FORCEMODE.IMPULSEEEEE

            StateLibrary.library.PlayerStateMachine.SwapState("AirbornePS"); // since we dont need to check for isGrounded, we manually swap to airborne.
        }
    }
}
