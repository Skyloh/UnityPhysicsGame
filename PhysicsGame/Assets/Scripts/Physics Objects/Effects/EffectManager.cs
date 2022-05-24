using UnityEngine;
using System.Collections;

public class EffectManager : MonoBehaviour
{

    private GravityObject me;
    private Rigidbody rbody;

    private Transform perspective;

    private TrailRenderer launch_trail;
    [SerializeField] private ParticleSystem launch_effect; // in editor
    [SerializeField] private ParticleSystem hold_effect; // in editor

    private void Awake()
    {
        me = GetComponent<GravityObject>();
    }

    IEnumerator Start()
    {
        if (me.is_solid)
        {
            GameObject player_cache = GameObject.FindGameObjectWithTag("Player");

            rbody = player_cache.GetComponent<Rigidbody>();

            launch_trail = player_cache.GetComponent<TrailRenderer>();

        }

        else
        {
            rbody = me.getBody();

            launch_trail = GetComponent<TrailRenderer>();
        }

        yield return new WaitForEndOfFrame(); // wait a frame for the player cam to be loaded

        foreach (Camera cam in Camera.allCameras)
        {
            if (cam.tag == "PlayerCamera")
            {
                perspective = cam.transform;
                
                yield break;
            }
        }

    }

    // Start is called before the first frame update
    void OnEnable()
    {
        me.LaunchEffects += ActivateLaunchEffect;
        me.LaunchEffects += ActivateLaunchTrail;
        me.LaunchEffects += DeactivateHoldEffect;

        me.StopLaunchEffects += DeactivateLaunchEffect;

        me.DeactivateHoldEffects += DeactivateHoldEffect;

        me.ActivateHoldEffects += ActivateHoldEffect;
    }
    void OnDisable() // includes OnDestroy
    {
        Desubscribe();
    }

    private void Desubscribe()
    {
        me.LaunchEffects -= ActivateLaunchEffect;
        me.LaunchEffects -= ActivateLaunchTrail;
        me.LaunchEffects -= DeactivateHoldEffect;

        me.StopLaunchEffects -= DeactivateLaunchEffect;

        me.DeactivateHoldEffects -= DeactivateHoldEffect;

        me.ActivateHoldEffects -= ActivateHoldEffect;
    }

    void ActivateLaunchEffect()
    {
        Vector3 pos = me.is_solid ? perspective.position : rbody.position - perspective.position.normalized;

        launch_effect.transform.SetPositionAndRotation(pos, transform.rotation);

        launch_effect.Play();
    }

    void DeactivateLaunchEffect() => launch_trail.emitting = false;

    void ActivateLaunchTrail() => launch_trail.emitting = true;

    void DeactivateHoldEffect() => hold_effect.Stop();

    void ActivateHoldEffect()
    {
        //hold_effect.transform.LookAt(transform.position + transform.forward);

        hold_effect.Play();
    }
}
