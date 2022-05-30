using UnityEngine;

public class PuzzleReceiver : MonoBehaviour
{
    protected bool is_active;

    public delegate void ActivationEffects();
    public ActivationEffects WhenActivated;

    public delegate void DeactivationEffects();
    public DeactivationEffects WhenDeactivated;

    public virtual void Activate()
    {
        is_active = true;

        PeekCameraScript.instance.Goto(transform);
    }

    public virtual void Deactivate()
    {
        is_active = false;
    }
}
