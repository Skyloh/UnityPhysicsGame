using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeGS : GravityState
{

    float initial_charge_time;
    float current_charge;

    bool do_vignette;

    public ChargeGS(GravityObject g, FPSCam f, Transform t) : base(g, f, t)
    {
        initial_charge_time = 0f;
        current_charge = 0f;
        do_vignette = true;
    }

    public override void FixedUpdate()
    {
        current_charge = Mathf.Clamp((Time.time - initial_charge_time) * LAUNCH_CHARGE_PER_SECOND, MINIMUM_LAUNCH_CHARGE, MAXIMUM_LAUNCH_CHARGE);

        // this is needed bc fixedupdate runs so many times that it runs between lines of RClick, which resets the vignette
        // then exits the state. in this tiny timeframe, the fixedupdate runs again, re-updating the vignette even though i 
        // dont want it to bc i have already given directive to leave the state.
        if (do_vignette)
        {
            linked_camera.UpdateVignette(current_charge / MAXIMUM_LAUNCH_CHARGE);
        }
    }

    public override void StateStart()
    {
        initial_charge_time = Time.time;
        do_vignette = true;
    }

    public override void RClick() // ADD IMPLEMENTATION FOR GRAVITYSOLID
    {

        linked_camera.ResetVignette();
        do_vignette = false;

        StateLibrary.library.GravityStateMachine.SwapState("DefaultGS");

        // if we already have something grabbed, release it then apply the launch force with current velo (to allow slinging)
        if (target != null)
        {
            target.Released();

            target.Launch(current_charge, true);

            AssignTarget(null);

            return;
        }

        RaycastHit data;

        // if we havent grabbed something, apply the launch force to the thing we're looking at without respect to its velo.
        if (Physics.Raycast(transform.position, linked_camera.transform.TransformDirection(Vector3.forward) * RAYCAST_RANGE, out data))
        {
            GravityObject to_be_launched = data.collider.gameObject.GetComponent<GravityObject>();

            to_be_launched?.Released(); // ISSUE: do i need this?

            to_be_launched?.Launch(current_charge, false);

            AssignTarget(null);

        }
    }

    // this is used to reduce movespeed
    public override float StateMultiplier()
    {
        return 0.25f;
    }
}
