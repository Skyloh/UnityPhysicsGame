using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class KillBarrierScript : MonoBehaviour
{
    [SerializeField] private bool instant_kill = false;

    [SerializeField] protected bool kills_player = true;

    [SerializeField] private bool kills_objects = true;

    private GravityControl playerControl; // bad, but i dont want to think abt a better way rn.

    private void Start()
    {
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<GravityControl>();
    }

    private void OnTriggerEnter(Collider other)
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
