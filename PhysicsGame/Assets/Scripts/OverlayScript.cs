using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class OverlayScript : MonoBehaviour
{
    [SerializeField] Image alphaToLerp;
    [SerializeField] RawImage peekToDisable;
    [SerializeField] List<GameObject> arms;
    Material lArmMaterial;
    Material rArmMaterial;

    float speed = 0.025f;
    int p_id;

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

    // Later Implementation adds Camera fade in and out.

    private void Awake()
    {
        p_id = Shader.PropertyToID("_PlayerProcess");

        lArmMaterial = arms[0].GetComponent<Renderer>().material;
        rArmMaterial = arms[1].GetComponent<Renderer>().material;

        arms = null;
    }

    private void OnEnable()
    {
        SceneLoadTrigger.WhenSceneChangeStarted += ProcessOut;
        SceneLoadTrigger.WhenSceneChangeStarted += FadeToBlack;

        PeekCameraScript.instance.OnDisable += DisableRawImage;
        PeekCameraScript.instance.OnEnable += EnableRawImage;
    }

    private void OnDestroy()
    {
        SceneLoadTrigger.WhenSceneChangeStarted -= ProcessOut;
        SceneLoadTrigger.WhenSceneChangeStarted -= FadeToBlack;

        PeekCameraScript.instance.OnDisable -= DisableRawImage;
        PeekCameraScript.instance.OnEnable -= EnableRawImage;
    }

    private IEnumerator Start() // called automatically when scene instance is loaded (but after awake so we dont get a nullref).
    {
        alphaToLerp.color = Color.black;
        alphaToLerp.gameObject.SetActive(true);

        while (alphaToLerp.color.a > 0.05f)
        {
            alphaToLerp.color = Color.Lerp(alphaToLerp.color, Color.clear, speed);
            
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1f);

        alphaToLerp.gameObject.SetActive(false);
    }

    private IEnumerator FadeToBlack()
    {
        alphaToLerp.color = Color.clear;
        alphaToLerp.gameObject.SetActive(true);

        while (alphaToLerp.color.a < 0.95f)
        {
            alphaToLerp.color = Color.Lerp(alphaToLerp.color, Color.black, speed);

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(0.1f);

        // object is immediately unloaded when scene changes to loadscene, so I dont need
        // to set it to inactive state.
    }

    private IEnumerator ProcessOut() // contains both bc of really weird coroutine things
    {
        float progress = 0f;
        
        while (progress < 30f)
        {
            progress = Mathf.Lerp(progress, 31f, 0.025f); // LERP NEVER GETS TO THE VALUE, ONLY CLOSE TO IT

            lArmMaterial.SetFloat(p_id, progress);
            rArmMaterial.SetFloat(p_id, progress);

            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

    }

    private void DisableRawImage()
    {
        peekToDisable.CrossFadeAlpha(0f, 1f, false);
        StartCoroutine(StupidWorkAround());
    }

    private IEnumerator StupidWorkAround() // yup, it is. why isnt crossfade a coroutine?
    {
        yield return new WaitForSeconds(1.25f);
        peekToDisable.gameObject.SetActive(false);
    }

    private void EnableRawImage()
    {
        peekToDisable.gameObject.SetActive(true);
        peekToDisable.canvasRenderer.SetAlpha(0.01f);
        peekToDisable.CrossFadeAlpha(1f, 1f, false);
    }

}
