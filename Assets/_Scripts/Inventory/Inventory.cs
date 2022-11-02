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
            try
            {
                if (i < startItems.Length)
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
            catch
            {
                Debug.LogWarning("start items is null");
            }
        }
    }

    public void Add(InventorySlot slot, ItemSO itemToAdd, bool changeWeight = true)
    {
        if (itemToAdd == null) return;

        if (itemToAdd.Weight <= FreeWeight)
        {
            if (changeWeight == true)
                Weight += itemToAdd.Weight;

            ChangeSlotItem(slot, itemToAdd);

            OnItemAdded?.Invoke(itemToAdd);
        }
        else
        {
            Debug.LogError("No free weight!");
        }
    }

    public void Remove(InventorySlot slot, bool changeWeight = true)
    {
        var itemToRemove = slot.Peek();

        if (changeWeight == true)
            Weight -= itemToRemove.Weight;

        ChangeSlotItem(slot, null);
    }

    public void Clear()
    {
        Weight = 0;
    }

    public void ChangeSlotItem(InventorySlot slot, ItemSO newItem)
    {
        Items[slot] = newItem;
    }

    public void SwapItems(InventorySlot slot1, InventorySlot slot2)
    {
        var item1 = slot1.Peek().Item;
        var item2 = slot2.Peek().Item;

        Items[slot1] = item2;
        Items[slot2] = item1;
    }
}
