using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerLoadKill : KillableObject
{
    string current_scene_name;

    // could have a delegate for OnLoad/OnEnable but idk what to put in it

    public delegate IEnumerator WhenDisabled();
    public static WhenDisabled OnDisable; // so i dont have to get a reference to this all the time

    protected override IEnumerator KillAfterEffect()
    {
        current_scene_name = SceneChangeScript.instance.GetSceneName();

        var invocation_list = OnDisable.GetInvocationList();

        foreach(WhenDisabled to_invoke in invocation_list)
        {
            StartCoroutine(to_invoke.Invoke());
        }

        yield return new WaitForSeconds(1.5f);
        
        // this does slow down the IEnumerator by average 0.02 seconds, so it is doing something.
        // actually, this isnt doing anything. it's the foreach combined with the yield statement
        // that is actually slowing it down. The Coroutines arent slowing it down at all.

        /*
        foreach(WhenDisabled to_check in invocation_list)
        {
            yield return to_check;
        }
        */


        SceneChangeScript.instance.ChangeScene(current_scene_name);
    }
}
