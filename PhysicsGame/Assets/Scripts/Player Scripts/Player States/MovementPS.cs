using UnityEngine;
using UnityEngine.InputSystem;

public class MovementPS : PlayerState
{
    // all of these are temporary and liable to change bc fuck slopes
    RaycastHit info; // raycast info on what the slope ray hit
    Vector3 up_direction; // the transform.up
    float dot; // dot product because declaring variables in Update is NO
    bool apply_slope; // see above
    float parity; // see above
    Vector3 adjustment; // idk lol

    public MovementPS(Vector2 c, Transform t, Rigidbody r) : base(c, t, r)
    {
        StateID = 1;

        up_direction = t.up; // cacche it CACHE IT
    }

    public override void WASD(InputAction.CallbackContext context)
    {
        // with isKeyDown, it's better to be safe than sorry
        // hence all the assignments

        if (context.started)
        {
            isKeyDown = true;
        }

        else if (context.performed)
        {
            current_input = context.ReadValue<Vector2>();

            isKeyDown = true;
        }

        else if (context.canceled)
        {
            current_input = Vector2.zero;

            isKeyDown = false;
        }
    }

    // sends out a ray to check for a slope, then adjusts the player's location instantly with .Translate
    // probably (definitely) a better way to do this, transform's Rotate and Translate are notoriously cringe and sensitive
    private void RaycastForIncline()
    {
        // 0.75 is just above the knees
        if (!Physics.Raycast(transform.position + transform.forward * 0.5f + Vector3.up * 0.75f, Vector3.down, out info, 1.5f))
        {
            return;
        }

        dot = Mathf.Abs(Vector3.Dot(up_direction, info.normal));

        apply_slope = (dot > MAX_INCLINE_DOT);

        raycast_offset = apply_slope ? dot * 0.75f : 0f;

        if (apply_slope)
        {
            parity = info.distance > 1f ? Mathf.Pow(dot, 2f) * -0.55f : 1f; // WORK HERE, THIS 0.55 COEFF CANT BE HARDCODED

            transform.Translate(parity * (up_direction * info.normal.y) * (1f - dot));
        }

    }

    public override void InFixedUpdate()
    {
        RaycastForIncline();

        base.InFixedUpdate();
        
        if (!isKeyDown)
        {
            StateLibrary.library.PlayerStateMachine.SwapState("IdlePS");
        }
    }

    public override void StateStart()
    {
        isKeyDown = true;
    }

    public override void StateExit(PlayerState next_state)
    {
        base.StateExit(next_state);
    }

    public override void Shift(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            StateLibrary.library.PlayerStateMachine.SwapState("SprintPS");
        }
    }

}
