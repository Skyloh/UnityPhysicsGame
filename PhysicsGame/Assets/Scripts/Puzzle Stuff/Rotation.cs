using System.Collections;
using UnityEngine;

public class Rotation : TransformAction
{
    public override void Start()
    {
        origin = TARGET.eulerAngles;

        if (additive)
        {
            destination += TARGET.eulerAngles;
        }

        to_value = destination;
    }

    public override IEnumerator DoTransform()
    {
        while (Vector3.Distance(TARGET.eulerAngles, to_value) > 1f)
        {
            TARGET.eulerAngles = Vector3.Lerp(TARGET.eulerAngles, to_value, speed);

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
