using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIOverlayManager : MonoBehaviour
{
    private Scene OverlayScene;

    public delegate void OnToggleOverlay();
    public static OnToggleOverlay onToggleOverlay;

    private void Awake()
    {
        SceneManager.LoadScene("OverlayScene", LoadSceneMode.Additive);
    }

    private void Start()
    {
        StartCoroutine(WaitBriefly());
    }

    private IEnumerator WaitBriefly()
    {
        yield return new WaitForSeconds(1f);

        onToggleOverlay();

        yield break;
    }

    // method that enables scene, calls the respective animation for it, and enables Gravity SM

    // method that disables the scene, calls the respective animation for that, and disables Gravity SM
    // make it halt the script until the animation is complete (or just after a bit of time)
}
