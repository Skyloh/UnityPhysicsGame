using UnityEngine;

public class StateLibrary : MonoBehaviour
{ 
    public static StateLibrary library; // singleton pattern go brrrrrrr

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

        Transform player_transform = gameObject.transform;
        Rigidbody player_rb = GetComponent<Rigidbody>();
        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        RigBlending blender = GetComponent<RigBlending>();

        PlayerStateMachine = GetComponent<PlayerSM>();
        MovementPlayerState = new MovementPS(Vector2.zero, player_transform, player_rb /*, linked_camera.transform*/);
        AirbornePlayerState = new AirbornePS(Vector2.zero, player_transform, player_rb);
        PrejumpPlayerState = new PrejumpPS(Vector2.zero, player_transform, player_rb);
        IdlePlayerState = new IdlePS(Vector2.zero, player_transform, player_rb);
        VaultPlayerState = new VaultPS(Vector2.zero, player_transform, player_rb, collider, blender);
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
