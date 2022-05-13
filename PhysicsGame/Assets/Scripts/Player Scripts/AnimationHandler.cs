using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator AnimationController;
    private Animator ArmAnimationController;

    private delegate void OnAnimUpdate(bool including_fullState);
    private static OnAnimUpdate onAnimUpdate;

    private SPC SPCONTROL;

    // private values
    private int f_value;
    private int u_value;

    // hashes
    private int f_Hash;
    private int u_Hash;

    void OnEnable()
    {
        onAnimUpdate += UpdateParameters;
        UIOverlayManager.onToggleOverlay += getArmAnimator;
    }

    void OnDisable()
    {
        onAnimUpdate -= UpdateParameters;
        UIOverlayManager.onToggleOverlay -= getArmAnimator;
    }

    public void Start()
    {
        SPCONTROL = GetComponent<SPC>();
        f_Hash = Animator.StringToHash("FullState");
        u_Hash = Animator.StringToHash("ActionState");
    }

    private void UpdateParameters(bool fullState) // only update one or the other, you never need to update both.
    {
        if (fullState)
        {
            FullState = SPCONTROL.getPStateID();
        }
        else
        {
            UpperState = SPCONTROL.getGStateID();
        }
    }

    public static void UpdateAnimators(bool fullState)
    {
        onAnimUpdate(fullState);
    }
    
    private void getArmAnimator()
    {
        ArmAnimationController = GameObject.FindGameObjectWithTag("ArmAnimator").GetComponent<Animator>();
    }

    // getsets
    #region
    public int FullState
    {
        get { return f_value; }
        set
        {
            if (value == f_value) return;
            f_value = value;
            AnimationController.SetInteger(f_Hash, f_value);
        }
    }
    public int UpperState
    {
        get { return u_value; }
        set
        {
            if (value == u_value) return;
            u_value = value;
            ArmAnimationController.SetInteger(u_Hash, u_value);
        }
    }
    #endregion
    

}
