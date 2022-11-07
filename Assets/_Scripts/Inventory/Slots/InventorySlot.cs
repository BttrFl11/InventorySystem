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

    public InventoryItem Item
    {
        get => _item;
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
                SwapItems(newItem);
            }
            else
            {
                newItem.ChangeParent(this);
            }

            _item = newItem;
        }
    }

    protected virtual void SwapItems(InventoryItem newItem)
    {
        var oldItem = _item;
        var oldSlot = newItem.Slot;

        _inventoryManager.SwapItems(this, oldSlot);

        newItem.ChangeParent(this);
        oldItem.ChangeParent(oldSlot);
        AttachItem(newItem);
    }

    public virtual void Clear()
    {
        if(_item != null)
        {
            Destroy(_item.gameObject);
            _item = null;
        }
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
