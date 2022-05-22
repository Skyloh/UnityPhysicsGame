using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerState
{
    // this is shoddy implementation of cutscene animations, but it works for now.
    protected bool allow_action = true; // essentially, puts the player into a "cutscene" for a brief moment while an animation plays
    protected bool allow_rotation = true; // see above, but for rotation only

    protected Vector3 movement = Vector3.zero; // declaring variables in Update is, again, a bad.

    // cache cache CACHE THOSE COMPONENTS ba da ba ba daaa :musical_note:
    protected Vector2 current_input;
    protected Transform transform;
    protected Rigidbody rbody;

    protected const float MOVE_SPEED = 200f; // self-explanatory
    protected const float MAX_RESULTANT_GROUND_VELO = 5f; // the max groundspeed the character can apply to themselves
    protected const float MAX_RESULTANT_AIR_VELO = 6f; // the max airspeed the character can apply to themselves (before custom drag sets in)
    protected const float JUMP_FORCE = 8f; // self-explanatory
    protected const int PREJUMP_DURATION = 5;
    protected const int AL_MASK = 1 << 11; // bit layer mask shifted to only target ActionableTerrain layer objects
    protected const float RAYCAST_LENGTH = 0.09f; // self-explanatory
    protected const float MAX_INCLINE_DOT = 0.4f; // the max dot product for a slope to warrant movement aid
    protected const float MAX_SLOPE_ANGLE = 40f; // the max angle for a slope to warrant airdrift aid to prevent collider stickiness

    protected float raycast_offset = 0f; // extends the length of the ray
    protected float dash_multiplier = 1f; // applied when in sprint mode
    protected RaycastHit GroundInfo;

    public int StateID;
    public bool isKeyDown; // lazy implementation for holding a key between state changes, but it works

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

    // runs in the physics process loop and therefore handles FISICS
    public virtual void InFixedUpdate()
    {
        // if we are in the air, swap accordingly
        if (!isGrounded())
        {
            StateLibrary.library.PlayerStateMachine.SwapState("AirbornePS");
        }

        // if we aren't inputting nothing and we are not at capped speed, apply force (AS VELOCITY CHANGE)
        else if (current_input != Vector2.zero && !(rbody.velocity.magnitude > MAX_RESULTANT_GROUND_VELO * dash_multiplier))
        {
            movement = transform.right * current_input.x * 0.5f + transform.forward * current_input.y;
            
            rbody.AddForce(movement * MOVE_SPEED * Time.deltaTime, ForceMode.VelocityChange);
        }

        // and if we are doing nothing, apply custom drag
        else if (current_input == Vector2.zero)
        {
            rbody.AddForce(-rbody.velocity * 0.75f, ForceMode.VelocityChange);
        }
    }

    // called when a state swap occurs as a way to init the state
    public virtual void StateStart()
    {
        // pass
    }

    // called when a state swap occurs, but as a way to pass relevant info to the state
    public virtual void StateExit(PlayerState next_state)
    {
        next_state.UpdateReferences(current_input, transform, rbody);
        next_state.isKeyDown = isKeyDown;
        next_state.StateStart();
    }

    // obviously, handles WASD inputs
    // InputAction methods are called 3 times (for reasons), and context's different booleans
    // stores the state that each function execution was called under.
    public virtual void WASD(InputAction.CallbackContext context)
    {

        if (context.started)
        {
            isKeyDown = true;
        }

        else if (context.performed)
        {
            current_input = context.ReadValue<Vector2>(); // why am i not reading input in context.started? good question.
        }

        else if (context.canceled)
        {
            current_input = Vector2.zero;

            isKeyDown = false;
        }
    }

    // technically should be context.started to avoid any accidental extra calls, but it hasnt happened yet
    // and InputAction is super cursed when it comes to swapping overrides during runtime (it just stops).
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

    // returns a boolean to see if the player is within ground bounds. ray length varies in some states.
    protected bool isGrounded() => Physics.Raycast(transform.position, Vector3.down, out GroundInfo, RAYCAST_LENGTH + raycast_offset);

    // 2d pythag used in jumping states to limiting lateral movement
    protected float getXZVelo() => Mathf.Sqrt(Mathf.Pow(rbody.velocity.x, 2) + Mathf.Pow(rbody.velocity.z, 2));

    #region ew gross ew (action/rotation bools)
    // absolute nightmares, look at PlayerSM for their explanation.
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
    #endregion
}
