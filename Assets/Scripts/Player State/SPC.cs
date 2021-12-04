using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Simple Player Controller
// manages state machines

public class SPC : MonoBehaviour
{

    //TODO:
    // make movement not feel like butter on ice
    // add impact to launches
    //      minor screen shake? particle effects? sfx?
    // add VFX/particle effects to attraction + hold
    // add reverse attraction (GravitySolid object that pulls you toward it, not the other way around)

    // to be ported to it's own script?
    private Vector2 mouse_input;

    private FPSCam linked_camera;

    private GravitySM GravityStateManager;
    private PlayerSM PlayerStateManager;

    private void Start()
    {
        linked_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FPSCam>();

        GravityStateManager = StateLibrary.library.GravityStateMachine;
        PlayerStateManager = StateLibrary.library.PlayerStateMachine;
    }

    private void FixedUpdate()
    {
        UpdateRotationToCam();

        // State Managers have their own FixedUpdate loop

    }

    public void WASD_Input(InputAction.CallbackContext context)
    {
        PlayerStateManager.WASD(context);
    }
    
    public void Jump_Input(InputAction.CallbackContext context)
    {
        PlayerStateManager.Jump(context);
    }

    // if we have a held item, drop it. otherwise, send out a ray to attract an item.
    public void Click_Input(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GravityStateManager.LClickEvent();
        }
    }

    // applies a force away from the selected or held object if we have a charge built up. otherwise, start building one.
    public void RightClick_Input(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GravityStateManager.RClickEvent();
        }
    }

    public void OnMouseDelta(InputAction.CallbackContext context)
    {
        mouse_input = context.ReadValue<Vector2>();

        linked_camera.RotateCamera(mouse_input);
    }

    private void UpdateRotationToCam()
    {
        transform.eulerAngles = Vector3.up * (linked_camera.transform.eulerAngles.y);
    }

}
