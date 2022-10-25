using UnityEngine;

public class EquipableItem : InventoryItem
{
    private bool _isEquiped = false;

    public bool IsEquiped
    {
        get => _isEquiped;
        protected set => _isEquiped = value;
    }

    public SlotTypes.Types SlotType
    {
        get => _item.SlotType;
    }

    public void Equip()
    {
        IsEquiped = true;
    }

    public void Unequip()
    {
        IsEquiped = false;
    }
}
