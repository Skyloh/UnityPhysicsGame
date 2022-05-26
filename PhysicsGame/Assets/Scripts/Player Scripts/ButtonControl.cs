using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class ButtonControl : MonoBehaviour
{
    List<int> EXIT_STATE_IDS = new List<int> { 0, 1, 4 };

    RaycastHit ray_info;

    PlayerSM state_machine;

    // possible optimization:
    // cache the InstanceID of the button collider and the button script.
    // when checking the ray, make sure the InstanceID is unique before we do collider.tag.
    // if it isn't unique, then just used the cached button.

    private void Start()
    {
        state_machine = GetComponent<PlayerSM>();
    }

    public void ButtonLClick(InputAction.CallbackContext context)
    {
        if (context.started && Physics.Raycast(transform.position, transform.forward, out ray_info, 1.5f))
        {
            if (ray_info.collider.tag == "Button" && EXIT_STATE_IDS.Contains(state_machine.getState()))
            {
                ManualButton button = ray_info.collider.gameObject.GetComponent<ManualButton>();

                // i could consolidate this into the previous if, but that'd be MASSIVE and ugly
                if (button.PressButton())
                {
                    state_machine.SwapState("ButtonPressPS");
                }

            }
        }
    }
}
