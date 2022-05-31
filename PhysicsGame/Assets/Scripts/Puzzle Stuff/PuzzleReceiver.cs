using UnityEngine;

public class PuzzleReceiver : MonoBehaviour
{
    public delegate void ActivationEffects();
    public ActivationEffects WhenActivated;

    public delegate void DeactivationEffects();
    public DeactivationEffects WhenDeactivated;

    [SerializeField] private MonoBehaviour BASEONLY_at; // leave null if unused/no inherited scripts are on gameObject
    [SerializeField] private bool BASEONLY_sd;

    private void Start()
    {
        if (BASEONLY_at != null && BASEONLY_sd)
        {
            BASEONLY_at.enabled = false;
        }
    }

    public virtual void Activate()
    {
        if (BASEONLY_at != null)
        {
            BASEONLY_at.enabled = !BASEONLY_at.enabled;

            return;
        }

        gameObject.SetActive(true);
    }

    public virtual void Deactivate()
    {
        if (BASEONLY_at != null)
        {
            BASEONLY_at.enabled = !BASEONLY_at.enabled;

            return;
        }

        gameObject.SetActive(false);
    }
}
