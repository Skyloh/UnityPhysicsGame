using System.Collections;
using UnityEngine;

public class AnimatedDecalScript : MonoBehaviour
{
    [SerializeField] protected Transform to_affect;
    [SerializeField] float baseRandom = 0.75f;
    [SerializeField] float maxRandom = 1.5f;

    float sinOfTime;

    private void Start()
    {
        StartCoroutine(Animate(Random.Range(baseRandom, maxRandom)));
    }

    protected virtual IEnumerator Animate(float speed)
    {
        float float_height = speed / 300f; // division :flushed:

        while (true)
        {
            sinOfTime = Mathf.Sin(Time.time * speed);

            transform.position += transform.up * sinOfTime * float_height;

            to_affect.Rotate(Vector3.down * speed * 0.5f * (2 + sinOfTime));

            yield return new WaitForEndOfFrame();
        }
    }
}
