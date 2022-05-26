using System.Collections.Generic;
using UnityEngine;

// for gravity solids that do something with their transform when activated
public class PRTransform : PuzzleReceiver
{
    [SerializeField] GravitySolid to_enable; // leave this null if the gameobject is just to move

    [SerializeField] Material Hand_Sigil; // same with this

    List<TransformAction> transformActions;
    bool has_transforms;

    void Start()
    {

        if (to_enable != null)
        {
            GravitySolidExclusive(0f);
        }

        transformActions = new List<TransformAction>(GetComponents<TransformAction>());

        has_transforms = transformActions.Count != 0;
        
    }

    private void OnDisable()
    {
        if (to_enable != null)
        {
            Hand_Sigil.SetFloat("_IsActive", 0f);
        }
    }

    public override void Activate()
    {
        base.Activate();

        if (to_enable != null)
        {
            GravitySolidExclusive(1f);
        }

        if (has_transforms)
        {
            transformActions.ForEach((TransformAction ta) => ta.ToggleDoTransform(true));
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();

        if (to_enable != null)
        {
            GravitySolidExclusive(0f);
        }

        if (has_transforms)
        {
            transformActions.ForEach((TransformAction ta) => ta.ToggleDoTransform(false));
        }
    }

    private void GravitySolidExclusive(float input_float)
    {
        to_enable.enabled = input_float == 1f;

        Hand_Sigil.SetFloat("_IsActive", input_float);
    }
}
