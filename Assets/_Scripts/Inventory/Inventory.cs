using System.Collections.Generic;
using UnityEngine;
using System;

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

    public Dictionary<InventorySlot, InventoryItem> Items
    {
        get => _items;
        protected set => _items = value;
    }

    public Inventory(float weight)
    {
        MaxWeight = weight;
    }

    public void Add(InventorySlot slot, InventoryItem item)
    {
        if (item.Weight <= FreeWeight)
        {
            Weight += item.Weight;

            Items.Remove(slot);
            Items.Add(slot, item);

            OnItemAdded?.Invoke(item);
        }
        else
        {
            Debug.LogError("No free weight!");
        }
    }

    public void Remove(InventorySlot slot, InventoryItem itemToRemove)
    {
        Weight -= itemToRemove.Weight;

        Items.Remove(slot);
    }
}
