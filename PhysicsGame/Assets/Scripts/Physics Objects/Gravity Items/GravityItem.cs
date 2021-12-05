using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityItem : GravityObject
{
    private GCore gCore;

    private const float ATTRACTION = 8f;
    private const float MINIMUM_HELD_DISTANCE = 1f;

    // Start is called before the first frame update
    public override void Awake()
    {
        target_body = GetComponent<Rigidbody>();

        gCore = GameObject.FindGameObjectWithTag("Gravity Core").GetComponent<GCore>();
    }

    public override void AttractionUpdate()
    {
        target_body.AddForce(-(transform.position - gCore.getPos()).normalized * duration_of_attraction); // used to be * ATTRACTION

        duration_of_attraction += 1;

        is_held = (transform.position - gCore.getPos()).magnitude < MINIMUM_HELD_DISTANCE;
        is_attracted = !is_held;
    }

    public override void HeldUpdate()
    {
        target_body.AddForce(-(transform.position - gCore.getPos()).normalized * ATTRACTION);

        if (!gCore.do_lerp)
        {
            return;
        }

        transform.position = Vector3.Lerp(transform.position, gCore.getPos(), 0.125f);
    }

    public override void Launch(float l_s, bool with_curr_velo)
    {
        Vector3 l_force = (Camera.main.transform.forward * l_s) + (with_curr_velo ? target_body.velocity : Vector3.zero);

        target_body.AddForceAtPosition(l_force, -gCore.getPos(), ForceMode.Impulse);

        duration_of_attraction = 0;

        ActivateEffects(l_s, target_body.position);
    }

}
