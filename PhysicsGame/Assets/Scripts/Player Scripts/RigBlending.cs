using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigBlending : MonoBehaviour
{
    // TO REMOVE?


    [SerializeField] Transform HEAD_BONE;
    [SerializeField] Transform ARM_BONE;

    [SerializeField] GameObject Rig;

    [SerializeField] Transform NeckAimTarget; // set to Target
    private GCore CORE;

    private void Awake()
    {
        CORE = GameObject.FindGameObjectWithTag("Gravity Core").GetComponent<GCore>();
    }

    private void Update()
    {
        NeckAimTarget.position = Vector3.Lerp(NeckAimTarget.position, CORE.getPos(), 0.125f);
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
