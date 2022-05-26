using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonPressPS : PlayerState
{
    Transform overlay; // like WallGrabPS, because I didnt think this through :<

    public ButtonPressPS(Vector2 current_input, Transform player, Rigidbody rbody) : base(current_input, player, rbody)
    {
        StateID = 8;
    }

    public override void StateStart()
    {
        if (overlay == null)
        {
            overlay = Camera.main.transform;
        }

        allow_action = false;
        allow_rotation = false;

        rbody.isKinematic = true;

        overlay.eulerAngles = Vector3.right * 55f + Vector3.back * 25f; // desired rotation to make the arm visble and not at a weird incoming angle
        
    }

    public override void StateExit(PlayerState next_state)
    {
        allow_rotation = true;
        rbody.isKinematic = false;

        overlay.eulerAngles = Vector3.right * 10f; // 10f is the initial rotation of the viewport camera

        base.StateExit(next_state);
    }

    public override void InFixedUpdate()
    {
        if (allow_action)
        {
            isKeyDown = false;

            StateLibrary.library.PlayerStateMachine.SwapState("IdlePS");
        }
    }

    public override void WASD(InputAction.CallbackContext context)
    {
        // pass
    }

    public override void Jump(InputAction.CallbackContext context)
    {
        // pass
    }
    public override void Shift(InputAction.CallbackContext context)
    {
        // pass
    }
}
