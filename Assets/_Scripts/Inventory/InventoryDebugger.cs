using UnityEngine;

public class InventoryDebugger : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private InventoryManager _inventory;
    [SerializeField] private ItemSO _item1;
    [SerializeField] private ItemSO _item2;
    [SerializeField] private ItemSO _item3;
    [SerializeField] private NPC[] _npcs;

    private int fKeyStart = 282;

    private void Start()
    {
        for (int i = 0; i < _npcs.Length; i++)
        {
            _npcs[i].Initialize(_inventory);
        }
    }

    private void Update()
    {
        TryCreateItems();
        TryPrint();

        TryInitialize();
        TryCloseOpen();
    }

    private void TryCloseOpen()
    {
        if (Input.GetKeyDown(KeyCode.C))
            _inventory.SetPanelActive(false);
        if (Input.GetKeyDown(KeyCode.V))
            _inventory.SetPanelActive(true);
    }

    private void TryInitialize()
    {
        for (int i = 0; i < _npcs.Length; i++)
        {
            KeyCode key = (KeyCode)fKeyStart + i;
            if (Input.GetKeyDown(key))
            {
                if (_inventory.Bag == _npcs[i].BagInventory)
                    return;

                Debug.Log("NPC INVENTORY " + i);
                _inventory.SetPanelActive(true);
                _inventory.Initialize(_npcs[i].BagInventory, _npcs[i].DollInventory);
            }
        }
    }

    private void TryPrint()
    {
        if (Input.GetKeyDown(KeyCode.B))
            PrintBagInventoryFull();
        if (Input.GetKeyDown(KeyCode.D))
            PrintDollInventoryFull();
        if (Input.GetKeyDown(KeyCode.I))
            PrintInventory();
    }

    private void PrintInventory()
    {
        var doll = _inventory.Doll.Items;
        var bag = _inventory.Bag.Items;
        Debug.Log("--------------DOLL-------------");
        foreach (var key in doll.Keys)
        {
            Debug.Log(doll[key]);
        }
        Debug.Log("-----------------------------");
        Debug.Log("--------------BAG-------------");
        foreach (var key in bag.Keys)
        {
            Debug.Log(bag[key]);
        }
        Debug.Log("-----------------------------");
    }

    private void TryCreateItems()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            _inventory.CreateItem(_item1, _inventory.GetFirstEmptySlot());
        if (Input.GetKeyDown(KeyCode.Alpha2))
            _inventory.CreateItem(_item2, _inventory.GetFirstEmptySlot());
        if (Input.GetKeyDown(KeyCode.Alpha3))
            _inventory.CreateItem(_item3, _inventory.GetFirstEmptySlot());
    }


    [ContextMenu("Print/BagInventoryFull")]
    public void PrintBagInventoryFull()
    {
        Debug.Log("========BAG INVENTORY========");

        int i = 0;
        foreach (var slot in _inventory.BagSlots)
        {
            if (slot.Peek() != null)
                Debug.Log($"{i}-Item: '{slot.Peek()}' ");
            else
                Debug.Log($"{i}-Item is null");

            i++;
        }

        Debug.Log("============END DEBUG=========");
    }

    [ContextMenu("Print/BagInventoryFull")]
    public void PrintDollInventoryFull()
    {
        Debug.Log("========BAG INVENTORY========");

        int i = 0;
        foreach (var slot in _inventory.DollSlots)
        {
            if (slot.Peek() != null)
                Debug.Log($"{i}-Item: '{slot.Peek()}' ");
            else
                Debug.Log($"{i}-Item is null");

            i++;
        }

        Debug.Log("============END DEBUG=========");
    }
}
