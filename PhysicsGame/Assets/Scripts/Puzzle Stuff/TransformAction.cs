using System.Collections;
using UnityEngine;

public class TransformAction : MonoBehaviour
{
    protected Vector3 origin;
    [SerializeField] protected Vector3 destination;
    protected Vector3 to_value;

    [SerializeField] protected bool additive;
    [SerializeField] protected bool ping_pong = true; // transform over and over if true, if false, transform once per Activation

    [SerializeField] protected float speed = 0.0125f;

    [SerializeField] protected Transform TARGET; // leave NULL if targeting gameObject. Specify object for additional transforms.

    public virtual void Start()
    {
    }

    public void ToggleDoTransform(bool on_off)
    {
        //StopAllCoroutines(); 

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
