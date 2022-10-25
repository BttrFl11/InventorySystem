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
        Initialize();

        base.OnEnable();
    }

    protected virtual void Initialize()
    {
        Slot = GetComponentInParent<InventorySlot>();

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
