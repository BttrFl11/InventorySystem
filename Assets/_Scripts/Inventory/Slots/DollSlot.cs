using UnityEngine;
using UnityEngine.EventSystems;

public class DollSlot : InventorySlot
{
    [SerializeField] private SlotTypes.Types _slotType;

    public SlotTypes.Types SlotType
    {
        get => _slotType;
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent(out EquipableItem newItem)
            && newItem.SlotType == _slotType)
        {
            if (newItem != _item)
            {
                if (_item != null)
                {
                    SwapItems(newItem);
                }
                else if (newItem.Slot.TryGetComponent(out DollSlot _))
                {
                    _inventoryManager.RemoveItemFromDoll(newItem.Slot);
                    _inventoryManager.AddItemToDoll(this, newItem.Item);
                    newItem.ChangeParent(this);
                }
                else
                {
                    _inventoryManager.RemoveItemFromBag(newItem.Slot);
                    _inventoryManager.AddItemToDoll(this, newItem.Item);
                    newItem.ChangeParent(this);
                }
            }

            _item = newItem;
            newItem.Equip();
        }
    }

    protected override void SwapItems(InventoryItem newItem)
    {
        _item.GetComponent<EquipableItem>().Unequip();
        var newItemSlot = newItem.Slot;
        var oldItem = _item;

        _inventoryManager.RemoveItemFromDoll(this);
        _inventoryManager.RemoveItemFromBag(newItem.Slot);
        _inventoryManager.AddItemToBag(newItem.Slot, _item.Item);
        _inventoryManager.AddItemToDoll(this, newItem.Item);

        oldItem.ChangeParent(newItemSlot);
        newItem.ChangeParent(this);
        newItemSlot.AttachItem(oldItem);
    }
}
