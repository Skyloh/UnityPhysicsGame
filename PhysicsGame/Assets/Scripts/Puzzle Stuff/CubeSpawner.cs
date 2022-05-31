using System.Collections;
using UnityEngine;

public class CubeSpawner : PRTransform
{
    [SerializeField] bool spawn_on_awake = true;
    [SerializeField] GameObject fabby;

    bool stop_input = false;

    KillableObject current;

    protected override void Start()
    {
        base.Start();

        if (spawn_on_awake)
        {
            StartCoroutine(SpawnCube());
        }
    }

    public override void Activate()
    {
        if (stop_input)
        {
            return;
        }

        PeekCameraScript.instance.Goto(transform);;

        stop_input = true;

        StartCoroutine(SpawnCube());
    }

    private IEnumerator SpawnCube()
    {
        if (current != null)
        {
            current.DoKill(false);
        }

        GameObject cache = GameObject.Instantiate(fabby, transform.position + transform.up * 2f, Quaternion.identity);

        current = cache.GetComponent<KillableObject>();

        yield return new WaitForSeconds(1f);

        stop_input = false;
    }

}
