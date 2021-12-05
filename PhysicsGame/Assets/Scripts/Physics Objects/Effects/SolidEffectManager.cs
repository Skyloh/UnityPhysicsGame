using UnityEngine;

public class SolidEffectManager : MonoBehaviour
{

    private GravitySolid me;
    private Rigidbody player;

    [SerializeField] private ParticleSystem launch_effect; // in editor
    [SerializeField] private ParticleSystem hold_effect; // in editor

    // Start is called before the first frame update
    void Start()
    {
        me = GetComponent<GravitySolid>();
        player = me.getBody();

        me.ActivateEffects += ActivateLaunchEffect;

        me.ActivateHoldEffects += ActivateHoldEffect;

        me.DeactivateHoldEffects += DeactivateHoldEffect;
    }

    void ActivateLaunchEffect(float l_force, Vector3 pos)
    {
        var main = launch_effect.main;

        main.maxParticles = (int)(Mathf.Pow(2, (l_force / 50f) * 6.6438619f)); // the magic number is log base 2 of 100

        launch_effect.transform.SetPositionAndRotation(pos - ((pos - Camera.main.transform.position).normalized / 2f), Quaternion.Euler(Camera.main.transform.eulerAngles));

        launch_effect.Play();
    }

    void DeactivateHoldEffect(float l_force, Vector3 pos) => hold_effect.Stop();

    void ActivateHoldEffect() => hold_effect.Play();
}
