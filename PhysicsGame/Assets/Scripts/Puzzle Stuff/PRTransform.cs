using System.Collections.Generic;
using UnityEngine;

// for things that do something with their transform when activated
public class PRTransform : PuzzleReceiver
{

    List<TransformAction> transformActions;
    bool has_transforms;

    // i.e. when a lift is activated once, it goes down.
    // a second activation makes it go back up.
    [SerializeField] bool return_upon_reactivation = false;

    protected virtual void Start()
    {
        transformActions = new List<TransformAction>(GetComponents<TransformAction>());

        has_transforms = transformActions.Count != 0;
    }

    public override void Activate()
    {
        PeekCameraScript.instance.Goto(transform);;

        if (has_transforms)
        {
            transformActions.ForEach((TransformAction ta) => ta.ToggleDoTransform(true));
        }
    }

    public override void Deactivate()
    {
        if (has_transforms)
        {
            transformActions.ForEach((TransformAction ta) => ta.ToggleDoTransform(return_upon_reactivation));
        }
    }
}
