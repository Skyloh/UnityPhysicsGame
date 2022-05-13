using UnityEngine;
using System.Collections;

public class GravityControl : MonoBehaviour
{
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
            AnimationHandler.UpdateAnimators(false); // just update the hand animations, nothing else
        }
    }

    private GravityObject target;
    private Transform linked_camera_transform; // this is the player's perspective camera.

    // Start is called before the first frame update
    void Start()
    {
        // some sort of anim goes here (or maybe on onenable/disable)
    }

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

            AssignTarget(null);

            actionState = 0;

            return;
        }

        RaycastHit data;

        if (Physics.Raycast(transform.position + transform.up * 2, linked_camera_transform.TransformDirection(Vector3.forward), out data, RAYCAST_RANGE, LAYER_MASK))
        {
            AssignTarget(data.collider.gameObject.GetComponent<GravityObject>());

            target?.Attract();

            actionState = 1;
        }

    }

    public void RClick()
    {
        StopCoroutine(SetAfterDelay());

        RaycastHit data;

        // apply the launch force to the thing we're looking at without respect to its velo.
        if (Physics.Raycast(transform.position + transform.up * 2, linked_camera_transform.TransformDirection(Vector3.forward), out data, RAYCAST_RANGE, LAYER_MASK))
        {
            actionState = 2;

            GravityObject to_be_launched = data.collider.gameObject.GetComponent<GravityObject>();

            to_be_launched?.Released(); // ISSUE: do i need this?

            to_be_launched?.Launch(POWER, false);

            AssignTarget(null);

            StartCoroutine(SetAfterDelay());
        }
    }

    private void AssignTarget(GravityObject target)
    {
        this.target = target;
    }

    public int getActionID()
    {
        return actionState;
    }

    public IEnumerator SetAfterDelay()
    {
        yield return new WaitForSeconds(0.25f);

        actionState = 0;
    }
}
