using UnityEngine;
using UnityEngine.EventSystems;

public class DollSlot : InventorySlot
{
    [SerializeField] private SlotTypes.Types _slotType;

    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent(out EquipableItem newItem) && newItem.SlotType == _slotType)
        {
            if (_item != null)
            {
                SwitchItems(_item, newItem);
            }
            else
            {
                newItem.ChangeParent(_itemParent, this);
            }

            _item = newItem;
        }
    }
}
