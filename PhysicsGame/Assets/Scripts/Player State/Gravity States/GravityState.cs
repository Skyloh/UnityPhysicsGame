using UnityEngine;

public class GravityState
{

    protected const float LAUNCH_CHARGE_PER_SECOND = 10f; // used when calculating launch force for right-click functionality
    protected const float MINIMUM_LAUNCH_CHARGE = 0f; // see above, used in the clamp method
    protected const float MAXIMUM_LAUNCH_CHARGE = 50f; // see above, used in the clamp method
    protected const float RAYCAST_RANGE = 5f; // changes how far we can attract/repel objects


    protected Transform transform;

    protected GravityObject target;
    protected FPSCam linked_camera;

    public GravityState(GravityObject t, FPSCam l, Transform trans)
    {
        target = t;
        linked_camera = l;
        transform = trans;
    }

    public void UpdateReferences(GravityObject t, FPSCam l, Transform trans)
    {
        target = t;
        linked_camera = l;
        transform = trans;
    }

    public virtual void FixedUpdate()
    {
        // ISSUE: add implementation for attraction TO a gravitysolid
    }

    public virtual void StateStart()
    {
        // pass
    }

    public virtual void StateExit(GravityState next_state)
    {
        // maybe add some logic here to stop GO physics or something

        next_state.UpdateReferences(target, linked_camera, transform);
        next_state.StateStart();
    }


    // the InputAction contexts arent needed for these, maybe
    public virtual void RClick()
    {
        // pass
    }

    public virtual void LClick()
    {
        // pass
    }

    #region get/set
    public void AssignTarget(GravityObject target)
    {
        this.target = target;
    }

    public GravityObject GetTarget()
    {
        return target;
    }
    #endregion

    public virtual float StateMultiplier() // a value that exists for reasons :)
    {
        return 1f;
    }
}
