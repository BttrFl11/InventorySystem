using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField] protected RectTransform _itemParent;

    protected InventoryItem _item;
    protected InventoryManager _inventoryManager;

    public RectTransform ItemParent
    {
        get => _itemParent;
    }

    protected virtual void Awake()
    {
        _item = GetComponentInChildren<InventoryItem>();
        _inventoryManager = GetComponentInParent<InventoryManager>();
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent(out InventoryItem newItem))
        {
            if (_item != null)
            {
                SwitchItems(_item, newItem);
            }
            else
            {
                newItem.ChangeParent(_itemParent, this);
            }

            _item = newItem;
        }
    }

    protected virtual void SwitchItems(InventoryItem item1, InventoryItem item2)
    {
        item1.ChangeParent(item2.Parent, item2.Slot);
        item2.ChangeParent(_itemParent, this);

        item1.Slot.AttachItem(item1);
    }

    public virtual void DetachItem()
    {
        _item = null;
    }

    public virtual void AttachItem(InventoryItem newItem)
    {
        _item = newItem;
    }

    public InventoryItem Peek() => _item;
}
