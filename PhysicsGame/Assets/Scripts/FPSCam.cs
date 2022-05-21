using UnityEngine;

public class FPSCam : MonoBehaviour
{
    [SerializeField] private float SENSITIVITY = 4f;

    private Transform POV;

    private float xRotation = 0f;
    private float zRotation = 0f;

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
        xRotation += input.y / SENSITIVITY;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);

        zRotation -= input.x / (SENSITIVITY*2f); 

        transform.rotation = Quaternion.Euler(-xRotation, -zRotation, 0); // used to be localRotation when the camera was not child of PlayerCharacter
    }

}
