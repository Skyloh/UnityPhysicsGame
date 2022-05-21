using UnityEngine;

public class GravityItem : GravityObject
{
    private Transform gCore;

    private const float ATTRACTION = 8f;
    private const float MINIMUM_HELD_DISTANCE = 1f;

    // Start is called before the first frame update
    public override void Awake()
    {
        target_body = GetComponent<Rigidbody>();

        gCore = GameObject.FindGameObjectWithTag("Gravity Core").transform;
    }

    public override void AttractionUpdate()
    {
        target_body.AddForce(-(transform.position - gCore.position).normalized * duration_of_attraction); // used to be * ATTRACTION

        duration_of_attraction += 1;

        is_held = (transform.position - gCore.position).magnitude < MINIMUM_HELD_DISTANCE;
        is_attracted = !is_held;
    }

    public override void HeldUpdate()
    {
        target_body.AddForce(-(transform.position - gCore.position).normalized * ATTRACTION);

        transform.position = Vector3.Lerp(transform.position, gCore.position, 0.125f);
    }

    public override void Launch(float l_s, bool with_curr_velo, Vector3 direction)
    {
        Vector3 l_force = (direction * l_s) + (with_curr_velo ? target_body.velocity : Vector3.zero);

        target_body.AddForceAtPosition(l_force, -gCore.position, ForceMode.Impulse); 

        duration_of_attraction = 0;

        ActivateEffects(l_s, target_body.position);
    }

}
