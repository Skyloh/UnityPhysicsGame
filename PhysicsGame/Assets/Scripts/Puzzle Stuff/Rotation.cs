using System.Collections;
using UnityEngine;

public class Rotation : TransformAction
{
    public override void Start()
    {
        origin = transform.eulerAngles;

        if (additive)
        {
            destination += transform.eulerAngles;
        }

        to_value = destination;
    }

    public override IEnumerator DoTransform()
    {
        while (Vector3.Distance(transform.eulerAngles, to_value) > 5f)
        {
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, to_value, speed);

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1f);

        to_value = to_value == destination ? origin : destination;

        StartCoroutine(DoTransform());
    }
}
