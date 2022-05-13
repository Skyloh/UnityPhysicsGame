using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerState
{
    protected bool allow_action = true; // essentially, puts the player into a "cutscene" for a brief moment while an animation plays

    protected Vector2 current_input;

    protected Transform transform;
    protected Rigidbody rbody;

    public bool isKeyDown;

    private RaycastHit ActionRaycastData;

    protected const float MOVE_SPEED = 5000f; // self-explanatory
    protected const float MAX_VELO = 5f; // self-explanatory
    protected const float JUMP_FORCE = 15f; // self-explanatory
    protected const int PREJUMP_DURATION = 5;
    protected float DASH_MULTIPLIER = 1f; // applied when in sprint mode

    protected const int AL_MASK = (1 << 10);

    public int StateID;

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
        Debug.Log((int)didWallRaycastHit());

        if (!isGrounded())
        {
            StateLibrary.library.PlayerStateMachine.SwapState("AirbornePS");

            return;
        }

        else if (current_input != Vector2.zero && !(rbody.velocity.magnitude > MAX_VELO * DASH_MULTIPLIER))
        {
            Vector3 movement = transform.right * current_input.x * 0.5f + transform.forward * current_input.y;

            rbody.AddForce(movement * MOVE_SPEED * Time.deltaTime);
        }

        // * 0.85f, ForceMode.Impulse
        else if (current_input == Vector2.zero)
        {
            rbody.AddForce(-rbody.velocity * 0.75f, ForceMode.Impulse);
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

    protected float didWallRaycastHit()
    {
        Physics.Raycast(transform.position + transform.forward * 1.5f + Vector3.up * 3f, Vector3.down * 0.5f, out ActionRaycastData, AL_MASK); // WIP HERE

        return ActionRaycastData.distance;
    }

    public void ToggleActionBool(bool to_value)
    {
        allow_action = to_value;
    }

}
