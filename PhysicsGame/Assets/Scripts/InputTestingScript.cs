using UnityEngine;
using UnityEngine.InputSystem;

public class InputTestingScript : MonoBehaviour
{
    public void WASD_Input(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log(context.ReadValue<Vector2>());
        }
    }

    public void Jump_Input(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log(context.ReadValueAsButton());
        }
    }

    public void Click_Input(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("clicked");
        }
    }
}
