using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{

    // delegatges????? :thinking:
    // also useless class lmao

    private Vector3 ActionRayCastOffset;

    public bool do_raycast = true; // stops raycasts during unneccesary actions, like vaulting or prejump

    private RaycastHit r_action_info;
    private RaycastHit r_grounded_info;

    private void Start()
    {
        ActionRayCastOffset = -transform.right * 3 + Vector3.up * 12f;
    }

    private void Update()
    {
        if (do_raycast)
        {
            DoGroundedRaycast();
            DoActionRaycast();
        }
    }

    private void DoGroundedRaycast()
    {
        Physics.Raycast(transform.position, Vector3.down, out r_grounded_info, 0.09f);
    }

    private void DoActionRaycast()
    {
        Physics.Raycast(transform.position + ActionRayCastOffset, Vector3.down, out r_action_info, 0.09f);
    }


}
