using UnityEngine;
using UnityEngine.InputSystem;

public class WallGrabPS : PlayerState
{
    // a notorious "cinematic" state.
    // wish i could've remade the wall-suite of states into one that inherited from a much simpler
    // parent state because so much of the implementation is removed but shhhh

    // the main gimmick with these states is that the key toggle is not on the script.
    // the key toggle is on an Event attached to the state's respective animation.
    // go to the Player model in the Project tab and manuever to animations.
    // each one has a timeline hidden under the "Events" tab.
    // sorry

    Transform perspective;
    Transform overlay;

    public WallGrabPS(Vector2 current_input, Transform player, Rigidbody rbody) : base (current_input, player, rbody)
    {
        StateID = 5;

        // unfortunate foreach :(
        foreach (Camera cam in Camera.allCameras)
        {
            if (cam.tag == "PlayerCamera")
            {
                perspective = cam.transform;

                return;
            }
        }

        // we can't assign overlay camera here because the scene hasnt loaded by the time this is called
        // i refuse to hook this up to a delegate too because that is WAY too much spaghetti code.
    }

    public override void StateStart()
    {
        // my solution to the overlay camera scene thingy.
        // pretty okay
        if (overlay == null)
        {
            overlay = Camera.main.transform;
        }

        // *retch*
        allow_action = false;
        allow_rotation = false;

        // no gravity or any residual forces
        rbody.isKinematic = true;

        RotateCamerasForState(1f);
    }

    // given a direction of movement (parity), rotates the cameras to give the view of looking upwards.
    private void RotateCamerasForState(float parity)
    {
        // I *would* make this a lerp, but i dont have time and it's fine for now
        perspective.eulerAngles += Vector3.left * 45f * parity;
        overlay.eulerAngles += Vector3.left * 55f * parity;
    }

    public override void StateExit(PlayerState next_state)
    {
        allow_rotation = true;
        allow_action = true; // just in case

        rbody.isKinematic = false;

        RotateCamerasForState(-1f);

        base.StateExit(next_state);
    }

    public override void InFixedUpdate()
    {
        // pass
    }

    public override void WASD(InputAction.CallbackContext context)
    {
        // pass
    }

    // the two actions you can take in a wall-grab state: getting up, or dropping down
    public override void Jump(InputAction.CallbackContext context)
    {
        if (allow_action && context.performed)
        {
            StateLibrary.library.PlayerStateMachine.SwapState("WallGetupPS"); // not yet added, will give you an error
        }
    }
    public override void Shift(InputAction.CallbackContext context)
    {
        if (allow_action && context.performed)
        {
            // again, transforming the player due to animation quirks
            transform.position.Set(transform.position.x, transform.position.y - 2f, transform.position.y);
            StateLibrary.library.PlayerStateMachine.SwapState("WallDropPS");
        }
    }
}

