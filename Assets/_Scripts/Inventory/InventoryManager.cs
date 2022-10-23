using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using static SlotTypes;
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

    private void OnWeightChanged(float freeWeight, float maxWeight)
    {
        _weightText.text = $"{freeWeight} / {maxWeight} kg";
    }

    public void AddItem(InventoryItem itemToAdd)
    {
        _bagInventory.Add(GetInventorySlot(), itemToAdd);
    }

    public void RemoveItem(InventorySlot slot, InventoryItem itemToRemove)
    {
        _bagInventory.Remove(slot, itemToRemove);
    }

    private InventorySlot GetInventorySlot()
    {
        var items = _bagInventory.Items;
        for (int i = 0; i < items.Keys.Count; i++)
        {
            return items.FirstOrDefault(x => x.Value == null).Key;
        }

        return null;
    }

    [ContextMenu("Print/BagInventory")]
    private void PrintBagInventory()
    {
        for (int i = 0; i < _bagSlots.Length; i++)
        {
            if (_bagInventory.Items.TryGetValue(_bagSlots[i], out InventoryItem item))
            {
                Debug.Log($"{i}: {item.Name}");
            }
        }
    }

    [ContextMenu("Print/DollInventory")]
    private void PrintDollInventory()
    {
        for (int i = 0; i < _dollSlots.Length; i++)
        {
            if (_dollInventory.Items.TryGetValue(_dollSlots[i], out InventoryItem item))
            {
                Debug.Log($"{i}: {item.Name}");
            }
        }
    }
}
