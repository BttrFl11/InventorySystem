using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    [SerializeField] private ItemSO _itemToAdd;

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();

        _image.sprite = _itemToAdd.Icon;
    }

    public void OnButton()
    {
        var inventory = InventoryManager.Instance;
        inventory.CreateBagItem(_itemToAdd, inventory.GetEmptyBagSlot());
    }
}
