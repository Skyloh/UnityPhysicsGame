using UnityEngine;
using System.Collections;

public class GravityControl : MonoBehaviour
{
    // the main gimmick script. can be toggle at any time to disable gravity controls.
    // further implementation will maybe have unlockable abilities.

    // little comments on this one because the whole suite of gravity implementations is due for
    // a little bit of a massive overhaul.

    private const float RAYCAST_RANGE = 30f; // changes how far we can attract/repel objects
    private const float POWER = 25f; // obvious
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

    RaycastHit data; // raycast cache

    // this is just a setter for a private field
    public void PassCameraTransform(Transform camera)
    {
        linked_camera_transform = camera;
    }

    public void LClick()
    {
        StopCoroutine(SetAfterDelay());

        if (target != null) // if we already have something grabbed
        {
            target.Released(); // drop it :>

            target = null;

            actionState = 0;

            return;
        }

        if (Physics.Raycast(transform.position + transform.up * 2, linked_camera_transform.TransformDirection(Vector3.forward), out data, RAYCAST_RANGE, LAYER_MASK))
        {
            target = data.collider.gameObject.GetComponent<GravityObject>();

            target?.Attract();

            actionState = 1;
        }
    }

    public void RClick()
    {
        StopCoroutine(SetAfterDelay());

        // apply the launch force to the thing we're looking at without respect to its velo.
        if (Physics.Raycast(transform.position + transform.up * 2, linked_camera_transform.TransformDirection(Vector3.forward), out data, RAYCAST_RANGE, LAYER_MASK))
        {
            target = data.collider.gameObject.GetComponent<GravityObject>();

            actionState = 2;

            StartCoroutine(SetAfterDelay());

            // graphical effects here

            if (target != null)
            {
                target.Released();

                target.Launch(POWER, false, linked_camera_transform.TransformDirection(Vector3.forward));

                target = null;
            }
        }
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
