using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator AnimationController;

    private SPC SPCONTROL; // this could be better done with a delegate that actives with every state change, but this is a POLISH

    #region Hashes + GetSets
    // private values
    private int f_value;
    private int u_value;

    // hashes
    private int f_Hash;
    private int u_Hash;

    // getsets
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
            AnimationController.SetInteger(u_Hash, u_value);
        }
    }
    #endregion

    public void Awake()
    {
        SPCONTROL = GetComponent<SPC>();
        f_Hash = Animator.StringToHash("FullState");
        u_Hash = Animator.StringToHash("UpperState");
    }

    private void Start()
    {
    }

    private void Update()
    {
        FullState = SPCONTROL.getPStateID();
        UpperState = SPCONTROL.getGStateID();
    }

}
