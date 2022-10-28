using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Inventory
{
    private float _maxWeight;
    private float _weight;

    private Dictionary<InventorySlot, ItemSO> _items = new();

    /// <summary>
    /// <param name="arg1"> Weight </param>
    /// <param name="arg2"> MaxWeight </param>
    /// </summary>
    public Action<float, float> OnWeightChanged;
    public Action<ItemSO> OnItemAdded;

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
    public Dictionary<InventorySlot, ItemSO> Items
    {
        get => _items;
        protected set
        {
            _items = value;
        }
    }

    public Inventory(float weight, InventorySlot[] slots, ItemSO[] startItems)
    {
        MaxWeight = weight;

        for (int i = 0; i < slots.Length; i++)
        {
            if (startItems != null)
            {
                if (i < startItems.Length - 1)
                {
                    Debug.Log($"start item {i}: {startItems[i].name}");
                    if (startItems[i].Weight <= FreeWeight)
                    {
                        ChangeSlotItem(slots[i], startItems[i]);

                        OnItemAdded?.Invoke(startItems[i]);
                        Weight += startItems[i].Weight;
                    }
                }
                else
                {
                    ChangeSlotItem(slots[i], null);
                }

            }
            else
            {
                Debug.LogWarning("start items is null");
            }
        }
    }

    public void Add(InventorySlot slot, ItemSO itemToAdd)
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

    public void Remove(InventorySlot slot, ItemSO itemToRemove)
    {
        ChangeSlotItem(slot, null);

        Weight -= itemToRemove.Weight;
    }

    public void ChangeSlotItem(InventorySlot slot, ItemSO newItem)
    {
        Items[slot] = newItem;
    }
}
