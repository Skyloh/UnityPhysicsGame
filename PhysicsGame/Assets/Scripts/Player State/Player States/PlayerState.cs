using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerState
{
    protected Vector2 current_input;

    protected Transform transform;
    protected Rigidbody rbody;

    protected const float MOVE_SPEED = 25f; // self-explanatory
    protected const float MAX_VELO = 5f; // self-explanatory
    protected const float JUMP_FORCE = 5f; // self-explanatory
    protected const int PREJUMP_DURATION = 5;

    public PlayerState(Vector2 current_input, Transform player, Rigidbody rbody)
    {
        this.current_input = current_input;
        transform = player;
        this.rbody = rbody;
    }

    public void UpdateReferences(Vector2 current_input, Transform player, Rigidbody rbody)
    {
        this.current_input = current_input;
        transform = player;
        this.rbody = rbody;
    }

    public virtual void InFixedUpdate()
    {
        if (!isGrounded())
        {
            StateLibrary.library.PlayerStateMachine.SwapState("AirbornePS");

            return;
        }

        if (current_input != Vector2.zero && !(rbody.velocity.magnitude > MAX_VELO))
        {
            Vector3 movement = transform.right * current_input.x * 0.5f + transform.forward * current_input.y;

            rbody.AddForce(movement * MOVE_SPEED * StateMultiplier());
        }
        // * 0.85f, ForceMode.Impulse
        else if (current_input == Vector2.zero)
        {
            rbody.AddForce(-rbody.velocity * 0.15f, ForceMode.Impulse);
        }
    }

    public virtual void StateStart()
    {
        // pass
    }

    public virtual void StateExit(PlayerState next_state)
    {
        next_state.UpdateReferences(current_input, transform, rbody);
        next_state.StateStart();
    }

    public virtual void WASD(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            current_input = context.ReadValue<Vector2>();
        }

        else if (context.canceled)
        {
            current_input = Vector2.zero;
        }
    }

    public virtual void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StateLibrary.library.PlayerStateMachine.SwapState("PrejumpPS");
        }
    }


    protected bool isGrounded() => Physics.Raycast(transform.position, Vector3.down, 1.3f);

    public virtual float StateMultiplier() => 1f;
}
