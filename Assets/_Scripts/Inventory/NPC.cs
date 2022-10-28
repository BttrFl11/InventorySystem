using UnityEngine;

[System.Serializable]
public class NPC
{
    [SerializeField] private float _inventoryWeight;
    [SerializeField] private ItemSO[] _startItems;

    private Inventory _bagInventory;
    private Inventory _dollInventory;

    public Inventory BagInventory
    {
        get => _bagInventory;
        protected set
        {
            _bagInventory = value;
        }
    }

    public Inventory DollInventory
    {
        get => _dollInventory;
        protected set
        {
            _dollInventory = value;
        }
    }

    public void Initialize(InventoryManager manager)
    {
        BagInventory = new(_inventoryWeight, manager.BagSlots, _startItems);
        DollInventory = new(Mathf.Infinity, manager.DollSlots, null);
    }
}
