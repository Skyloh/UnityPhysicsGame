using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CloseGame : MonoBehaviour
{

    bool is_quitting = false;

    public void Escape(InputAction.CallbackContext context)
    {
        if (context.started && !is_quitting)
        {
            is_quitting = true;
            StartCoroutine(QuitGame());
        }
    }

    private IEnumerator QuitGame()
    {
        foreach (SceneLoadTrigger.StartSceneChange effect in SceneLoadTrigger.WhenSceneChangeStarted.GetInvocationList())
        {
            yield return StartCoroutine(effect.Invoke());
        }

        yield return new WaitForSeconds(1f);

        Application.Quit();
    }
}
