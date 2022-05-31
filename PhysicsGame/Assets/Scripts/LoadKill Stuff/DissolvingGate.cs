using System.Collections;
using UnityEngine;

public class DissolvingGate : KillBarrierScript
{
    [ColorUsage(true, true)] // allows for HDR access in editor
    [SerializeField] Color block_kill_color;

    Renderer render_component;


    float parity = 1f;
    float y_sdest;

    private void Awake()
    {
        y_sdest = transform.localScale.y;

        render_component = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        StopAllCoroutines();

        render_component.enabled = true;

        StartCoroutine(LerpScale());
        
    }

    private void OnDisable()
    {
        if (gameObject.activeInHierarchy)
        {
            StopAllCoroutines();
            StartCoroutine(LerpScale());
        }

    }

    private void Start()
    {
        if (!kills_player)
        {
            render_component.material.SetColor("_DissolverColor", block_kill_color);
        }
    }

    private IEnumerator LerpScale()
    {
        Vector3 cache = new Vector3();

        float progress = 0f;

        while (progress < 0.95f)
        {
            progress = Mathf.Lerp(progress, 1f, 0.0125f);

            cache.Set(transform.localScale.x, progress * y_sdest * parity + (1f - progress) * y_sdest * (1f - parity), transform.localScale.z);

            transform.localScale = cache;

            yield return new WaitForEndOfFrame();
        }

        if (parity == 0f)
        {
            parity = 1f;

            render_component.enabled = false;
        }
        else
        {
            parity = 0f;
        }

    }
}
