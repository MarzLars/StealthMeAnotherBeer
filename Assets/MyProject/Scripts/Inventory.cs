using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory
{
    private class InventoryItem
    {
        public string Name
        {
            get { return DeliverableData.Name;  }
        }
        public int Amount;
        public DeliverableModel DeliverableData;

        public InventoryItem(DeliverableModel data, int amount)
        {
            Amount = amount;
            DeliverableData = data;
        }
    }

    private List<InventoryItem> items = new List<InventoryItem>();

    public bool ContainsItem(string name)
    {
        return GetItem(name) != null;
    }

    public int AmountOfItem(string name)
    {
        return GetItem(name).Amount;
    }

    public void AdjustAmountOfItem(string name, int adjuster)
    {
        GetItem(name).Amount += adjuster;
    }

    public void AddItems(DeliverableModel deliverableData, int amount)
    {
        string name = deliverableData.Name;
        if (ContainsItem(name))
        {
            AdjustAmountOfItem(name, amount);
        }
        else
        {
            items.Add(new InventoryItem(deliverableData, amount));
        }
    }

    public bool RemoveItems(string name, int amount)
    {
        if (ContainsItem(name))
        {
            AdjustAmountOfItem(name, -amount);
            return true;
        }
        return false;
    }

    public DeliverableModel GetNextItem(string currentItem)
    {
        int index = IndexOfItem(currentItem);
        if (index != -1)
        {
            InventoryItem nextItem = GetItemByIndex((index + 1) % items.Count);
            return nextItem.DeliverableData;
        }
        else
        {
            return null;
        }
    }

    private InventoryItem GetItem(string name)
    {
        int length = items.Count;
        for (int i = 0; i < length; i++)
        {
            if (items[i].Name == name)
            {
                return items[i];
            }
        }
        return null;
    }

    private int IndexOfItem(string name)
    {
        int length = items.Count;
        for (int i = 0; i < length; i++)
        {
            if (items[i].Name == name)
            {
                return i;
            }
        }
        return -1;
    }

    private InventoryItem GetItemByIndex(int index)
    {
        if (index >= 0 && index < items.Count)
        {
            return items[index];
        }
        return null;
    }
}
