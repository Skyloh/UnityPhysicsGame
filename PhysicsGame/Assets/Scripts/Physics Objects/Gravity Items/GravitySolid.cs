using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySolid : GravityObject
{
    private Vector3 collision_point;

    private Vector3 launch_dir;

    [SerializeField] bool fixed_angle;

    public override void Awake()
    {
        is_solid = true;

        target_body = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(is_attracted && collision.gameObject.tag == "Player")
        {
            is_held = true;
            is_attracted = false;

            collision_point = collision.transform.position;
        }
    }

    public override void AttractionUpdate()
    {
        target_body.AddForce((transform.position - target_body.position).normalized * duration_of_attraction); // used to be * ATTRACTION
        
        duration_of_attraction += 1;
    }

    public override void HeldUpdate()
    {
        target_body.AddForce(-target_body.velocity * 15f);
        target_body.position = Vector3.Lerp(collision_point, target_body.position, 0.125f);

        target_body.useGravity = false;
    }

    public override void Launch(float l_s, bool with_curr_velo, Vector3 direction)
    {
        if (being_launched)
        {
            return;
        }

        launch_dir = fixed_angle ? 0.25f * -direction + transform.up : -direction;

        Vector3 l_force = launch_dir * l_s;

        target_body.AddForce(l_force, ForceMode.Impulse);

        duration_of_attraction = 0;

        LaunchEffects();

        being_launched = true;
    }

    public override void Released()
    {
        base.Released();

        collision_point = Vector3.zero;
    }

}
