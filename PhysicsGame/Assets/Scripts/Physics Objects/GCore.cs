using UnityEngine;

public class GCore : MonoBehaviour
{
    public bool do_lerp = true;

    // this script had functionality to sort of slow the object as it approached the
    // core, but i removed it bc it didnt work too well and also was a polish and not
    // worth the time.

    public Vector3 getPos()
    {
        return transform.position;
    }


}
