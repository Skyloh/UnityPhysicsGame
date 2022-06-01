using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PuzzleEmitter : MonoBehaviour
{
    [SerializeField] private List<PuzzleReceiver> linked = new List<PuzzleReceiver>();
    int index = 0;

    [SerializeField] protected bool once_toggle = false; // you need to activate this emitter only once.

    protected delegate void OnActivation();
    protected OnActivation WhenPressed;

    protected delegate void OnDeactivation();
    protected OnDeactivation WhenReleased;

    [SerializeField] protected Transform animated_object;
    protected int loops = 50;

    [SerializeField] ParticleSystem PULSE_SYSTEM;
    Transform PULSE_OBJECT;


    private void OnEnable()
    {
        PULSE_OBJECT = PULSE_SYSTEM.transform;

        if (linked.Count == 0)
        {
            Debug.LogError("A Puzzle Emitter has no linked objects!");
            gameObject.SetActive(false);
            return;
        }

        foreach (PuzzleReceiver pz in linked)
        {
            WhenPressed += pz.Activate;
            WhenReleased += pz.Deactivate;
        }

        StartCoroutine(EmitPulse(2f));
    }

    private void OnDisable()
    {
        if (linked.Count == 0)
        {
            Debug.LogError("A Puzzle Emitter has no linked objects! " + gameObject.name);
            gameObject.SetActive(false);
            return;
        }

        foreach (PuzzleReceiver pz in linked)
        {
            WhenPressed -= pz.Activate;
            WhenReleased -= pz.Deactivate;
        }

        StopCoroutine(EmitPulse(0f));
    }

    protected virtual IEnumerator ActivationAnimation()
    {
        yield break;
    }

    protected virtual IEnumerator DeactivationAnimation()
    {
        yield break;
    }

    private IEnumerator EmitPulse(float delay)
    {
        while (gameObject.activeInHierarchy)
        {
            PULSE_SYSTEM.gameObject.SetActive(true);

            PULSE_OBJECT.position = transform.position;
            PULSE_OBJECT.LookAt(transform.position + transform.up);

            Vector3 destination = linked[index].transform.position;
            index = (index + 1) % (linked.Count);

            while (Vector3.Distance(PULSE_OBJECT.position, destination) > 1f)
            {
                PULSE_OBJECT.position = Vector3.Lerp(PULSE_OBJECT.position, destination, 0.0125f);
                
                yield return new WaitForEndOfFrame();
            }

            PULSE_SYSTEM.TriggerSubEmitter(0);

            yield return new WaitForSeconds(2f);

            PULSE_OBJECT.gameObject.SetActive(false);

            yield return new WaitForSeconds(delay);
        }
        

    }

}
