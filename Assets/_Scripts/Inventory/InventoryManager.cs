using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _weightText;

    [SerializeField] private GameObject _panel;
    [SerializeField] private GameObject _inventoryItemPrefab;
    [SerializeField] private GameObject _inventoryEquipableItemPrefab;
    [SerializeField] private Transform _bagParent;
    [SerializeField] private Transform _dollParent;

    private Inventory _bagInventory;
    private Inventory _dollInventory;

    private BagSlot[] _bagSlots;
    private DollSlot[] _dollSlots;

    public BagSlot[] BagSlots
    {
        get => _bagSlots;
    }

    public DollSlot[] DollSlots
    {
        get => _dollSlots;
    }

    public Inventory Bag
    {
        get => _bagInventory;
    }

    public Inventory Doll
    {
        get => _dollInventory;
    }

    public bool IsOpen
    {
        get => _panel.activeSelf;
    }

    public static InventoryManager Instance;

    private void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Scene has 2 and more InventoryManagers!");
            Destroy(gameObject);
        }
        #endregion

        _bagSlots = _bagParent.GetComponentsInChildren<BagSlot>();
        _dollSlots = _dollParent.GetComponentsInChildren<DollSlot>();

        SetPanelActive(false);
    }

    private void OnDisable()
    {
        if (_bagInventory != null)
            _bagInventory.OnWeightChanged -= UpdateUI;
    }

    private void UpdateUI(float freeWeight, float maxWeight)
    {
        _weightText.text = $"{freeWeight:0.0} / {maxWeight:0.0} kg";
    }

    public void Initialize(Inventory bag, Inventory doll)
    {
        _bagInventory = bag;
        _dollInventory = doll;

        ClearAllClots();

        InitializeSlots(BagSlots);
        InitializeSlots(DollSlots);

        _bagInventory.OnWeightChanged += UpdateUI;
        UpdateUI(_bagInventory.Weight, _bagInventory.MaxWeight);
    }

    private void InitializeSlots(InventorySlot[] slots)
    {
        bool isDoll = slots[0].TryGetComponent(out DollSlot _);
        var inventory = isDoll ? _dollInventory : _bagInventory;

        for (int i = 0; i < inventory.Items.Count; i++)
        {
            var slot = slots[i];
            var item = inventory.Items[slot];
            slot.Stack = 0;

            if (item != null)
            {
                if (isDoll)
                {
                    CreateDollItem(item);
                }
                else
                {
                    CreateBagItem(item, slot, changeWeight: false);
                }
            }
        }
    }

    private void ClearAllClots()
    {
        foreach (var slot in DollSlots)
        {
            slot.Clear();
        }

        foreach (var slot in BagSlots)
        {
            slot.Clear();
        }
    }

    private InventoryItem InstantiateItem(ItemSO newItemSO)
    {
        var prefab = newItemSO.SlotType == SlotTypes.Types.None ? _inventoryItemPrefab : _inventoryEquipableItemPrefab;
        GameObject itemGO = Instantiate(prefab, _panel.transform);
        InventoryItem item = itemGO.GetComponent<InventoryItem>();
        item.Item = newItemSO;

        return item;
    }

    public void SwapItems(InventorySlot slot1, InventorySlot slot2)
    {
        _bagInventory.SwapItems(slot1, slot2);
    }

    public void AddItemToBag(InventorySlot newSlot, InventorySlot oldSlot, ItemSO itemToAdd, bool changeWeight = true)
    {
        if (_bagInventory.IsFull() == false)
        {
            bool dollSlot = oldSlot.TryGetComponent(out DollSlot _);
            var inventory = dollSlot ? _dollInventory : _bagInventory;

            var newStack = inventory.Stacks[oldSlot];
            _bagInventory.Add(newSlot, itemToAdd, newStack, changeWeight);
            newSlot.Stack = _bagInventory.Stacks[newSlot];
        }
        else
        {
            Debug.Log("Inventory is full");
        }
    }

    private void AddCreatedItemToBag(InventorySlot newSlot, ItemSO itemToAdd, bool changeWeight = true)
    {
        if (_bagInventory.IsFull() == false)
        {
            _bagInventory.Add(newSlot, itemToAdd, changeWeight: changeWeight);
            newSlot.Stack = _bagInventory.Stacks[newSlot];
        }
        else
        {
            Debug.Log("Inventory is full");
        }
    }

    public void RemoveItemFromBag(InventorySlot slot, int stack = 1, bool changeWeight = true)
    {
        _bagInventory.Remove(slot, stack, changeWeight);
    }

    public void AddItemToDoll(InventorySlot slot, ItemSO itemToAdd)
    {
        _dollInventory.Add(slot, itemToAdd);
    }

    public void RemoveItemFromDoll(InventorySlot slot)
    {
        _dollInventory.Remove(slot);
    }

    public void SetPanelActive(bool active) => _panel.SetActive(active);

    public InventoryItem CreateDollItem(ItemSO newItemSO)
    {
        var slot = GetEmptyDollSlot(newItemSO);
        if (slot == null)
            return null;

        if (IsOpen)
        {
            var createdItem = InstantiateItem(newItemSO);
            createdItem.ChangeParent(slot);
            createdItem.GetComponent<EquipableItem>().Equip();

            return createdItem;
        }

        AddItemToDoll(slot, newItemSO);

        return null;
    }

    public InventoryItem CreateBagItem(ItemSO newItem, InventorySlot slot, bool changeWeight = true)
    {
        bool hasStack = false;
        InventoryItem createdItem = null;

        if (_bagInventory.HasItem(newItem, out List<InventorySlot> newSlots))
        {
            foreach (var newSlot in newSlots)
            {
                if (_bagInventory.HasFreeStack(newSlot, newItem))
                {
                    hasStack = true;
                    break;
                }
            }
        }

        if (IsOpen && hasStack == false)
        {
            createdItem = InstantiateItem(newItem);

            createdItem.ChangeParent(slot);
        }
        AddCreatedItemToBag(slot, newItem, changeWeight);

        return createdItem;
    }

    public InventorySlot GetEmptyDollSlot(ItemSO itemToAdd)
    {
        foreach (var key in _dollInventory.Items.Keys)
        {
            if (key.TryGetComponent(out DollSlot dollSlot))
            {
                if (dollSlot.SlotType == itemToAdd.SlotType && dollSlot.Peek() == null)
                    return dollSlot;
            }
        }

        return null;
    }

    public InventorySlot GetEmptyBagSlot()
    {
        foreach (var slot in _bagSlots)
        {
            if (slot.Peek() == null)
                return slot;
        }

        return null;
    }

    public InventorySlot GetEmptyBagSlot(ItemSO itemToAdd)
    {
        if (_bagInventory.HasItem(itemToAdd, out List<InventorySlot> slots))
        {
            foreach (var slot in slots)
            {
                if (_bagInventory.HasFreeStack(slot, itemToAdd))
                    return slot;
            }
        }

        return GetEmptyBagSlot();
    }
}
