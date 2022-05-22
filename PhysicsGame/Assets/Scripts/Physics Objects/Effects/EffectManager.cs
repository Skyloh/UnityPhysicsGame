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
        rbody = me.getBody();

        launch_trail = GetComponent<TrailRenderer>(); // WORK FROM HERE
    }

    IEnumerator Start()
    {
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
        launch_trail.emitting = false;

        me.LaunchEffects += ActivateLaunchEffect;
        me.LaunchEffects += ActivateLaunchTrail;
        me.LaunchEffects += DeactivateHoldEffect;

        me.DeactivateHoldEffects += DeactivateHoldEffect;

        me.ActivateHoldEffects += ActivateHoldEffect;
    }
    void OnDisable() // includes OnDestroy
    {
        launch_trail.emitting = false;

        Desubscribe();
    }

    private void Desubscribe()
    {
        me.LaunchEffects -= ActivateLaunchEffect;
        me.LaunchEffects -= ActivateLaunchTrail;
        me.LaunchEffects -= DeactivateHoldEffect;

        me.DeactivateHoldEffects -= DeactivateHoldEffect;

        me.ActivateHoldEffects -= ActivateHoldEffect;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ((launch_trail.emitting && rbody.velocity.magnitude < 0.05f) || me.beingInteractedWith())
        {
            launch_trail.emitting = false;
        }
    }

    void ActivateLaunchEffect()
    {
        Vector3 pos = rbody.position - perspective.position.normalized;

        launch_effect.transform.SetPositionAndRotation(pos, transform.rotation);

        launch_effect.Play();
    }

    void ActivateLaunchTrail() => launch_trail.emitting = true;

    void DeactivateHoldEffect() => hold_effect.Stop();

    void ActivateHoldEffect() => hold_effect.Play();
}
