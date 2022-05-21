using UnityEngine;
using UnityEngine.InputSystem;

public class AirbornePS : PlayerState
{
    // my thinking is that you only want to ledgegrab if you are airborne, but this may
    // feel weird. it requires playtesting.

    private RaycastHit ActionRaycastData;
    private float ar_distance; // caches raycast distance, even though RaycastHit is a struct :/


    // KNOWN *BUG*
    // Holding into the face of a wall slows your descent slightly.
    // This could be a nice gimmick?
    // Maybe add a graphical raycast to add spark effects while sliding.

    public AirbornePS(Vector2 c, Transform t, Rigidbody r) : base(c, t, r)
    {
        StateID = 2;
    }

    private float GetActionRaycastDistance()
    {
        // send a raycast out in front and above the player to detect for any ledges from their mid-section to below (maybe idk)
        Physics.Raycast(transform.position + transform.forward + Vector3.up, Vector3.up * 1.5f, out ActionRaycastData, 1f, AL_MASK);
        
        return (ActionRaycastData.collider != null) ? ActionRaycastData.transform.position.y : -5f; // -5f because at this raycast length, 5 is never a value
    }

    // rework this for optimal execution (i.e. change around the else if and ifs
    public override void InFixedUpdate()
    {
        // must be cached
        ar_distance = GetActionRaycastDistance();

        // if we hit a ledge the proper distance away, grab it.
        // don't do this if this exit is guarded, because that means we grabbed a ledge recently.
        if (!guard_exit && !(Mathf.Abs(ar_distance) > 4f)) // in future add the caviat that the player cannot be moving fast away/parallel to edge before grabbing it.
        {
            // adjusts the player location to match the animation because apparently root transforms are cringe
            transform.position += Vector3.up * (ar_distance - transform.position.y - 2.1f) + transform.forward * 0.3f;

            StateLibrary.library.PlayerStateMachine.CarefulSwapState("WallGrabPS"); // dont go into wallgrab if we were in it before
            // this is important bc it essentially locks the player from being immediately returned into wallgrab after letting go.
        }

        if (isGrounded())
        {
            
            if (isKeyDown)
            {
                StateLibrary.library.PlayerStateMachine.SwapState("MovementPS");

                return;
            }

            StateLibrary.library.PlayerStateMachine.SwapState("IdlePS");
            
        }

        Vector3 movement = transform.right * current_input.x * 0.5f + transform.forward * current_input.y;

        rbody.AddForce(movement * MOVE_SPEED * 0.2f * Time.deltaTime, ForceMode.Force); // allows for >>subtle<< air movement

        // ignore Y velo because we dont care about limiting rising/falling speed.
        // probably really suboptimal to define my own method of this 2d pythag but stfu future goon
        if (getXZVelo() > MAX_RESULTANT_AIR_VELO)
        {
            rbody.AddForce(rbody.velocity * -1.5f, ForceMode.Force);
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
