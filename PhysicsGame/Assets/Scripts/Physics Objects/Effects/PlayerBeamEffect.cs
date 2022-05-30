using System.Collections;
using UnityEngine;

public class PlayerBeamEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem effect;
    [SerializeField] ParticleSystem.MinMaxGradient RED_START;
    [SerializeField] ParticleSystem.MinMaxGradient BLUE_START;

    Transform initial;

    [SerializeField] GravityControl controller;

    private void OnEnable()
    {
        controller.TriggerVSFXs += Fire;
    }

    private void OnDisable()
    {
        controller.TriggerVSFXs -= Fire;
    }

    private void Start()
    {
        initial = GameObject.FindGameObjectWithTag("Player").transform;

        transform.position = initial.position + Vector3.up * 2f;
    }

    private void Fire(bool type, Vector3 direction, float distance)
    {
        ResetSystem();

        var main = effect.main;

        main.startColor = type ? BLUE_START : RED_START;

        transform.LookAt(transform.position + direction);

        StartCoroutine(MoveSystem(distance, transform.forward));
    }

    private IEnumerator MoveSystem(float distance, Vector3 direction)
    {
        float traveled = 0f;

        effect.Play();

        while (traveled < distance)
        {
            traveled += 1f;

            transform.position += direction;

            yield return new WaitForEndOfFrame();
        }

        effect.Stop();
    }

    private void ResetSystem()
    {
        StopCoroutine(MoveSystem(0f, Vector3.zero));
        effect.Stop();
        transform.position = initial.position + Vector3.up * 2f;
    }
}
