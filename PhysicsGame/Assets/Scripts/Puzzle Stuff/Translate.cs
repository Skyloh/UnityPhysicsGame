using System.Collections;
using UnityEngine;

public class Translate : TransformAction
{
    public override void Start()
    {
        if (TARGET == null)
        {
            TARGET = transform;
        }

        origin = TARGET.position;

        if (additive)
        {
            destination += TARGET.position;
        }

        to_value = destination;
    }

    public override IEnumerator DoTransform()
    {
        while (Vector3.Distance(TARGET.position, to_value) > 1f)
        {
            TARGET.position = Vector3.Lerp(TARGET.position, to_value, speed);

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1f);

        to_value = to_value == destination ? origin : destination;

        if (!ping_pong)
        {
            yield break;
        }

        StartCoroutine(DoTransform());
    }
}
