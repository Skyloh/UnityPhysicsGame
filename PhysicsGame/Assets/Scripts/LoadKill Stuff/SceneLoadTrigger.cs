using System.Collections;
using UnityEngine;

public class SceneLoadTrigger : MonoBehaviour
{

    // One Per Scene.

    bool entered = false;

    [SerializeField] string loadSceneName;

    public delegate IEnumerator StartSceneChange(); // things bouta get funky
    public static StartSceneChange WhenSceneChangeStarted;

    private void OnTriggerEnter(Collider other)
    {
        if (entered == false)
        {
            entered = true;
            StartCoroutine(StartLoadScene());
            return;
        }
    }

    private IEnumerator StartLoadScene()
    {   
        foreach (StartSceneChange effect in WhenSceneChangeStarted.GetInvocationList())
        {
            yield return StartCoroutine(effect.Invoke());
        }

        yield return new WaitForSeconds(1f);

        // i hate having to use so many tags.

        SceneChangeScript master = GameObject.FindGameObjectWithTag("SceneHub").GetComponent<SceneChangeScript>();

        master.ChangeScene(loadSceneName);
    }
}
