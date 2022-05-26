using System.Collections;
using UnityEngine;

public class TransformAction : MonoBehaviour
{
    protected Vector3 origin;
    [SerializeField] protected Vector3 destination;
    protected Vector3 to_value;

    [SerializeField] protected bool additive;

    [SerializeField] protected float speed = 0.0125f;

    public virtual void Start()
    {
    }

    public void ToggleDoTransform(bool on_off)
    {
        StopAllCoroutines();

        if (on_off)
        {
            StartCoroutine(DoTransform());
        }
        else
        {
            StopCoroutine(DoTransform());
        }
    }

    public virtual IEnumerator DoTransform()
    {
        yield break;
    }

}
