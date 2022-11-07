using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
            var item = inventory.Items[slots[i]];
            if (item != null)
            {
                if (isDoll)
                {
                    CreateDollItem(item);
                }
                else
                {
                    var createdItem = CreateBagItem(item, slots[i], changeWeight: false);
                    createdItem.Stack = inventory.Stacks[slots[i]];
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

    private void InitializeCreatedItem(InventoryItem createdItem, InventorySlot slot, bool addToBag, bool changeWeight)
    {
        if (IsOpen)
            createdItem.ChangeParent(slot);

        if (addToBag == true)
            AddItemToBag(slot, createdItem.Item, changeWeight);
        else
            AddItemToDoll(slot, createdItem.Item);
    }

    public void SwapItems(InventorySlot slot1, InventorySlot slot2)
    {
        _bagInventory.SwapItems(slot1, slot2);
    }

    public void AddItemToBag(InventorySlot slot, ItemSO itemToAdd, bool changeWeight = true)
    {
        if (_bagInventory.IsFull() == false)
        {
            bool added = _bagInventory.Add(slot, itemToAdd, changeWeight);
            if (added == false)
            {
                slot = GetEmptyBagSlot();
                _bagInventory.Add(slot, itemToAdd, changeWeight);
                slot.Item.Stack = _bagInventory.Stacks[slot];
            }
        }
        else
        {
            Debug.Log("Inventory is full");
        }
    }

    public void AddItemToBag(ItemSO itemToAdd)
    {
        if (_bagInventory.HasItem(itemToAdd, out InventorySlot slot))
        {
            if(_bagInventory.HasFreeStack(slot, itemToAdd))
            {
                _bagInventory.Add(slot, itemToAdd);

                if (IsOpen)
                {
                    slot.Item.Stack = _bagInventory.Stacks[slot];
                }
            }
        }
        else
        {
            Debug.LogError("Dont have that item to add it to the stack!");
        }
    }

    public void RemoveItemFromBag(InventorySlot slot, bool changeWeight = true)
    {
        _bagInventory.Remove(slot, changeWeight);
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
            InitializeCreatedItem(createdItem, slot, addToBag: false, changeWeight: false);
            createdItem.ChangeParent(slot);
            createdItem.GetComponent<EquipableItem>().Equip();

            Debug.Log($"Item {createdItem.Name} was created!");

            return createdItem;
        }
        else
        {
            AddItemToDoll(slot, newItemSO);
        }

        return null;
    }

    public InventoryItem CreateBagItem(ItemSO newItem, InventorySlot slot, bool changeWeight = true)
    {
        if (_bagInventory.HasItem(newItem, out InventorySlot s) && _bagInventory.HasFreeStack(s, newItem))
        {
            AddItemToBag(newItem);
        }
        else
        {
            if (IsOpen)
            {
                var createdItem = InstantiateItem(newItem);
                InitializeCreatedItem(createdItem, slot, addToBag: true, changeWeight: changeWeight);
                GetEmptyDollSlot(newItem);

                Debug.Log($"Item {createdItem.Name} was created!");

                return createdItem;
            }
            else
            {
                AddItemToBag(slot, newItem, changeWeight);
            }
        }

        return null;
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
}
