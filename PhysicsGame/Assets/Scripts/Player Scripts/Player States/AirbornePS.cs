using UnityEngine;
using UnityEngine.InputSystem;

public class AirbornePS : PlayerState
{
    // my thinking is that you only want to ledgegrab if you are airborne, but this may
    // feel weird. it requires playtesting.

    private RaycastHit ActionRaycastData;
    private float ar_distance; // caches the distance, allowing for -1 to be stored
    // you *can* set the distance on a data output struct but that looks really really weird so im not gonna :>


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
        // if we don't hit anything, return -1. if we just returned the datadistance, we'd get 0.
        // this would trigger the wall-climb state even tho we just arent in front of anything.
        // if the raycast is in the wall completely, it'll return 0, therefore we ignore that
        // value too.
        if (!Physics.Raycast(transform.position + transform.forward * 1.75f + Vector3.up * 3f, Vector3.down, out ActionRaycastData, 3f, AL_MASK))
        {
            return -1f;
        }

        return ActionRaycastData.distance;
    }

    // rework this for optimal execution (i.e. change around the else if and ifs
    public override void InFixedUpdate()
    {
        ar_distance = GetActionRaycastDistance();

        // if we hit a ledge the proper distance away, grab it.
        // don't do this if this exit is guarded, because that means we grabbed a ledge recently.
        if (!guard_exit && !(ar_distance <= 0) && ar_distance < MIN_WALL_CLIMB_HEIGHT) // in future add the caviat that the player cannot be moving fast away/parallel to edge before grabbing it.
        {
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

        rbody.AddForce(movement * MOVE_SPEED * 0.2f * Time.deltaTime, ForceMode.Force); // allows for subtle air movement

        if (getXYVelo() > MAX_RESULTANT_AIR_VELO)
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
