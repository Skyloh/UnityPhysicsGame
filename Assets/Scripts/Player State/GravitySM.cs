using UnityEngine;

[RequireComponent(typeof(StateLibrary))]
public class GravitySM : MonoBehaviour
{

    GravityState current_state;
    GravityState next_state;

    // Start is called before the first frame update
    void Start()
    {
        current_state = StateLibrary.library.DefaultGravityState;
    }

    public void SwapState(string dest)
    {
        next_state = StateLibrary.library.MatchStringToGS(dest);

        current_state.StateExit(next_state);

        current_state = next_state;
    }


    void FixedUpdate()
    {
        current_state.FixedUpdate();
    }

    public void LClickEvent()
    {
        current_state.LClick();
    }

    public void RClickEvent()
    {
        current_state.RClick();
    }

    public float StateVal()
    {
        return current_state.StateMultiplier();
    }

}
