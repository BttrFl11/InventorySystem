using UnityEngine;

public class InventoryDebugger : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private InventoryManager _inventory;
    [SerializeField] private ItemSO _item1;
    [SerializeField] private ItemSO _item2;
    [SerializeField] private ItemSO _item3;

    [Header("Start Settings")]
    [SerializeField] private ItemSO[] _startItems;

    private void Start()
    {
        for (int i = 0; i < _startItems.Length; i++)
        {
            _inventory.CreateItem(_startItems[i]);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            _inventory.CreateItem(_item1);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            _inventory.CreateItem(_item2);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            _inventory.CreateItem(_item3);
    }
}
