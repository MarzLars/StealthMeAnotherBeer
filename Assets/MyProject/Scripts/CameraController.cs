using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    protected float damping = 1.0f;

    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - target.transform.position;
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.transform.position + offset;
        Vector3 position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);
        transform.position = position;
    }
}
