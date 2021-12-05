using UnityEngine;

public class EffectManager : MonoBehaviour
{

    private GravityObject me;
    private Rigidbody rbody;

    private TrailRenderer launch_trail;
    [SerializeField] private ParticleSystem launch_effect; // in editor
    [SerializeField] private ParticleSystem hold_effect; // in editor

    // Start is called before the first frame update
    void Start()
    {
        me = GetComponent<GravityObject>();
        rbody = me.getBody();

        launch_trail = GetComponent<TrailRenderer>();

        launch_trail.emitting = false;

        me.ActivateEffects += ActivateLaunchEffect;
        me.ActivateEffects += ActivateLaunchTrail;
        me.ActivateEffects += DeactivateHoldEffect;

        me.DeactivateHoldEffects += DeactivateHoldEffect;

        me.ActivateHoldEffects += ActivateHoldEffect;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ((launch_trail.emitting && rbody.velocity.magnitude < 0.05f) || me.beingInteractedWith())
        {
            launch_trail.emitting = false;
        }
    }

    void ActivateLaunchEffect(float l_force, Vector3 pos)
    {
        var main = launch_effect.main;

        main.maxParticles = (int)(Mathf.Pow(2, (l_force / 50f) * 6.6438619f)); // the magic number is log base 2 of 100
        
        launch_effect.transform.SetPositionAndRotation(pos + (pos - Camera.main.transform.position).normalized, Quaternion.Euler(Camera.main.transform.eulerAngles));

        launch_effect.Play();
    }

    void ActivateLaunchTrail(float l_force, Vector3 pos ) => launch_trail.emitting = true;

    void DeactivateHoldEffect(float l_force, Vector3 pos) => hold_effect.Stop();

    void ActivateHoldEffect() => hold_effect.Play();
}
