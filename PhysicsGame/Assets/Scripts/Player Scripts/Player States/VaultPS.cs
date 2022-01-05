using UnityEngine;
using UnityEngine.InputSystem;

public class VaultPS : PlayerState
{

    private float time;
    private CapsuleCollider collider;
    private RigBlending blender;

    private Vector3 dest;
    private Vector3 initial;

    public VaultPS(Vector2 c, Transform t, Rigidbody r, CapsuleCollider coll, RigBlending blend) : base(c, t, r)
    {
        StateID = 4;

        collider = coll;
        blender = blend;
    }

    public override void InFixedUpdate()
    {
        time += Time.deltaTime;

        // funky cubics are funky
        // work on this more
        transform.position = Vector3.Lerp(initial, dest, (Mathf.Pow(2f*time - 1f, 3f) + 0.5f) * 0.25f);

        if (time > 1.8f)
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
    }

    public override void StateStart()
    {
        rbody.isKinematic = true;

        time = 0f;

        dest = transform.position + transform.forward * 5f;
        initial = transform.position;

        collider.height /= 4f;
        blender.enabled = false;
    }

    public override void StateExit(PlayerState next_state)
    {
        rbody.isKinematic = false;

        collider.height *= 4f;
        blender.enabled = true;

        base.StateExit(next_state);
    }

    public override void WASD(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isKeyDown = true;
        }

        else if (context.performed)
        {
            current_input = context.ReadValue<Vector2>();
        }

        else if (context.canceled)
        {
            current_input = Vector2.zero;

            isKeyDown = false;
        }
    }

    public override void Jump(InputAction.CallbackContext context)
    {
    }
}
