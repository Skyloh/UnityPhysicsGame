using System.Collections;
using UnityEngine;

public class KillableObject : MonoBehaviour
{

    public delegate void OnKill();
    public OnKill WhenObjectKilled; // uncomment instances when implementation is added

    public bool is_currently_dying = false;

    protected virtual IEnumerator Start()
    {
        yield return null;
    }

    protected virtual void KillInstantly()
    {
        // WhenObjectKilled();
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

        // needed bc if a cube from an auto-generate spawner dies in a KillBarrier, DoKill's IEnumerator gets called twice,
        // and therefore starts acting on a dying object. (causes null ref errors INCONSISTENTLY)
        else if (!is_currently_dying)
        {
            StartCoroutine(KillAfterEffect());

            is_currently_dying = true;
        }
    }
}
