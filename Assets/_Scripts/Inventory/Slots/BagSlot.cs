using UnityEngine.EventSystems;
using UnityEngine;

public class BagSlot : InventorySlot
{
    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent(out InventoryItem newItem))
        {
            if (_item != null)
            {
                if (newItem.TryGetComponent(out EquipableItem eItem) && eItem.IsEquiped == true)
                    FromDollToFullBagSlot(newItem, eItem);
                else
                    SwapItems(newItem);
            }
            else
            {
                if (newItem.Slot.TryGetComponent(out BagSlot _))
                    FromBagToNullBagSlot(newItem);
                else if (newItem.TryGetComponent(out EquipableItem eItem) && eItem.IsEquiped == true)
                    FromDollToNullBagSlot(newItem, eItem);

                newItem.ChangeParent(this);
            }

            _item = newItem;
        }
    }

    protected override void SwapItems(InventoryItem newItem)
    {
        var newSlot = newItem.Slot;
        var oldItem = _item;
        var oldSlot = _item.Slot;

        //_inventoryManager.RemoveItemFromBag(newSlot, oldSlot.Stack, false);
        //_inventoryManager.RemoveItemFromBag(oldSlot, newSlot.Stack, false);
        //_inventoryManager.AddItemToBag(newSlot, oldSlot, newItem.Item, false);
        //_inventoryManager.AddItemToBag(oldSlot, newSlot, oldItem.Item, false);

        _inventoryManager.SwapItems(oldSlot, newSlot);
        newSlot.Stack = _inventoryManager.Bag.Stacks[newSlot];
        oldSlot.Stack = _inventoryManager.Bag.Stacks[oldSlot];

        newItem.ChangeParent(oldSlot);
        oldItem.ChangeParent(newSlot);
        AttachItem(newItem);

        Debug.Log("SwapItems");
    }

    private void FromBagToNullBagSlot(InventoryItem newItem)
    {
        var stack = newItem.Slot.Stack;
        _inventoryManager.AddItemToBag(this, newItem.Slot, newItem.Item);
        _inventoryManager.RemoveItemFromBag(newItem.Slot, stack);
    }

    private void FromDollToNullBagSlot(InventoryItem newItem, EquipableItem eItem)
    {
        _inventoryManager.AddItemToBag(this, newItem.Slot, newItem.Item);
        _inventoryManager.RemoveItemFromDoll(newItem.Slot);
        eItem.Unequip();
    }

    private void FromDollToFullBagSlot(InventoryItem newItem, EquipableItem eItem)
    {
        var slot = _inventoryManager.GetEmptyBagSlot();
        _item.ChangeParent(slot);

        _inventoryManager.AddItemToBag(this, newItem.Slot, newItem.Item);
        _inventoryManager.RemoveItemFromDoll(newItem.Slot);

        newItem.ChangeParent(this);

        eItem.Unequip();
    }
}
