using UnityEngine;

public class OverlayAnimator : MonoBehaviour
{
    // This script just exists so that animations that call functions on the Player don't have a seizure
    // when trying to call them on the overlay model as well.
    //
    // I cant make a new animator controller and avoid this problem; it stems from the animations, not the
    // fabs themselves.
    // i.e. idfc LMAO

    public void ToggleActionability(int foo)
    {
        // bar
    }
}
