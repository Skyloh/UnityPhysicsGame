using UnityEngine;
using UnityEngine.Animations.Rigging;
using System.Collections;

public class RigBlending : MonoBehaviour
{

    // multi-target constraint script for the arm and head
    // not visible to the player except through the shadoows or mirrors and such
    // because of that i dont really care about this :)

    [SerializeField] Transform HEAD_BONE;
    [SerializeField] Transform ARM_BONE; // this doesnt really work, i mean the rig multiaim.
    // fix it maybe?
    // or remove it entirely, i guess. GJ past goon, i think that was a GREAT idea. :/

    [SerializeField] Rig rig;

    [SerializeField] Transform NeckAimTarget; // set to Target
    private Transform CORE;

    private void Awake()
    {
        CORE = GameObject.FindGameObjectWithTag("Gravity Core").transform;
        // any reason why this is Awake and not Start? no? ok cool
    }

    private void Update()
    {
        NeckAimTarget.position = CORE.position; // Vector3.Lerp(NeckAimTarget.position, CORE.position, 0.125f); why a lerp?
    }

    private void OnEnable()
    {
        rig.weight = 1f;
    }

    private void OnDisable()
    {
        rig.weight = 0f;
    }

}
