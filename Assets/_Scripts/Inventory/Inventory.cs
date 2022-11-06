using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using Unity.VisualScripting;

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

    public Inventory(float weight, InventorySlot[] slots, ItemSO[] startItems, bool isDoll)
    {
        MaxWeight = weight;

        for (int i = 0; i < slots.Length; i++)
        {
            ChangeSlotValue(slots[i], null);
        }

        for (int i = 0; i < startItems.Length; i++)
        {
            if (isDoll)
            {
                foreach (var slot in slots)
                {
                    if (slot.TryGetComponent(out DollSlot dollSlot) && dollSlot.SlotType == startItems[i].SlotType)
                    {
                        Add(slot, startItems[i]);
                        break;
                    }
                }
            }
            else
            {
                Add(slots[i], startItems[i]);
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

            ChangeSlotValue(slot, itemToAdd);

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

        ChangeSlotValue(slot, null);
    }

    public void Clear()
    {
        Weight = 0;
    }

    public void ChangeSlotValue(InventorySlot slot, ItemSO newItem)
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
