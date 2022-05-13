using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSM : MonoBehaviour
{

    PlayerState current_state;
    PlayerState next_state;

    void Start()
    {
        current_state = StateLibrary.library.IdlePlayerState;
    }

    public void SwapState(string dest)
    {
        next_state = StateLibrary.library.MatchStringToPS(dest);

        current_state.StateExit(next_state);

        current_state = next_state;

        AnimationHandler.UpdateAnimators(true);
    }

    public void WASD(InputAction.CallbackContext context)
    {
        current_state.WASD(context);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        current_state.Jump(context);
    }

    public void Shift(InputAction.CallbackContext context)
    {
        current_state.Shift(context);
    }

    void FixedUpdate()
    {
        current_state.InFixedUpdate();
    }

    // add input fields

    public int getState()
    {
        return current_state.StateID;
    }

    public void ToggleActionability(int int_to_value) // this is triggered by the wall-action animations because they are essentially cutscenes.
    {
        current_state.ToggleActionBool(int_to_value > 0);
    }

}
