using UnityEngine;

public class EquipableItem : InventoryItem
{
    [SerializeField] private SlotTypes.Types _slotType;

    private bool _isEquiped = false;

    public bool IsEquiped
    {
        get => _isEquiped;
        protected set => _isEquiped = value;
    }

    public SlotTypes.Types SlotType
    {
        get => _slotType;
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
