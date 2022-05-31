using UnityEngine;

// for solids that do things when activated
public class SolidPRTransform : PRTransform
{
    [SerializeField] GravitySolid to_enable; // leave this null if the gameobject is just to move
    [SerializeField] GameObject Hand_Sigil_OBJ; // same with this

    Material Hand_Sigil;

    void OnEnable()
    {
        Hand_Sigil = Hand_Sigil_OBJ.GetComponent<Renderer>().material;

        to_enable.enabled = false;
    }

    void OnDisable()
    {
        Hand_Sigil.SetFloat("_IsActive", 0f);
    }

    public override void Activate()
    {
        to_enable.enabled = true;

        Hand_Sigil.SetFloat("_IsActive", 1f);

        PeekCameraScript.instance.Goto(transform);;
    }

    public override void Deactivate()
    {
        to_enable.enabled = false;

        Hand_Sigil.SetFloat("_IsActive", 0f);

        base.Deactivate();
    }
}
