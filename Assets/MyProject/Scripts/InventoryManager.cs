using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public delegate void DeliverableEvent(DeliverableModel deliverable);
    public DeliverableEvent OnEquippedDeliverable;
    public DeliverableEvent OnDeliverableDepleted;

    public static InventoryManager Instance;

    [SerializeField]
    protected DeliverableModel[] allDeliverables;

    [Header("UI")]
    [SerializeField]
    protected Image selectedIcon;
    [SerializeField]
    protected Text selectedCount;
    private DeliverableModel selectedDeliverable = null;
    private bool isDeliverableEquipped = false;

    private Dictionary<string, int> inventoryList = new Dictionary<string, int>();

    public void AddToInventory(DeliverableModel deliverable, int number, bool equipIfEmpty = true)
    {
        if (inventoryList.ContainsKey(deliverable.name))
        {
           inventoryList[deliverable.name] += number;
        }
        else
        {
            inventoryList.Add(deliverable.name, number);
        }

        if (selectedDeliverable == null)
        {
            SetSelectedDeliverable(deliverable);
        }
        else if (deliverable == selectedDeliverable)
        {
            selectedCount.text = inventoryList[deliverable.name].ToString();
        }

        if (equipIfEmpty && !isDeliverableEquipped)
        {
            EquipDeliverable(deliverable);
        }
    }

    private bool RemoveFromInventory(DeliverableModel deliverable, int number)
    {
        if (inventoryList.ContainsKey(deliverable.name) && inventoryList[deliverable.name] >= number)
        {
            inventoryList[deliverable.name] -= number;
            if (deliverable == selectedDeliverable)
            {
                selectedCount.text = inventoryList[deliverable.name].ToString();
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DeliverableThrown(DeliverableModel deliverable)
    {
        RemoveFromInventory(deliverable, 1);
        EquipNextDeliverable();
    }

    private void EquipNextDeliverable()
    {
        if (inventoryList[selectedDeliverable.name] > 0)
        {
            EquipDeliverable(selectedDeliverable);
        }
        else
        {
            isDeliverableEquipped = false;
            if (OnDeliverableDepleted != null)
            {
                OnDeliverableDepleted(selectedDeliverable);
            }
        }
    }

    private void SetSelectedDeliverable(DeliverableModel deliverable)
    {
        selectedDeliverable = deliverable;
        selectedIcon.sprite = deliverable.Icon;
        selectedCount.text = inventoryList[deliverable.name].ToString();
    }

    private void EquipDeliverable(DeliverableModel deliverable)
    {
        isDeliverableEquipped = true;
        if (OnEquippedDeliverable != null)
        {
            OnEquippedDeliverable(deliverable);
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
