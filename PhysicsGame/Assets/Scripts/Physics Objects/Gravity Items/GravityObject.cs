using UnityEngine;
using System.Collections;

public class GravityObject : MonoBehaviour
{
    public bool is_solid;

    protected Rigidbody target_body;

    protected bool is_attracted = false;
    protected bool is_held = false;
    protected bool being_launched = false;

    protected int duration_of_attraction = 0; // increases over time when being attracted

    public delegate void OnLaunch();
    public OnLaunch LaunchEffects;

    public delegate void OnStop();
    public OnStop StopLaunchEffects;

    public delegate void OnHold();
    public OnHold ActivateHoldEffects;

    public delegate void OnDrop(); 
    public OnDrop DeactivateHoldEffects;

    public virtual void Awake()
    {
        LaunchEffects += () => being_launched = true;

        StopLaunchEffects += () => being_launched = false;
    }

    private void FixedUpdate()
    {
        if (being_launched && (target_body.velocity.magnitude < 0.05f || beingInteractedWith()))
        {
            StopLaunchEffects();
            being_launched = false;
        }

        else if (is_attracted)
        {
            AttractionUpdate();
        }

        else if (is_held)
        {
            HeldUpdate();
        }
    }

    public virtual void AttractionUpdate()
    {
    }

    public virtual void HeldUpdate()
    {
    }

    // Release is called before Launch
    public virtual void Launch(float l_s, bool with_curr_velo, Vector3 direction)
    {
    }

    public virtual void Attract()
    {
        is_attracted = true;

        target_body.useGravity = is_held = false;

        ActivateHoldEffects();
    }

    public virtual void Released()
    {
        is_held = is_attracted = false;

        target_body.useGravity = true;

        target_body.AddForce(-target_body.velocity * 5f);

        duration_of_attraction = 0;

        DeactivateHoldEffects();
    }

    public int getID()
    {
        return GetInstanceID();
    }

    public Rigidbody getBody()
    {
        return target_body;
    }

    public bool beingInteractedWith()
    {
        return is_attracted || is_held;
    }

}
