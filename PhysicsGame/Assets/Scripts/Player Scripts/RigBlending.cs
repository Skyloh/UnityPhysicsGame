using UnityEngine;

public class RigBlending : MonoBehaviour
{

    // multi-target constraint script for the arm and head
    // not visible to the player except through the shadoows or mirrors and such
    // because of that i dont really care about this :)

    [SerializeField] Transform HEAD_BONE;
    [SerializeField] Transform ARM_BONE;

    [SerializeField] GameObject Rig;

    [SerializeField] Transform NeckAimTarget; // set to Target
    private Transform CORE;

    private void Awake()
    {
        CORE = GameObject.FindGameObjectWithTag("Gravity Core").transform;
    }

    private void Update()
    {
        NeckAimTarget.position = Vector3.Lerp(NeckAimTarget.position, CORE.position, 0.125f);
    }

    private void OnEnable()
    {
        Rig.SetActive(true);
    }

    private void OnDisable()
    {
        Rig.SetActive(false);
    }

}
