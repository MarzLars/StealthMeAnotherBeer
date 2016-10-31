using UnityEngine;
using System.Collections;

public class CharacterArms : MonoBehaviour
{
    [SerializeField]
    protected Rigidbody[] leftArm, rightArm;
    [SerializeField]
    protected Rigidbody leftHand, rightHand;
    [SerializeField]
    protected Transform deliverablePosition;

    [SerializeField]
    protected float forwardForce = 100, upwardForce = 100, right = 100;

    [SerializeField]
    protected float throwTime = 0.25f;

    private Deliverable currentDeliverable;

    private bool throwing = false;
    private float throwCount = 0;
    private Transform body;
    private Vector3 force;

    private void Start()
    {
        body = GetComponent<CharacterLegs>().chestBody.transform;
        InventoryManager.Instance.OnEquippedDeliverable += EquipDeliverable;
        InventoryManager.Instance.OnDeliverableDepleted += CurrentDeliverableDepleted;
        InventoryManager.Instance.OnUnequipCurrentDeliverable += UnequipCurrentDeliverable;
    }

    private void OnDestroy()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnEquippedDeliverable -= EquipDeliverable;
            InventoryManager.Instance.OnDeliverableDepleted -= CurrentDeliverableDepleted;
            InventoryManager.Instance.OnUnequipCurrentDeliverable -= UnequipCurrentDeliverable;
        }
    }

    private void Update()
    {
        if (!throwing && Input.GetKeyDown(KeyCode.Q) && currentDeliverable != null)
        {
            force = (upwardForce * body.up + forwardForce * body.forward + right * -body.right);
            throwing = true;
            throwCount = 0;
        }
    }

    private void FixedUpdate()
    {
        if (throwing)
        {
            if (throwCount <= throwTime)
            {
                leftHand.AddForce(force, ForceMode.Force);
                throwCount += Time.deltaTime;
            }
            else
            {
                throwing = false;
                currentDeliverable.Throw();
                InventoryManager.Instance.DeliverableThrown(currentDeliverable.Data);
            }
        }
    }

    private void CurrentDeliverableDepleted(DeliverableModel deliverableData)
    {
        currentDeliverable = null;
    }

    private void EquipDeliverable(DeliverableModel deliverableData)
    {
        Deliverable deliverable = Instantiate(deliverableData.Prefab).GetComponent<Deliverable>();
        currentDeliverable = deliverable;
        currentDeliverable.transform.position = deliverablePosition.position;
        FixedJoint joint = currentDeliverable.gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = leftHand;
    }

    private void UnequipCurrentDeliverable()
    {
        if (currentDeliverable != null)
        {
            Destroy(currentDeliverable.gameObject);
        }
        currentDeliverable = null;
    }
}
