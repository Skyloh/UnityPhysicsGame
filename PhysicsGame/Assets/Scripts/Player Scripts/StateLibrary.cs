using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public WallDropPS WallDropState;
    public WallGetupPS WallGetupState;
    public ButtonPressPS ButtonPressState;

    private Queue<PlayerState> guard_exit_toggle_queue = new Queue<PlayerState>();

    void Awake()
    {
        library = this;

        Transform player_transform = gameObject.transform;
        Rigidbody player_rb = GetComponent<Rigidbody>();

        PlayerStateMachine = GetComponent<PlayerSM>();
        MovementPlayerState = new MovementPS(Vector2.zero, player_transform, player_rb);
        AirbornePlayerState = new AirbornePS(Vector2.zero, player_transform, player_rb, GetComponent<CapsuleCollider>());
        PrejumpPlayerState = new PrejumpPS(Vector2.zero, player_transform, player_rb);
        IdlePlayerState = new IdlePS(Vector2.zero, player_transform, player_rb);
        SprintPlayerState = new SprintPS(Vector2.zero, player_transform, player_rb);
        WallGrabState = new WallGrabPS(Vector2.zero, player_transform, player_rb);
        WallDropState = new WallDropPS(Vector2.zero, player_transform, player_rb);
        WallGetupState = new WallGetupPS(Vector2.zero, player_transform, player_rb);
        ButtonPressState = new ButtonPressPS(Vector2.zero, player_transform, player_rb);
    }

    // why do i even have this if i made every state public level access??????????????
    // why dont i just consolidate it all into a public array??????????
    // this just takes extra time to match a string???????!?!?!?!
    // wtf past goon
    public PlayerState MatchStringToPS(string search) // make this into a hash please :blush: >>> // stfu goon dumbass wtf is this implementation >:(
    {
        switch (search)
        {
            case "MovementPS":
                StopCoroutine(DisableGuardExitWithDelay());

                AirbornePlayerState.guard_exit = false;

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
                AirbornePlayerState.guard_exit = true; // lock the player from entering wallgrab from airborne until timer is done

                guard_exit_toggle_queue.Enqueue(AirbornePlayerState);

                return WallGrabState;

            case "WallDropPS":
                StartCoroutine(DisableGuardExitWithDelay());

                return WallDropState;

            case "WallGetupPS":
                StartCoroutine(DisableGuardExitWithDelay());

                return WallGetupState;

            case "ButtonPressPS":
                return ButtonPressState;

            default:
                Debug.LogError("Did you mess up the string query? lmao stupid idiot maybe be better next time (cringe): " + search);
                return null;
        }
    }

    // handles disabling the guard_exit boolean on each state.
    // this is really only used for airborne as of 5/14, but adding
    // a generic implementation is always nice.
    // i cant just pass the state to the method because if i needed to
    // do multiple passes of this method at the same time, it would break.
    // coroutine does not multithread.
    private IEnumerator DisableGuardExitWithDelay()
    {
        yield return new WaitForSeconds(3f);

        guard_exit_toggle_queue.Dequeue().guard_exit = false;
    }
}
