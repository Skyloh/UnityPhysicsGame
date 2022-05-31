using System.Collections.Generic;

// for things that do something with their transform when activated
public class PRTransform : PuzzleReceiver
{

    List<TransformAction> transformActions;
    bool has_transforms;

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
        base.Deactivate();

        if (has_transforms)
        {
            transformActions.ForEach((TransformAction ta) => ta.ToggleDoTransform(false));
        }
    }
}
