using UnityEngine;
using UnityEngine.InputSystem;

public class AirbornePS : PlayerState
{
    // my thinking is that you only want to ledgegrab if you are airborne, but this may
    // feel weird. it requires playtesting.

    RaycastHit ActionRaycastData;
    float ar_distance; // caches raycast distance, even though RaycastHit is a struct :/
    bool on_slope;

    CapsuleCollider capsuleCollider;
    float original_capsule_radius;
    float prior_xzvelo = 999f; // used to see if the player is stuck in a position

    // KNOWN *BUG* (s)
    // Holding into the face of a wall slows your descent slightly.
    // This could be a nice gimmick?
    // Maybe add a graphical raycast to add spark effects while sliding.
    //
    // if you are on a slope and you mash a direction into the slope,
    // you sometimes move up it. If you mash jump as well, you can jump
    // on the slope, too.

    public AirbornePS(Vector2 c, Transform t, Rigidbody r, CapsuleCollider cc) : base(c, t, r)
    {
        StateID = 2;
        on_slope = false;

        capsuleCollider = cc;
        original_capsule_radius = cc.radius;
    }

    public override void StateStart()
    {
        raycast_offset = 2f;
    }

    public override void StateExit(PlayerState nextState)
    {
        raycast_offset = 0f;

        base.StateExit(nextState);
    }

    private float GetActionRaycastDistance()
    {
        // send a raycast out in front and above the player to detect for any ledges from their mid-section to below (maybe idk)
        Physics.Raycast(transform.position + transform.forward * 0.5f, Vector3.up * 3f, out ActionRaycastData, 1f, AL_MASK);
        
        return (ActionRaycastData.collider != null) ? ActionRaycastData.transform.position.y : -5f; // -5f because at this raycast length, 5 is never a value
    }

    public override void InFixedUpdate()
    {
        on_slope = MAX_SLOPE_ANGLE - Mathf.Abs(Vector3.Angle(Vector3.up, GroundInfo.normal)) < 0f;

        // must be cached
        ar_distance = GetActionRaycastDistance();

        if (isGrounded())
        {
            if (on_slope)
            {
                rbody.AddForce(GroundInfo.normal * (2f + RAYCAST_LENGTH - GroundInfo.distance) * 5f, ForceMode.Force);
            }
            
            else if (GroundInfo.distance < RAYCAST_LENGTH)
            {

                capsuleCollider.radius = original_capsule_radius;

                if (isKeyDown)
                {
                    StateLibrary.library.PlayerStateMachine.SwapState("MovementPS");

                    return;
                }

                StateLibrary.library.PlayerStateMachine.SwapState("IdlePS");

                return;
            }

        }

        else
        {
            // if we hit a ledge the proper distance away, grab it.
            // don't do this if this exit is guarded, because that means we grabbed a ledge recently.
            if (!guard_exit && !(Mathf.Abs(ar_distance) > 4f)) // in future add the caviat that the player cannot be moving fast away/parallel to edge before grabbing it.
            {
                // adjusts the player location to match the animation because apparently root transforms are cringe
                transform.position += Vector3.up * (ar_distance - transform.position.y - 2.1f) + transform.forward * 0.3f;

                StateLibrary.library.PlayerStateMachine.CarefulSwapState("WallGrabPS"); // dont go into wallgrab if we were in it before
                                                                                        // this is important bc it essentially locks the player from being immediately returned into wallgrab after letting go.
            }


            // general air movement stuff
            movement = transform.right * current_input.x * 2f + transform.forward * current_input.y * 2f;

            rbody.AddForce(movement * MOVE_SPEED * Time.deltaTime, ForceMode.Force); // allows for >>subtle<< air movement

            // ignore Y velo because we dont care about limiting rising/falling speed.
            // probably really suboptimal to define my own method of this 2d pythag but stfu future goon
            float xzvelo = getXZVelo();

            if (xzvelo > MAX_RESULTANT_AIR_VELO)
            {
                rbody.AddForce(rbody.velocity * -1.5f, ForceMode.Force);
            }
            else if (Mathf.Abs(xzvelo - prior_xzvelo) < 0.3f)
            {
                capsuleCollider.radius = Mathf.Clamp(capsuleCollider.radius - 0.1f, 0.1f, original_capsule_radius);
            }

            prior_xzvelo = xzvelo;
        }

    }

    public override void Jump(InputAction.CallbackContext context)
    {
        // remove implementation
    }

    public override void Shift(InputAction.CallbackContext context)
    {
        // remove implementation
    }
}
