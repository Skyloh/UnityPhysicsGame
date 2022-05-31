using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneChangeScript : MonoBehaviour
{
    // NOTICE:
    // THE INITIAL SCENE WITH THIS GAMEOBJECT MUST NOT BE LOADED INTO AGAIN (i.e. the player shouldnt be able to die).
    // IF IT DOES, A DUPLICATE SINGLETON IS PUT INTO THE HIERARCHY AND THAT IS BAD.

    // the proper solution involves a DDOL manager, which i didnt care to implement.
    // cite: https://gamedev.stackexchange.com/questions/140014/how-can-i-get-all-dontdestroyonload-gameobjects


    TextMeshProUGUI tmp;

    string current_scene_name;
    
    public static SceneChangeScript instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        instance = this;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += GetTextElement;

        current_scene_name = SceneManager.GetActiveScene().name;
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

    // the coroutine needs to be called from THIS SCRIPT because if it is called from any other script,
    // it immediately gets destroyed, and the process of moving through the IEnumerator stops.
    public void ChangeScene(string name)
    {
        StartCoroutine(ChangeSceneProcess(name));
    }

    private IEnumerator ChangeSceneProcess(string build_name)
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
        
        tmp.SetText("Rendering...");
        yield return new WaitForSeconds(Random.Range(0.5f, 1f));

        load.allowSceneActivation = true;

        yield return new WaitUntil(() => load.isDone);

        current_scene_name = build_name;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(build_name));
    }

    public string GetSceneName()
    {
        return current_scene_name;
    }
}
