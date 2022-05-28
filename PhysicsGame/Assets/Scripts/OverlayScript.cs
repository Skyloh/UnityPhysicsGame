using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class OverlayScript : MonoBehaviour
{
    [SerializeField] Image alphaToLerp;
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
    }

    private void OnDestroy()
    {
        SceneLoadTrigger.WhenSceneChangeStarted -= ProcessOut;
        SceneLoadTrigger.WhenSceneChangeStarted -= FadeToBlack;
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

        yield return new WaitForSeconds(1f);

        alphaToLerp.gameObject.SetActive(false);
    }

    private IEnumerator ProcessOut()
    {
        float progress = 0f;
        
        while (progress < 30f)
        {
            progress = Mathf.Lerp(progress, 30f, 0.125f);

            lArmMaterial.SetFloat(p_id, progress);
            rArmMaterial.SetFloat(p_id, progress);

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1f);

        alphaToLerp.gameObject.SetActive(false);
    }

}
