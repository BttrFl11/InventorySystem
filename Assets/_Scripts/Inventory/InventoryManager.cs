using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _weightText;

    [SerializeField] private Transform bagParent;
    [SerializeField] private Transform dollParent;

    [SerializeField] private float _maxWeight;

    private Inventory _bagInventory;
    private Inventory _dollInventory;

    private BagSlot[] _bagSlots;
    private DollSlot[] _dollSlots;

    private void OnEnable()
    {
        _bagInventory = new(_maxWeight);
        _dollInventory = new(Mathf.Infinity);

        _bagInventory.OnWeightChanged += OnWeightChanged;

        _bagSlots = bagParent.GetComponentsInChildren<BagSlot>();
        _dollSlots = dollParent.GetComponentsInChildren<DollSlot>();

        for (int i = 0; i < _bagSlots.Length; i++)
        {
            var item = _bagSlots[i].Peek();
            _bagInventory.Add(_bagSlots[i], item);
        }

        for (int j = 0; j < _dollSlots.Length; j++)
        {
            var item = _dollSlots[j].Peek();
            _dollInventory.Add(_dollSlots[j], item);
        }
    }

    private void OnDisable()
    {
        _bagInventory.OnWeightChanged -= OnWeightChanged;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            PrintBagInventoryFull();
    }

    private void OnWeightChanged(float freeWeight, float maxWeight)
    {
        _weightText.text = $"{freeWeight} / {maxWeight} kg";
    }

    public void AddItemToBag(InventorySlot slot, InventoryItem itemToAdd)
    {
        _bagInventory.Add(slot, itemToAdd);
    }

    public void RemoveItemFromBag(InventorySlot slot, InventoryItem itemToRemove)
    {
        _bagInventory.Remove(slot, itemToRemove);
    }

    public void AddItemToDoll(InventorySlot slot, InventoryItem itemToAdd)
    {
        _dollInventory.Add(slot, itemToAdd);
    }

    public void RemoveItemFromDoll(InventorySlot slot, InventoryItem itemToRemove)
    {
        _dollInventory.Remove(slot, itemToRemove);
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
