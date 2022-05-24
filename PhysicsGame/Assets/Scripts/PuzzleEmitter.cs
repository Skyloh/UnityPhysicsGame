using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PuzzleEmitter : MonoBehaviour
{
    [SerializeField] private List<PuzzleReceiver> linked = new List<PuzzleReceiver>();
    int index = 0;

    private delegate void OnActivation();
    private OnActivation WhenPressed;

    private delegate void OnDeactivation();
    private OnDeactivation WhenReleased;

    [SerializeField] Transform animated_object;
    int loops = 50;

    [SerializeField] Transform PULSE_OBJECT;

    private void Start()
    {
        if(linked.Count == 0)
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

    private void OnTriggerEnter(Collider other)
    {
        WhenPressed();
        StartCoroutine(ActivationAnimation());
    }

    private void OnTriggerExit(Collider other)
    {
        WhenReleased();
        StartCoroutine(DeactivationAnimation());
    }

    protected IEnumerator ActivationAnimation()
    {
        for (int i = 0; i < loops; i++)
        {
            animated_object.position = Vector3.Lerp(animated_object.position, animated_object.position - transform.up * 0.25f, 0.0125f);

            yield return new WaitForEndOfFrame();
        }
    }

    protected IEnumerator DeactivationAnimation()
    {
        for (int i = 0; i < loops; i++)
        {
            animated_object.position = Vector3.Lerp(animated_object.position, animated_object.position + transform.up * 0.25f, 0.0125f);

            yield return new WaitForEndOfFrame();
        }
    }

    protected IEnumerator EmitPulse(float delay)
    {
        while (gameObject.activeInHierarchy)
        {
            PULSE_OBJECT.gameObject.SetActive(true);

            PULSE_OBJECT.position = transform.position;
            PULSE_OBJECT.LookAt(transform.position + transform.up);

            Vector3 destination = linked[index].transform.position;
            index = (index + 1) % (linked.Count);

            while (Vector3.Distance(PULSE_OBJECT.position, destination) > 1f)
            {
                PULSE_OBJECT.position = Vector3.Lerp(PULSE_OBJECT.position, destination, 0.0125f);
                

                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(2f);

            PULSE_OBJECT.gameObject.SetActive(false);

            yield return new WaitForSeconds(delay);
        }
        

    }

}
