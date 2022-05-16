using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerState
{
    protected bool allow_action = true; // essentially, puts the player into a "cutscene" for a brief moment while an animation plays
    protected bool allow_rotation = true;

    protected Vector2 current_input;
    protected Transform transform;
    protected Rigidbody rbody;

    protected const float MOVE_SPEED = 200f; // self-explanatory
    protected const float MAX_RESULTANT_GROUND_VELO = 5f; // the max groundspeed the character can apply to themselves
    protected const float MAX_RESULTANT_AIR_VELO = 6f; // the max airspeed the character can apply to themselves (before custom drag sets in)
    protected const float JUMP_FORCE = 8f; // self-explanatory
    protected const int PREJUMP_DURATION = 5;
    protected float DASH_MULTIPLIER = 1f; // applied when in sprint mode
    protected float MIN_WALL_CLIMB_HEIGHT = 0.5f;
    protected const int AL_MASK = 1 << 11;

    public int StateID;
    public bool isKeyDown;

    public bool guard_exit = false; // limits to which state the current state can exit to, and is set by the StateLibrary

    public PlayerState(Vector2 current_input, Transform player, Rigidbody rbody)
    {
        this.current_input = current_input;
        transform = player;
        this.rbody = rbody;

        isKeyDown = false;

        StateID = 0;
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
        }

        else if (current_input != Vector2.zero && !(rbody.velocity.magnitude > MAX_RESULTANT_GROUND_VELO * DASH_MULTIPLIER))
        {
            Vector3 movement = transform.right * current_input.x * 0.5f + transform.forward * current_input.y;
            
            rbody.AddForce(movement * MOVE_SPEED * Time.deltaTime, ForceMode.VelocityChange);
        }

        else if (current_input == Vector2.zero)
        {
            rbody.AddForce(-rbody.velocity * 0.75f, ForceMode.VelocityChange);
        }
    }

    public virtual void StateStart()
    {
        // pass
    }

    public virtual void StateExit(PlayerState next_state)
    {
        next_state.UpdateReferences(current_input, transform, rbody);
        next_state.isKeyDown = isKeyDown;
        next_state.StateStart();
    }

    public virtual void WASD(InputAction.CallbackContext context)
    {

        if (context.started)
        {
            isKeyDown = true;
        }

        else if (context.performed)
        {
            current_input = context.ReadValue<Vector2>();

            // isKeyDown = true; as of 5/12, do i need this?
        }

        else if (context.canceled)
        {
            current_input = Vector2.zero;

            isKeyDown = false;
        }
    }

    public virtual void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StateLibrary.library.PlayerStateMachine.SwapState("PrejumpPS");
        }
    }

    public virtual void Shift(InputAction.CallbackContext context)
    {
        // only defined in Movement and Sprint
    }

    protected bool isGrounded() => Physics.Raycast(transform.position, Vector3.down, 0.09f);

    protected float getXYVelo() => Mathf.Sqrt(Mathf.Pow(rbody.velocity.x, 2) + Mathf.Pow(rbody.velocity.y, 2));

    public void ToggleActionBool(bool to_value)
    {
        allow_action = to_value;
    }

    public void ToggleRotationBool(bool to_value)
    {
        allow_rotation = to_value;
    }

    public bool GetAllowAction()
    {
        return allow_action;
    }
    public bool GetAllowRotation()
    {
        return allow_rotation;
    }

}
