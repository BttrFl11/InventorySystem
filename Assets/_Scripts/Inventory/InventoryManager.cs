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

        _bagSlots = GetComponentsInChildren<BagSlot>();
        _dollSlots = GetComponentsInChildren<DollSlot>();
    }

    private void OnDisable()
    {
        _bagInventory.OnWeightChanged -= UpdateUI;
    }

    private void UpdateUI(float freeWeight, float maxWeight)
    {
        _weightText.text = $"{freeWeight} / {maxWeight} kg";
    }

    public void Initialize(Inventory bag, Inventory doll)
    {
        _bagInventory = bag;
        _dollInventory = doll;

        _bagInventory.OnWeightChanged += UpdateUI;

        InitializeSlots(_bagSlots, _bagInventory);
        InitializeSlots(_dollSlots, _dollInventory);

        UpdateUI(_bagInventory.Weight, _bagInventory.MaxWeight);
    }

    private void InitializeSlots(InventorySlot[] slots, Inventory inventory)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Clear();

            var item = inventory.Items[slots[i]];
            Debug.Log(item.name);

            if (item != null)
                CreateItem(item);
        }
    }

    public void AddItemToBag(InventorySlot slot, ItemSO itemToAdd)
    {
        _bagInventory.Add(slot, itemToAdd);
    }

    public void RemoveItemFromBag(InventorySlot slot, ItemSO itemToRemove)
    {
        _bagInventory.Remove(slot, itemToRemove);
    }

    public void AddItemToDoll(InventorySlot slot, ItemSO itemToAdd)
    {
        _dollInventory.Add(slot, itemToAdd);
    }

    public void RemoveItemFromDoll(InventorySlot slot, ItemSO itemToRemove)
    {
        _dollInventory.Remove(slot, itemToRemove);
    }

    public void SetPanelActive(bool active) => _panel.SetActive(active);

    /// <summary>
    /// Creates a new Item and adds it to the bag
    /// </summary>
    public void CreateItem(ItemSO newItemSO, bool addToBag = true)
    {
        InventorySlot slot = GetFirstEmptySlot();
        if (addToBag == false)
        {
            //foreach (var key in _dollInventory.Items.Keys)
            //{
            //    if (key.TryGetComponent(out DollSlot dollSlot))
            //    {
            //        if (dollSlot.SlotType == newItemSO.SlotType && dollSlot.Peek() == null)
            //            slot = dollSlot;
            //    }
            //}
        }

        var prefab = newItemSO.SlotType == SlotTypes.Types.None ? _inventoryItemPrefab : _inventoryEquipableItemPrefab;
        GameObject itemGO = Instantiate(prefab, _panel.transform);
        InventoryItem item = itemGO.GetComponent<InventoryItem>();

        item.Item = newItemSO;
        item.ChangeParent(slot.ItemParent, slot);
        item.Initialize();

        if (addToBag == true)
            AddItemToBag(slot, newItemSO);
        else
            AddItemToDoll(slot, newItemSO);

        Debug.Log($"Item {item.Name} was created!");
    }

    public InventorySlot GetFirstEmptySlot()
    {
        foreach (var slot in _bagSlots)
        {
            if (slot.Peek() == null)
                return slot;
        }

        return null;
    }

    #region Debug

    [ContextMenu("Print/BagInventoryFull")]
    public void PrintBagInventoryFull()
    {
        Debug.Log("========BAG INVENTORY========");

        int i = 0;
        foreach (var slot in _bagSlots)
        {
            if (slot.Peek() != null)
                Debug.Log($"{i}-Item: '{slot.Peek()}' ");
            else
                Debug.Log($"{i}-Item is null");

            i++;
        }

        Debug.Log("============END DEBUG=========");
    }

    #endregion
}
