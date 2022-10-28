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

                _inventory.SetPanelActive(true);
                _inventory.Initialize(_npcs[i].BagInventory, _npcs[i].DollInventory);
            }
        }
    }

    private void TryPrint()
    {
        if (Input.GetKeyDown(KeyCode.T))
            _inventory.PrintBagInventoryFull();
    }

    private void TryCreateItems()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            _inventory.CreateItem(_item1);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            _inventory.CreateItem(_item2);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            _inventory.CreateItem(_item3);
    }
}
