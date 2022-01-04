using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


// FPS camera script that also controls charge vignette effect
public class FPSCam : MonoBehaviour
{
    [SerializeField] private float SENSITIVITY = 4f;

    private Transform POV;
    private GCore core;

    [SerializeField] private PostProcessVolume volume;
    private Vignette charge_vignette;
    private float initial_intensity;

    private float xRotation = 0f;
    private float zRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        POV = GameObject.FindGameObjectWithTag("Neck").transform;

        core = GameObject.FindGameObjectWithTag("Gravity Core").GetComponent<GCore>();

        volume.profile.TryGetSettings(out charge_vignette);

        initial_intensity = charge_vignette.intensity.value;
    }

    private void Update()
    {
        transform.position = POV.position;
    }

    // much ado about nothing :/
    public void RotateCamera(Vector2 input)
    {
        xRotation += input.y / SENSITIVITY;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);

        core.do_lerp = xRotation > -30f;

        zRotation -= input.x / (SENSITIVITY*2f); 

        transform.localRotation = Quaternion.Euler(-xRotation, -zRotation, 0);
    }

    public void UpdateVignette(float value)
    {
        if (!isNotYetMaxed())
        {
            return;
        }

        charge_vignette.intensity.value = value * (1f - initial_intensity) + 0.3f;
    }

    public void ResetVignette()
    {
        charge_vignette.intensity.value = initial_intensity;
    }

    bool isNotYetMaxed()
    {
        return !(charge_vignette.intensity.value > 0.95f);
    }

    public float getXRotation()
    {
        return transform.eulerAngles.x;
    }
}
