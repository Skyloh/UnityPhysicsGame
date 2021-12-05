using UnityEngine;

public class DefaultGS : GravityState
{

    public DefaultGS(GravityObject t, FPSCam l, Transform trans) : base(t, l, trans)
    {
        // pass
    }

    public override void StateStart()
    {
        AssignTarget(null);
    }

    public override void LClick()
    {
        if (target != null) // if we already have something grabbed
        {
            target.Released(); // drop it :>

            AssignTarget(null);

            return;
        }

        RaycastHit data;

        if (Physics.Raycast(transform.position, linked_camera.transform.TransformDirection(Vector3.forward) * RAYCAST_RANGE, out data))
        {
            AssignTarget(data.collider.gameObject.GetComponent<GravityObject>());

            target?.Attract();
        }

    }

    public override void RClick()
    {
        StateLibrary.library.GravityStateMachine.SwapState("ChargeGS");
    }
}
