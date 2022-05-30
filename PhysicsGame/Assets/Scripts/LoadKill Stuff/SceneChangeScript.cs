using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneChangeScript : MonoBehaviour
{

    [SerializeField] List<string> fake_code_functions;

    TextMeshProUGUI tmp;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += GetTextElement;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= GetTextElement;
    }

    void GetTextElement(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "LoadingScene")
        {
            return;
        }

        foreach (GameObject go in scene.GetRootGameObjects())
        {
            if (go.CompareTag("LoadingUIText"))
            {
                tmp = go.GetComponentInChildren<TextMeshProUGUI>();
                return;
            }
        }
    }

    public void ChangeScene(string build_name)
    {
        StartCoroutine(Process(build_name));
    }

    private IEnumerator Process(string build_name)
    {
        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Single);

        yield return new WaitUntil(() => tmp != null); // bc GetTextElement still needs to run

        AsyncOperation load = SceneManager.LoadSceneAsync(build_name, LoadSceneMode.Single);

        load.allowSceneActivation = false;

        while (!(load.progress >= 0.9f))
        {
            tmp.SetText("{0:2}%", load.progress);

            yield return new WaitForEndOfFrame();
        }

        foreach (string s in fake_code_functions)
        {
            tmp.SetText(s);
            yield return new WaitForSeconds(Random.Range(0.5f, 1f));
        }

        load.allowSceneActivation = true;

        yield return new WaitUntil(() => load.isDone);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(build_name));
    }
}
