using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneChangeScript : MonoBehaviour
{

    TextMeshPro tmp;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += GetTextElement;
    }

    void GetTextElement(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "LoadingScene")
        {
            tmp = GameObject.FindGameObjectWithTag("LoadingUIText").GetComponent<TextMeshPro>();
        }
    }

    public void ChangeScene(string build_name)
    {
        StartCoroutine(Process(build_name));
    }

    private IEnumerator Process(string build_name)
    {
        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Single);

        AsyncOperation load = SceneManager.LoadSceneAsync(build_name, LoadSceneMode.Additive);

        load.allowSceneActivation = false;

        yield return new WaitUntil(() => tmp != null); // bc GetTextElement still needs to run

        while (!(load.progress >= 0.9f))
        {
            tmp.SetText("{0:2}%", load.progress);
            // text = (load.progress * 110f) + "%";
        }

        tmp.SetText("Finishing...");

        yield return new WaitForSeconds(1f);

        SceneManager.UnloadSceneAsync("LoadingScene");

        load.allowSceneActivation = true;

        tmp = null;
    }
}
