using System.Collections;
using UnityEngine;

public class PeekCameraScript : MonoBehaviour
{
    [SerializeField] Vector3 default_euler;

    public delegate void WhenDisabled();
    public WhenDisabled OnDisable;

    public delegate void WhenEnabled();
    public WhenEnabled OnEnable;

    public static PeekCameraScript instance;

    private void Awake()
    {
        instance = this;

        gameObject.SetActive(false);
    }

    private void Start()
    {
        transform.eulerAngles = default_euler;
    }

    public void Goto(Transform calling_object)
    {
        gameObject.SetActive(true);

        OnEnable();

        transform.position = calling_object.position + calling_object.up * 5f + calling_object.forward * -5f;

        StartCoroutine(DismissWithTime());
    }

    private IEnumerator DismissWithTime()
    {
        yield return new WaitForSeconds(2.25f);

        OnDisable();

        gameObject.SetActive(false);
    }
    
}
