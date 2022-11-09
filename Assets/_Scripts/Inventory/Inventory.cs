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
    private Dictionary<InventorySlot, int> _stacks = new();

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

    public Dictionary<InventorySlot, ItemSO> Items
    {
        get => _items;
        protected set
        {
            _items = value;
        }
    }
    public Dictionary<InventorySlot, int> Stacks
    {
        get => _stacks;
        protected set
        {
            _stacks = value;
        }
    }

    public Inventory(float weight, InventorySlot[] slots)
    {
        MaxWeight = weight;

        for (int i = 0; i < slots.Length; i++)
        {
            ChangeSlotValue(slots[i], null);
            ChangeStackValue(slots[i], 0);
        }
    }

    public bool Add(InventorySlot slot, ItemSO itemToAdd, bool changeWeight = true)
    {
        if (itemToAdd == null) return false;

        if (HasFreeWeight(itemToAdd) && HasFreeStack(slot, itemToAdd))
        {
            if (changeWeight == true)
                Weight += itemToAdd.Weight;

            ChangeSlotValue(slot, itemToAdd);
            ChangeStackValue(slot, Stacks[slot] + 1);

            OnItemAdded?.Invoke(itemToAdd);

            return true;
        }

        return false;
    }

    public bool HasFreeStack(InventorySlot slot, ItemSO item)
    {
        if (Stacks[slot] < item.MaxStack)
            return true;
        return false;
    }

    public bool HasFreeWeight(ItemSO item)
    {
        if (item.Weight <= FreeWeight)
            return true;
        return false;
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

    public void ChangeStackValue(InventorySlot slot, int newStack)
    {
        Stacks[slot] = newStack;
    }

    public void SwapItems(InventorySlot slot1, InventorySlot slot2)
    {
        var item1 = slot1.Peek().Item;
        var item2 = slot2.Peek().Item;

        Items[slot1] = item2;
        Items[slot2] = item1;
    }

    public bool IsFull()
    {
        foreach (var slot in Items.Keys)
        {
            if (slot.Peek() == null)
                return false;
        }
        return true;
    }

    public bool HasItem(ItemSO item)
    {
        foreach (var i in Items.Values)
        {
            if (i == item)
                return true;
        }
        return false;
    }

    public bool HasItem(ItemSO item, out InventorySlot slot)
    {
        var slots = Items.Keys.ToArray();
        slot = null;

        int i = 0;
        foreach (var item2 in Items.Values)
        {
            if(item2 == item)
            {
                slot = slots[i];
                return true;
            }
            i++;
        }
        return false;
    }
}
