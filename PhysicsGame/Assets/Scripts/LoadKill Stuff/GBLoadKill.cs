using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script handles what to do when a gravity block is to be destroyed.
// it extends KillableObject so that a generic method can be called rather than
// a specialized one.
// GBs have a spawning animation, and therefore need Start implementation.
// They do not need OnEnable/OnDisable, as they are never enabled or disabled
// in a scene.

public class GBLoadKill : KillableObject
{
    Material destroyEffectMaterial;

    int p_id;
    BoxCollider box_collider;


    public delegate void OnKill();
    public OnKill WhenObjectKilled; // uncomment instances when implementation is added

    private void Awake()
    {
        destroyEffectMaterial = GetComponent<Renderer>().material;

        p_id = Shader.PropertyToID("_Process");

        box_collider = GetComponent<BoxCollider>();
        
    }

    protected override IEnumerator Start()
    {
        destroyEffectMaterial.SetFloat("_Scale", Random.Range(3f,15f));

        float current_progress = 1f;

        while (current_progress > -0.9f)
        {
            current_progress = Mathf.Lerp(current_progress, -1f, 0.025f);

            destroyEffectMaterial.SetFloat(p_id, current_progress);

            yield return new WaitForSeconds(.01f);
        }

        destroyEffectMaterial.SetFloat(p_id, -1f);
    }

    // lazy goon moment
    protected override IEnumerator KillAfterEffect()
    {
        box_collider.attachedRigidbody.isKinematic = false;
        box_collider.enabled = false;

        float current_progress = -1f;

        while (current_progress < 0.9f)
        {
            current_progress = Mathf.Lerp(current_progress, 1f, 0.025f);

            destroyEffectMaterial.SetFloat(p_id, current_progress);

            yield return new WaitForSeconds(0.01f);
        }

        destroyEffectMaterial.SetFloat(p_id, 1f);

        // WhenObjectKilled();
        Destroy(gameObject);
    }
}
