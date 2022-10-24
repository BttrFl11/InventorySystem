using UnityEngine.EventSystems;

public class BagSlot : InventorySlot
{
    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent(out InventoryItem newItem))
        {
            if (_item != null)
            {
                if (newItem.TryGetComponent(out EquipableItem _) && newItem.Slot.TryGetComponent(out DollSlot _))
                {
                    _inventoryManager.AddItemToBag(this, newItem);
                    _inventoryManager.RemoveItemFromDoll(this, newItem);

                    newItem.ChangeParent(_itemParent, this);
                }
                else
                {
                    SwitchItems(_item, newItem);
                }
            }
            else
            {
                newItem.ChangeParent(_itemParent, this);

                if(newItem.TryGetComponent(out EquipableItem eItem) && eItem.IsEquiped == true)
                {
                    _inventoryManager.AddItemToBag(this, newItem);
                }
            }

            _item = newItem;
        }
    }
}
