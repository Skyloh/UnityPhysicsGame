using UnityEngine;
using UnityEngine.InputSystem;

public class VaultPS : PlayerState
{

    private float time;
    private CapsuleCollider collider;
    private Vector3 dest;

    public VaultPS(Vector2 c, Transform t, Rigidbody r, CapsuleCollider coll) : base(c, t, r)
    {
        StateID = 3;

        collider = coll;
    }

    public override void InFixedUpdate()
    {
        time += Time.deltaTime;

        transform.position = Vector3.Lerp(transform.position, dest, Mathf.Exp(time - 2) / 10f);

        if (time > 1.8f)
        {
            StateLibrary.library.PlayerStateMachine.SwapState("MovementPS");
        }
    }

    public override void StateStart()
    {
        rbody.isKinematic = true;

        time = 0f;

        dest = transform.position + transform.forward * 7f;

        collider.height /= 4f;
    }

    public override void StateExit(PlayerState next_state)
    {
        rbody.isKinematic = false;

        rbody.AddForce(transform.forward * 100f);

        current_input = new Vector2(transform.position.x, transform.position.z).normalized;

        collider.height *= 4f;

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
