using UnityEngine;
using System.Collections;

public class VisionConeController : MonoBehaviour
{
    [SerializeField]
    protected GameObject coneObject, eyes;

    [SerializeField]
    protected LayerMask eyeLayerMask;

	private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            RaycastHit ray;
            Physics.Linecast(eyes.transform.position, collider.transform.position, out ray, eyeLayerMask);
            if (ray.transform.CompareTag("Player"))
            {
                coneObject.GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }
    }
}
