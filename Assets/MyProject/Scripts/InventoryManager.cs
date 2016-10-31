using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class InventoryManager : MonoBehaviour
{
    public delegate void DeliverableEvent(DeliverableModel deliverable);
    public DeliverableEvent OnEquippedDeliverable;
    public DeliverableEvent OnDeliverableDepleted;

    public delegate void InventoryEvent();
    public InventoryEvent OnUnequipCurrentDeliverable;

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

    private Inventory inventoryList = new Inventory();

    public void AddToInventory(DeliverableModel deliverable, int number, bool equipIfEmpty = true)
    {
        inventoryList.AddItems(deliverable, number);

        if (selectedDeliverable == null)
        {
            SetSelectedDeliverable(deliverable);
        }
        else if (deliverable == selectedDeliverable)
        {
            selectedCount.text = inventoryList.AmountOfItem(deliverable.name).ToString();
        }

        if (equipIfEmpty && !isDeliverableEquipped)
        {
            EquipDeliverable(deliverable);
        }
    }

    private bool RemoveFromInventory(DeliverableModel deliverable, int number)
    {
        if (inventoryList.ContainsItem(deliverable.name) && inventoryList.AmountOfItem(deliverable.name) >= number)
        {
            inventoryList.RemoveItems(deliverable.name, number);
            if (deliverable == selectedDeliverable)
            {
                selectedCount.text = inventoryList.AmountOfItem(deliverable.name).ToString();
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
        if (inventoryList.AmountOfItem(selectedDeliverable.name) > 0)
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
        selectedCount.text = inventoryList.AmountOfItem(deliverable.name).ToString();
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SelectNextDeliverable();
        }
    }

    private void SelectNextDeliverable()
    {
        DeliverableModel next = inventoryList.GetNextItem(selectedDeliverable.name);
        SetSelectedDeliverable(next);
        if (isDeliverableEquipped)
        {
            if (OnUnequipCurrentDeliverable != null)
            {
                OnUnequipCurrentDeliverable();
            }
        }
        EquipDeliverable(next);
    }
}
