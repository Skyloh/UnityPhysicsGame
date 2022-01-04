using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class AnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator AnimationController;

    private SPC SPCONTROL; // this could be better done with a delegate that actives with every state change, but this is a POLISH

    #region Hashes + GetSets
    [SerializeField] private int f_h;
    [SerializeField] private int u_h;
    [SerializeField] private int l_h;

    public int FullState
    {
        get { return f_h; }
        set
        {
            if (value == f_h) return;
            f_h = value;
            AnimationController.SetFloat("FullState", f_h);
        }
    }
    public int UpperState
    {
        get { return u_h; }
        set
        {
            if (value == u_h) return;
            u_h = value;
            AnimationController.SetFloat("UpperState", u_h);
        }
    }
    public int LowerState
    {
        get { return l_h; }
        set
        {
            if (value == l_h) return;
            l_h = value;
            AnimationController.SetFloat("LowerState", l_h);
        }
    }
    #endregion

    public void Awake()
    {
        SPCONTROL = GetComponent<SPC>();
        FullState = Animator.StringToHash("FullState");
        UpperState = Animator.StringToHash("UpperState");
        //LowerState = Animator.StringToHash("LowerState");
    }

    private void Start()
    {
    }

    private void Update()
    {
        AnimationController.SetInteger(FullState, SPCONTROL.getPStateID());
        AnimationController.SetInteger(UpperState, SPCONTROL.getGStateID());
        //AnimationController.SetInteger(LowerState, SPCONTROL.getPStateID());
    }

}
