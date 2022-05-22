using UnityEngine;
using System.Collections;

public class GravityObject : MonoBehaviour
{

    protected Rigidbody target_body;

    protected bool is_attracted = false;
    protected bool is_held = false;
    protected int duration_of_attraction = 0; // increases over time when being attracted

    protected bool do_attraction = false;

    public delegate void OnLaunch();
    public OnLaunch LaunchEffects;

    public delegate void OnHold();
    public OnHold ActivateHoldEffects;

    public delegate void OnDrop(); // scuffed
    public OnDrop DeactivateHoldEffects;

    public virtual void Awake()
    {
    }

    private void FixedUpdate()
    {
        if (!do_attraction)
        {
            return;
        }

        if (is_attracted)
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

    public virtual void Launch(float l_s, bool with_curr_velo, Vector3 direction)
    {
    }

    public virtual void Attract()
    {
        do_attraction = is_attracted = true;

        target_body.useGravity = is_held = false;

        ActivateHoldEffects();
    }

    public virtual void Released()
    {
        do_attraction = is_held = is_attracted = false;

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

    public virtual bool isTerminal() => target_body.velocity.magnitude > 500f;
}
