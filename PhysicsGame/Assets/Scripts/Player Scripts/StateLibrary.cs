using UnityEngine;

public class StateLibrary : MonoBehaviour
{ 
    public static StateLibrary library; // singleton pattern go brrrrrrr

    // Player State Machine and States
    public PlayerSM PlayerStateMachine;

    public MovementPS MovementPlayerState;  // NOTE TO FUTURE SELF:
    public AirbornePS AirbornePlayerState;  // i can just make this an array of PlayerStates, since it's immutable.
    public PrejumpPS PrejumpPlayerState;    // as for accessing them, maybe I can make an enum (in playerstate) and then use
    public IdlePS IdlePlayerState;          // that to transition between states with MatchEnumtoPS (after the enum input is cast to int)
    public SprintPS SprintPlayerState;
    public WallGrabPS WallGrabState;
    
    void Awake()
    {
        library = this;

        Transform player_transform = gameObject.transform;
        Rigidbody player_rb = GetComponent<Rigidbody>();

        PlayerStateMachine = GetComponent<PlayerSM>();
        MovementPlayerState = new MovementPS(Vector2.zero, player_transform, player_rb);
        AirbornePlayerState = new AirbornePS(Vector2.zero, player_transform, player_rb);
        PrejumpPlayerState = new PrejumpPS(Vector2.zero, player_transform, player_rb);
        IdlePlayerState = new IdlePS(Vector2.zero, player_transform, player_rb);
        SprintPlayerState = new SprintPS(Vector2.zero, player_transform, player_rb);
        WallGrabState = new WallGrabPS(Vector2.zero, player_transform, player_rb);
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

            case "SprintPS":
                return SprintPlayerState;

            case "WallGrabPS":
                return WallGrabState;

            default:
                Debug.LogError("Did you mess up the string query?: " + search);
                return null;
        }
    }

}
