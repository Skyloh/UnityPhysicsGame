using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class FPSCam : MonoBehaviour
{
    [SerializeField] private float SENSITIVITY = 4f;
    //[SerializeField] private float deltatimeconstant = 1f;

    private Transform POV;

    private float xRotation = 0f;
    private float zRotation = 0f;

    [SerializeField] Volume deathVolume;

    private void OnEnable()
    {
        PlayerLoadKill.OnDisable += LerpDeathVolume;
    }
    private void OnDisable()
    {
        PlayerLoadKill.OnDisable -= LerpDeathVolume;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        POV = GameObject.FindGameObjectWithTag("Neck").transform;
    }

    private void Update()
    {
        transform.position = POV.position;
    }

    // much ado about nothing :/
    public void RotateCamera(Vector2 input)
    {
        xRotation += (input.y / SENSITIVITY); // xRotation += deltatimeconstant * Time.deltaTime * (input.y / SENSITIVITY);
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);

        zRotation = (zRotation - (input.x / (SENSITIVITY*2f))) % 360; // zRotation = (zRotation - deltatimeconstant * Time.deltaTime * (input.x / (SENSITIVITY*2f))) % 360; 

        transform.rotation = Quaternion.Euler(-xRotation, -zRotation, 0); // used to be localRotation when the camera was not child of PlayerCharacter
    }

    public void AssignCameraToStack(Camera overlay)
    {
        var this_camera = GetComponent<Camera>().GetUniversalAdditionalCameraData();

        this_camera.cameraStack.Add(overlay);
    }

    public void ClearStack()
    {
        var this_camera = GetComponent<Camera>().GetUniversalAdditionalCameraData();

        this_camera.cameraStack.Clear();
    }

    private IEnumerator LerpDeathVolume()
    {
        deathVolume.weight = 1f;

        float progress = 1f;

        while (progress > 0f)
        {
            progress = Mathf.Lerp(progress, -0.1f, 0.0125f);

            deathVolume.weight = progress;

            yield return new WaitForEndOfFrame();
        }
    }

}
