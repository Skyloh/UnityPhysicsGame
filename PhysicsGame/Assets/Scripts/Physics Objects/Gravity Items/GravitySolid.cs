using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class GravitySolid : GravityObject
{
    private const float ATTRACTION = 8f;

    public override void Awake()
    {
        target_body = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            is_held = true;
            is_attracted = false;
        }
    }

    public override void AttractionUpdate()
    {
        target_body.AddForce((transform.position - target_body.position).normalized * duration_of_attraction); // used to be * ATTRACTION

        duration_of_attraction += 1;
    }

    public override void HeldUpdate()
    {
        target_body.AddForce((transform.position - target_body.position).normalized * ATTRACTION);

        target_body.useGravity = false;
    }

    public override void Launch(float l_s, bool with_curr_velo)
    {
        Vector3 l_force = -Camera.main.transform.forward * l_s;

        target_body.AddForce(l_force, ForceMode.Impulse);

        duration_of_attraction = 0;

        ActivateEffects(l_s, target_body.position);
    }

}
