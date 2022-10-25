using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Inventory
{
    private float _maxWeight;
    private float _weight;

    private Dictionary<InventorySlot, InventoryItem> _items = new();

    /// <summary>
    /// <param name="arg1"> Weight </param>
    /// <param name="arg2"> MaxWeight </param>
    /// </summary>
    public Action<float, float> OnWeightChanged;
    public Action<InventoryItem> OnItemAdded;

    public float MaxWeight
    {
        get => _maxWeight;
        protected set
        {
            _maxWeight = value;

            OnWeightChanged?.Invoke(Weight, MaxWeight);
        }
    }

    public float Weight
    {
        get => _weight;
        protected set
        {
            _weight = value;

            OnWeightChanged?.Invoke(Weight, MaxWeight);
        }
    }

    public float FreeWeight
    {
        get => MaxWeight - Weight;
    }

    /// <summary>
    /// <param name="agr1" Index of the slot </param>
    /// <param name="arg2" Item </param>
    /// </summary>
    public Dictionary<InventorySlot, InventoryItem> Items
    {
        get => _items;
        protected set
        {
            _items = value;
        }
    }

    public Inventory(float weight)
    {
        MaxWeight = weight;
    }

    public void Add(InventorySlot slot, InventoryItem itemToAdd)
    {
        if (itemToAdd == null) return;

        if (itemToAdd.Weight <= FreeWeight)
        {
            ChangeSlotItem(slot, itemToAdd);

            OnItemAdded?.Invoke(itemToAdd);
            Weight += itemToAdd.Weight;
        }
        else
        {
            Debug.LogError("No free weight!");
        }
    }

    public void Remove(InventorySlot slot, InventoryItem itemToRemove)
    {
        ChangeSlotItem(slot, null);

        Weight -= itemToRemove.Weight;
    }

    public void ChangeSlotItem(InventorySlot slot, InventoryItem newItem)
    {
        Items[slot] = newItem;
    }
}
