using System.Collections;
using UnityEngine;

public class Translate : TransformAction
{
    public override void Start()
    {
        origin = transform.position;

        if (additive)
        {
            destination += transform.position;
        }

        to_value = destination;
    }

    public override IEnumerator DoTransform()
    {
        while (Vector3.Distance(transform.position, to_value) > 1f)
        {
            transform.position = Vector3.Lerp(transform.position, to_value, speed);

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1f);

        to_value = to_value == destination ? origin : destination;

        StartCoroutine(DoTransform());
    }
}
