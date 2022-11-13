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
                {
                    var slot = _inventoryManager.GetEmptyBagSlot();
                    _item.ChangeParent(slot);

                    _inventoryManager.AddItemToBag(this, newItem.Slot, newItem.Item);
                    _inventoryManager.RemoveItemFromDoll(newItem.Slot);

                    newItem.ChangeParent(this);

                    eItem.Unequip();
                }
                else
                {
                    SwapItems(newItem);
                }
            }
            else
            {
                if (newItem.Slot.TryGetComponent(out BagSlot _))
                {
                    var stack = newItem.Slot.Stack;
                    _inventoryManager.AddItemToBag(this, newItem.Slot, newItem.Item);
                    _inventoryManager.RemoveItemFromBag(newItem.Slot, stack);
                }
                else if (newItem.TryGetComponent(out EquipableItem eItem) && eItem.IsEquiped == true)
                {
                    _inventoryManager.AddItemToBag(this, newItem.Slot , newItem.Item);
                    _inventoryManager.RemoveItemFromDoll(newItem.Slot);
                    eItem.Unequip();
                }

                newItem.ChangeParent(this);
            }

            _item = newItem;
        }
    }
}
