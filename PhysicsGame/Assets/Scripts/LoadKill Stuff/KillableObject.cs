using System.Collections;
using UnityEngine;

public class KillableObject : MonoBehaviour
{

    public delegate void OnKill();
    public OnKill WhenObjectKilled;

    protected virtual IEnumerator Start()
    {
        yield return null;
    }

    protected virtual void KillInstantly()
    {
        WhenObjectKilled();
        Destroy(gameObject);
    }

    protected virtual IEnumerator KillAfterEffect()
    {
        yield return null;
    }

    public void DoKill(bool do_kill_instantly)
    {
        if (do_kill_instantly)
        {
            KillInstantly();
        }

        else
        {
            StartCoroutine(KillAfterEffect());
        }
    }
}
