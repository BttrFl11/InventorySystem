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
                    if(_item.TryGetComponent(out EquipableItem eItem) && eItem.SlotType == _slotType)
                    {
                        SwitchItems(_item, newItem);

                    }
                    else
                    {
                        _inventoryManager.AddItemToBag(this, _item.Item);
                    }

                    _inventoryManager.RemoveItemFromBag(newItem.Slot, newItem.Item);
                    _inventoryManager.AddItemToDoll(this, newItem.Item);
                }
                else
                {
                    newItem.ChangeParent(_itemParent, this);
                }
            }

            _item = newItem;
            newItem.Equip();
        }
    }
}
