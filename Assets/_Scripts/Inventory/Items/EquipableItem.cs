using UnityEngine;

public class EquipableItem : InventoryItem
{
    [SerializeField] private SlotTypes.Types _slotType;

    public SlotTypes.Types SlotType
    {
        get => _slotType;
    }
}
