using UnityEngine;
using System.Collections;

public class GravityControl : MonoBehaviour
{
    // the main gimmick script. can be toggle at any time to disable gravity controls.
    // further implementation will maybe have unlockable abilities.

    // little comments on this one because the whole suite of gravity implementations is due for
    // a little bit of a massive overhaul.

    private const float RAYCAST_RANGE = 15f; // changes how far we can attract/repel objects
    private const float POWER = 14f; // obvious
    private const int LAYER_MASK = (1 << 10); // is this even right anymore idk

    private int action_state = 0; // stores the value of what action was just done so that the AnimControllers can handle it
    public int actionState
    {
        get { return action_state; }
        set
        {
            if (value == action_state) return;
            action_state = value;
            AnimationHandler.UpdateAnimators(true); // just update the hand animations, nothing else
        }
    }

    private GravityObject target; // what are we targeting now?
    private Transform linked_camera_transform; // this is the player's perspective camera.
    private Vector3 dir;

    RaycastHit data; // raycast cache

    public delegate void BeamEffect(bool type, Vector3 direction, float distance); // true = attracting, false = repel
    public BeamEffect TriggerVSFXs;

    // this is just a setter for a private field
    public void PassCameraTransform(Transform camera)
    {
        linked_camera_transform = camera;
    }

    public void LClick()
    {
        StopCoroutine(SetAfterDelay());

        dir = linked_camera_transform.TransformDirection(Vector3.forward);

        if (target != null) // if we already have something grabbed
        {
            target.Released(); // drop it :>

            target = null;

            actionState = 0;

            return;
        }

        if (Physics.Raycast(transform.position + transform.up * 2, dir, out data, RAYCAST_RANGE, LAYER_MASK))
        {
            TriggerVSFXs(true, dir, data.distance);

            target = data.collider.gameObject.GetComponent<GravityObject>();

            if (target == null || !target.enabled)
            {
                data.distance = RAYCAST_RANGE;

                return;
            }

            target?.Attract();

            actionState = 1;
        }

        else
        {
            TriggerVSFXs(true, dir, RAYCAST_RANGE);
        }

        data.distance = RAYCAST_RANGE;
    }

    public void RClick()
    {
        StopCoroutine(SetAfterDelay());

        dir = linked_camera_transform.TransformDirection(Vector3.forward);

        // apply the launch force to the thing we're looking at without respect to its velo.
        if (Physics.Raycast(transform.position + transform.up * 2, dir, out data, RAYCAST_RANGE, LAYER_MASK))
        {
            target = data.collider.gameObject.GetComponent<GravityObject>();

            actionState = 2;

            StartCoroutine(SetAfterDelay());

            TriggerVSFXs(false, dir, data.distance);

            if (target != null && target.enabled)
            {
                target.Released();

                target.Launch(POWER, false, dir);

                target = null;
            }
        }

        data.distance = RAYCAST_RANGE;
    }

    public int getActionID()
    {
        return actionState;
    }

    // after the launch anim timer is "finished", reset the actionState to 0.
    public IEnumerator SetAfterDelay()
    {
        yield return new WaitForSeconds(0.25f);

        actionState = 0;
    }
}
