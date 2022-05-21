using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSM : MonoBehaviour
{
    // statemachine head for the player's state

    PlayerState current_state; // what state are we in now?
    PlayerState next_state; // what are we going to be in?
    PlayerState prior_state; // what were we before?

    void Start()
    {
        current_state = StateLibrary.library.IdlePlayerState;
        next_state = StateLibrary.library.IdlePlayerState;
        prior_state = StateLibrary.library.IdlePlayerState;
    }

    // given a string statename, find it, and swap to it.
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

    // update variable references, exit the current state, and update the animators accordingly
    private void ChangeStates()
    {
        prior_state = current_state;

        current_state.StateExit(next_state); // calls the next state's stateStart method.
        // a little ugly, but the change is simply to just move the call out into this function.
        // i havent done it yet because it doesnt matter either way.

        current_state = next_state;

        AnimationHandler.UpdateAnimators(false);
    }

    #region StateMethods

    // these are where the state overrides are called.
    // when a state swaps with an input helled, the InputSystem
    // stops calling that function. The button needs to be pressed
    // again for the function to be called again.
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

    #endregion

    // add input fields

    public int getState()
    {
        return current_state.StateID;
    }

    // polish: find a better way for this to be implemented
    // these functions are what's actually called in the Event on the animations.
    // this is because you need to have the Animator component on the same GameObject that
    // has the function to be called.
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
