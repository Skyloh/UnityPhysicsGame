using UnityEngine;

public class GravityObject : MonoBehaviour
{

    protected Rigidbody target_body;

    protected bool is_attracted = false;
    protected bool is_held = false;
    protected int duration_of_attraction = 0; // increases over time when being attracted

    protected bool do_attraction = false;

    public delegate void OnLaunch(float launch_force, Vector3 pos);
    public OnLaunch ActivateEffects;

    public delegate void OnHold();
    public OnHold ActivateHoldEffects;

    public delegate void OnDrop(float launch_force, Vector3 pos); // scuffed
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

    public virtual void Launch(float l_s, bool with_curr_velo)
    {
    }

    public virtual void Attract()
    {
        do_attraction = true;

        target_body.useGravity = false;

        is_attracted = true;

        ActivateHoldEffects();
    }

    public virtual void Released()
    {
        do_attraction = false;

        is_held = false;
        
        target_body.AddForce(-target_body.velocity * 5f);
        
        target_body.useGravity = true;

        duration_of_attraction = 0;

        DeactivateHoldEffects(0f, Vector3.zero);
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
