using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityObject : MonoBehaviour
{

    private Rigidbody my_body;
    private Transform CORE_LOCATION;

    private bool is_attracted = false;
    private bool is_held = false;
    private int duration_of_attraction = 0; // increases over time when being attracted

    private const float STRENGTH = 25f;
    private const float ATTRACTION = 8f;
    private const float MINIMUM_HELD_DISTANCE = 1f;

    void Start()
    {
        my_body = GetComponent<Rigidbody>();
        CORE_LOCATION = GameObject.FindGameObjectWithTag("Gravity Core").transform;
    }

    private void FixedUpdate()
    {
        if (is_attracted)
        {
            my_body.AddForce(-(transform.position - CORE_LOCATION.position).normalized * duration_of_attraction); // used to be * ATTRACTION

            duration_of_attraction += 1;

            is_held = (transform.position - CORE_LOCATION.position).magnitude < MINIMUM_HELD_DISTANCE;
        }

        if (is_held)
        {
            is_attracted = false;
            
            transform.position = Vector3.Lerp(transform.position, CORE_LOCATION.position, 0.125f);

            // ISSUE: clipping
            // MAKE SURE TO CHECK IF THE NEXT POSITION WOULDN'T BE CLIPPING THROUGH A WALL!
            // This occurs when slamming an object into a thin layer, as it will then lerp through the layer.

            // ISSUE 2: funky physics
            // grabbing a block and standing on it while it's attracting has weird bouncing physics.
            // either figure out why and stop it, or ignore all physics with the player from that object.

            my_body.AddForce(-(transform.position - CORE_LOCATION.position).normalized * ATTRACTION);

        }
    }

    public void Attract()
    {
        my_body.useGravity = false;

        is_attracted = true;
    }

    public void Released()
    {
        is_held = false;

        my_body.AddForce(-my_body.velocity * 5f);
        
        my_body.useGravity = true;

        duration_of_attraction = 0;
    }

    public void Launch(float l_s, bool with_curr_velo)
    {
        Vector3 curr_velo = with_curr_velo ? my_body.velocity : Vector3.zero;
        
        my_body.AddForce((Camera.main.transform.forward * l_s) + curr_velo, ForceMode.Impulse); // minor ISSUE: maybe look into using ExplosionForce here?

        duration_of_attraction = 0;
    }

    public int getID()
    {
        return GetInstanceID();
    }
}
