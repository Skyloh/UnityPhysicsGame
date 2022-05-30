using System.Collections;
using UnityEngine;

public class ManualButton : PuzzleEmitter
{
    bool has_been_pressed = false;

    private void Start()
    {
        loops = 25;
    }

    public bool PressButton()
    {
        if (!has_been_pressed)
        {
            StartCoroutine(ActivationAnimation());

            return true;
        }

        return false;
    }

    protected override IEnumerator ActivationAnimation()
    {
        yield return new WaitForSeconds(1f); // length of anim to get to button-press

        WhenPressed();

        has_been_pressed = true; // i would put this in the delegate, but idk if i can remove lambda expressions with -=

        for (int i = 0; i < loops; i++)
        {
            animated_object.position = Vector3.Lerp(animated_object.position, animated_object.position - Vector3.up * 0.1f, 0.0125f);

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(DeactivationAnimation());
    }

    protected override IEnumerator DeactivationAnimation()
    {
        if (once_toggle) // guard statements go ooga
        {
            yield break;
        }

        WhenReleased();
        
        for (int i = 0; i < loops; i++)
        {
            animated_object.position = Vector3.Lerp(animated_object.position, animated_object.position + Vector3.up * 0.1f, 0.0125f);

            yield return new WaitForEndOfFrame();
        }

        has_been_pressed = false;
    }
}
