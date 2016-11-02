using UnityEngine;
using System.Collections;

public class VisionConeController : MonoBehaviour
{
    [SerializeField]
    protected GameObject coneObject, eyes;

    [SerializeField]
    protected LayerMask eyeLayerMask;

    private bool spottedPlayer = false;

    [SerializeField]
    protected float spottedCooldownTime = 3.0f;
    private float cooldownCounter = 0;

    private MeshRenderer coneMesh;

    private void Start()
    {
        coneMesh = coneObject.GetComponent<MeshRenderer>();
    }

	private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            RaycastHit ray;
            Physics.Linecast(eyes.transform.position, collider.transform.position, out ray, eyeLayerMask);
            if (ray.transform.CompareTag("Player"))
            {
                spottedPlayer = true;
                cooldownCounter = spottedCooldownTime;
                AICharacterMovement ai = GetComponentInParent<AICharacterMovement>();
                if (ai != null)
                {
                    ai.SetTarget(collider.transform, AICharacterMovement.TargetType.Attack);
                }
            }

        }
    }

    private void Update()
    {
        if (spottedPlayer)
        {
            cooldownCounter -= Time.deltaTime;
            if (cooldownCounter <= 0)
            {
                spottedPlayer = false;
                GetComponentInParent<AICharacterMovement>().StopAttacking();
            }
            float c = 1 - (cooldownCounter / spottedCooldownTime);
            coneMesh.material.color = new Color(1, c, c);
        }
    }
}
