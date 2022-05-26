using System.Collections;
using UnityEngine;

public class PressureButton : PuzzleEmitter
{

    private void OnTriggerEnter(Collider other)
    {
        WhenPressed();
        StartCoroutine(ActivationAnimation());
    }

    private void OnTriggerExit(Collider other)
    {
        if (once_toggle)
        {
            return;
        }

        WhenReleased();
        StartCoroutine(DeactivationAnimation());
    }

    protected override IEnumerator ActivationAnimation()
    {
        for (int i = 0; i < loops; i++)
        {
            animated_object.position = Vector3.Lerp(animated_object.position, animated_object.position - transform.up * 0.25f, 0.0125f);

            yield return new WaitForEndOfFrame();
        }
    }

    protected override IEnumerator DeactivationAnimation()
    {
        for (int i = 0; i < loops; i++)
        {
            animated_object.position = Vector3.Lerp(animated_object.position, animated_object.position + transform.up * 0.25f, 0.0125f);

            yield return new WaitForEndOfFrame();
        }
    }
}
