using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSM : MonoBehaviour
{
    PlayerState current_state;
    PlayerState next_state;
    PlayerState prior_state;

    void Start()
    {
        current_state = StateLibrary.library.IdlePlayerState;
        next_state = StateLibrary.library.IdlePlayerState;
        prior_state = StateLibrary.library.IdlePlayerState;
    }

    public void SwapState(string dest)
    {
        next_state = StateLibrary.library.MatchStringToPS(dest);

        ChangeStates();
    }

    // don't go to the next state if we were in it before this one.
    public void CarefulSwapState(string dest)
    {
        next_state = StateLibrary.library.MatchStringToPS(dest);

        if (next_state.StateID == prior_state.StateID)
        {
            return;
        }

        ChangeStates();
    }

    private void ChangeStates()
    {
        prior_state = current_state;

        current_state.StateExit(next_state);

        current_state = next_state;

        AnimationHandler.UpdateAnimators(false);
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

    // polish: find a better way for this to be implemented

    public void ToggleActionability(int int_to_value) // this is triggered by the wall-action animations because they are essentially cutscenes.
    {
        current_state.ToggleActionBool(int_to_value > 0);
    }

    public void ToggleRotation(int int_to_value) // this is triggered by the wall-action animations because they are essentially cutscenes.
    {
        current_state.ToggleRotationBool(int_to_value > 0);
    }

    public bool IsActionable()
    {
        return current_state.GetAllowAction();
    }

    public bool CanRotate()
    {
        return current_state.GetAllowRotation();
    }


}
