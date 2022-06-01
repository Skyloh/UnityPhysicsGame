using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillScript : AnimatedDecalScript
{
    protected override IEnumerator Animate(float speed)
    {
        while (true)
        {
            to_affect.Rotate(Vector3.up * speed * 0.5f);

            yield return new WaitForEndOfFrame();
        }
    }
}
