﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateLibrary : MonoBehaviour
{ 
    public static StateLibrary library; // singleton pattern go brrrrrrr

    // Gravity State Machine and States
    public GravitySM GravityStateMachine;

    public DefaultGS DefaultGravityState;
    public ChargeGS ChargeGravityState;


    // Player State Machine and States
    public PlayerSM PlayerStateMachine;

    public MovementPS MovementPlayerState;
    public AirbornePS AirbornePlayerState;
    public PrejumpPS PrejumpPlayerState;
    public IdlePS IdlePlayerState;
    public VaultPS VaultPlayerState;

    
    void Awake()
    {
        library = this;

        FPSCam linked_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FPSCam>();
        Transform player_transform = gameObject.transform;
        Rigidbody player_rb = GetComponent<Rigidbody>();
        CapsuleCollider collider = GetComponent<CapsuleCollider>();


        GravityStateMachine = GetComponent<GravitySM>();
        DefaultGravityState = new DefaultGS(null, linked_camera, player_transform);
        ChargeGravityState = new ChargeGS(null, linked_camera, player_transform);

        PlayerStateMachine = GetComponent<PlayerSM>();
        MovementPlayerState = new MovementPS(Vector2.zero, player_transform, player_rb /*, linked_camera.transform*/);
        AirbornePlayerState = new AirbornePS(Vector2.zero, player_transform, player_rb);
        PrejumpPlayerState = new PrejumpPS(Vector2.zero, player_transform, player_rb);
        IdlePlayerState = new IdlePS(Vector2.zero, player_transform, player_rb);
        VaultPlayerState = new VaultPS(Vector2.zero, player_transform, player_rb, collider);
    }

    public GravityState MatchStringToGS(string search)
    {
        switch (search)
        {
            case "DefaultGS":
                return DefaultGravityState;

            case "ChargeGS":
                return ChargeGravityState;

            default:
                Debug.LogError("Did you mess up the string query?: " + search);
                return null;
        }
    }

    public PlayerState MatchStringToPS(string search) // make this into a hash please :blush:
    {
        switch (search)
        {
            case "MovementPS":
                return MovementPlayerState;

            case "AirbornePS":
                return AirbornePlayerState;

            case "PrejumpPS":
                return PrejumpPlayerState;

            case "IdlePS":
                return IdlePlayerState;

            case "VaultPS":
                return VaultPlayerState;

            default:
                Debug.LogError("Did you mess up the string query?: " + search);
                return null;
        }
    }

}