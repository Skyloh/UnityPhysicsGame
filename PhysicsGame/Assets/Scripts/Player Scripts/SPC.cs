using UnityEngine;
using UnityEngine.InputSystem;

// Simple Player Controller
// manages state machines

public class SPC : MonoBehaviour
{

    // to be ported to it's own script?
    private Vector2 mouse_input;

    private FPSCam linked_camera;

    private GravityControl GController;

    private PlayerSM PlayerStateManager;

    private Transform tracked;

    private void Start()
    {
        linked_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FPSCam>();

        tracked = GameObject.FindGameObjectWithTag("Neck").transform;

        GController = GetComponent<GravityControl>();
        GController.PassCameraTransform(linked_camera.transform);

        PlayerStateManager = StateLibrary.library.PlayerStateMachine;
    }

    private void Update()
    {
        transform.eulerAngles = tracked.eulerAngles = Vector3.up * (linked_camera.transform.eulerAngles.y);
    }

    public void WASD_Input(InputAction.CallbackContext context)
    {
        PlayerStateManager.WASD(context);
    }
    
    public void Jump_Input(InputAction.CallbackContext context)
    {
        PlayerStateManager.Jump(context);
    }

    public void Shift_Input(InputAction.CallbackContext context)
    {
        PlayerStateManager.Shift(context);
    }

    // if we have a held item, drop it. otherwise, send out a ray to attract an item.
    public void Click_Input(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GController.LClick();
        }
    }

    // applies a force away from the selected or held object if we have a charge built up. otherwise, start building one.
    public void RightClick_Input(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GController.RClick();
        }

    }

    public void OnMouseDelta(InputAction.CallbackContext context)
    {
        mouse_input = context.ReadValue<Vector2>();

        linked_camera.RotateCamera(mouse_input);
    }


    public int getPStateID() => PlayerStateManager.getState();
    public int getGStateID() => GController.getActionID(); // GravityStateManager.getState() REPLACED WITH ANIM MANAGER FOR UIOVERLAY
}
