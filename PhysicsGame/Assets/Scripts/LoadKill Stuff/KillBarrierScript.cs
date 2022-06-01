using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class KillBarrierScript : MonoBehaviour
{
    [SerializeField] protected bool instant_kill = false;

    [SerializeField] protected bool kills_player = true;

    [SerializeField] protected bool kills_objects = true;

    protected GravityControl playerControl; // bad, but i dont want to think abt a better way rn.

    protected virtual void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        playerControl = player.GetComponent<GravityControl>();

    }

    protected void OnTriggerEnter(Collider other)
    {
        KillableObject killableObject = other.GetComponent<KillableObject>();

        if (killableObject == null)
        {
            Debug.LogWarning("An unrelated object has entered this trigger: " + other.name);

            return;
        }

        if (!kills_player && other.tag == "Player")
        {
            return;
        }

        else if (!kills_objects && other.tag == "GravityBlock")
        {
            return;
        }

        playerControl.DropItem();

        killableObject.DoKill(instant_kill);
    }
}
