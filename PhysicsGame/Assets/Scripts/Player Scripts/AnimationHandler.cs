using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AnimationHandler : MonoBehaviour
{
    /*
     * A preface to why there are two Animators stored here instead of one:
     * 
     * I originally thought to retrieve the animator from the overlay arms and simply
     * overwrite the runtime animator there with the runtime animator from the player.
     * 
     * However, while this in theory should have solved my asynchronous animation problem,
     * it didn't take into account that Animator Fabs and Runtime Animators are different things.
     * 
     * Animator Fabs have properties SHARED, Runtime Animators are made at runtime unique to each
     * of their instances and therefore do not have that same quality.
     * 
     * Therefore, I need to set the weights and parameters of each runtime animator when the
     * need arises; I can't simply modify just one and expect the other to conform.
     * 
     * I don't see the advantage to runtime animators being different, but hey
     * 
     * it's a unity moment, i guess. - 5/16
     */

    [SerializeField] private Animator AnimationController;
    private Animator OverlayAnimController;

    private Scene overlay_scene; // For disabling the scene.

    public delegate void OnAnimUpdate(bool how_do_i_describe_this_bool);
    public static OnAnimUpdate UpdateAnimators;
    // should eventually contain:
    // updating variables in the animators to swap animation states
    //      including that of the overlay
    // updating camera fov / telling it to start dynamically adjusted fov (for airborne state, i.e. if FullState == 2) (delegate)
    // updating the current sound effect playback (also delegate, same one as before?)

    private SPC SPCONTROL;

    // private values
    private int f_value;
    private int u_value;

    // hashes
    private int f_Hash;
    private int u_Hash;

    void OnEnable()
    {
        UpdateAnimators += UpdateParameters;
        SceneManager.sceneLoaded += OnOverlaySceneLoaded;
    }

    void OnDisable() // no memory leaks here nosiree
    {
        UpdateAnimators -= UpdateParameters;
        SceneManager.sceneLoaded -= OnOverlaySceneLoaded;
    }

    public void Start()
    {
        // loadscene and init hashes and stuff
        SceneManager.LoadScene("OverlayScene", LoadSceneMode.Additive);

        SPCONTROL = GetComponent<SPC>();
        f_Hash = Animator.StringToHash("FullState");
        u_Hash = Animator.StringToHash("ActionState");
    }

    private void OnOverlaySceneLoaded(Scene overlay, LoadSceneMode mode)
    {
        // just a safeguard in case i load any other scene in the not so distant future
        // also i hope i dont accidentally mess this up in the future somehow lmao
        if (overlay.buildIndex == 1)
        {
            overlay_scene = overlay;

            OverlayAnimController = GameObject.FindGameObjectWithTag("ArmAnimator").GetComponent<Animator>();

            UpdateAnimators(true); // run an init update
        }
    }

    // 
    private void UpdateParameters(bool should_update_upperstate)
    {

        int cached_gstate = SPCONTROL.getGStateID();
        int cached_pstate = SPCONTROL.getPStateID();

        // if the arm is doing something, run its anim overriding the player's arm anims
        // OR
        // if arm is idle and player is idle, run idle anim with overriding weight.
        if (cached_gstate > 0 || (cached_gstate == 0 && cached_pstate == 0)) 
        {
            AnimationController.SetLayerWeight(1, 1f);
            OverlayAnimController.SetLayerWeight(1, 1f);
        }

        else // i.e. if arm is idle but player is not
        {
            AnimationController.SetLayerWeight(1, 0f);
            OverlayAnimController.SetLayerWeight(1, 0f);
        }

        // if you fire a beam, you dont need to update your walking anim (why would you)
        // if you jump, you dont need to update the current hand anim (it gets overridden anyways)
        // dunno if this really does much to performance /shrug
        if (should_update_upperstate)
        {
            UpperState = cached_gstate;
        }
        else
        {
            FullState = cached_pstate;
        }
    }

    // to toggle the overlay from and to cutscenes or transitions, currently unfinished and unused.
    private IEnumerator ToggleOverlay(bool with_anim, bool to_enable)
    {
        if (with_anim)
        {
            // CALL THE ANIM FOR ARMS BEING DISABLED/ENABLED (based on bool enabling)
            //
            // :>

            yield return new WaitForSeconds(0f); // duration of anim, approximately
        }

        GameObject[] to_disable = overlay_scene.GetRootGameObjects();

        foreach (GameObject g in to_disable){
            g.SetActive(to_enable);
        }
        
        yield break;
    }

    // Comment on Removed Function:
    /*
    // why not just make onAnimUpdate public level? is that bad?
    // if i did the above, i'd expose two variables to public level.
    // bruh this is not worth the readability issues
    public static void UpdateAnimators(bool to_pass)
    {
        onAnimUpdate(to_pass); // onAnimUpdate was the original name of the private static delegate instance
    }
    */

    #region Encapsulated Get Sets
    // SetXXXX functions are expensive, so we want to reduce their calls and string
    // comparisons as much as possible.
    public int FullState
    {
        get { return f_value; }
        set
        {
            if (value == f_value) return;
            f_value = value;
            AnimationController.SetInteger(f_Hash, f_value);
            OverlayAnimController.SetInteger(f_Hash, f_value);
        }
    }
    public int UpperState
    {
        get { return u_value; }
        set
        {
            if (value == u_value) return;
            u_value = value;
            AnimationController.SetInteger(u_Hash, u_value);
            OverlayAnimController.SetInteger(u_Hash, u_value);
        }
    }
    #endregion
    

}
