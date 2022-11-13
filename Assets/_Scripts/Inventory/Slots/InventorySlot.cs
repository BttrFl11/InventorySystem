using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField] protected RectTransform _itemParent;
    [SerializeField] protected TextMeshProUGUI _stackCountText;

    private int _stack;
    public int Stack
    {
        get => _stack;
        set
        {
            _stack = value;

            _stackCountText.enabled = true;
            _stackCountText.text = Stack.ToString();

            if (_stack == 0 || _stack == 1)
                _stackCountText.enabled = false;
        }
    }

    protected InventoryItem _item;
    protected InventoryManager _inventoryManager;

    public int MaxStack => _item.Item.MaxStack;
    public RectTransform ItemParent => _itemParent;
    public InventoryItem Item => _item;

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
        if (_item != null)
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
