using UnityEngine.UI;
using UnityEngine;

public class InventoryItem : Draggable
{
    [SerializeField] protected ItemSO _item;
    [SerializeField] protected Image _itemIcon;

    protected InventorySlot _slot;

    public InventorySlot Slot
    {
        get => _slot;
        protected set => _slot = value;
    }

    public ItemSO Item
    {
        get => _item;
        set
        {
            _item = value;

            Initialize();
        }
    }

    public float Weight
    {
        get => _item.Weight;
    }

    public float StackSize
    {
        get => _item.StackSize;
    }

    public string Name
    {
        get => _item.name;
    }

    protected override void OnEnable()
    {
        Slot = GetComponentInParent<InventorySlot>();

        Initialize();

        base.OnEnable();
    }

    public virtual void Initialize()
    {
        if (_item != null)
            _itemIcon.sprite = _item.Icon;
    }

    public void ChangeParent(RectTransform newParent, InventorySlot newSlot)
    {
        if (Slot != null)
            Slot.DetachItem();

        Slot = newSlot;
        Slot.AttachItem(this);

        ChangeParent(newParent);
    }
}
