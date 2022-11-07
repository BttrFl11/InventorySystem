using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class InventoryItem : Draggable
{
    [SerializeField] protected ItemSO _item;
    [SerializeField] protected Image _itemIcon;
    [SerializeField] protected TextMeshProUGUI _stackCountText;
    
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

    private int _stack;
    public int Stack
    {
        get => _stack;
        set
        {
            _stack = value;
            if (_stack > MaxStack)
                _stack = MaxStack;

            _stackCountText.text = Stack.ToString();
        }
    }

    public float Weight => _item.Weight;
    public int MaxStack => _item.MaxStack;
    public string Name => _item.name;

    protected void OnEnable()
    {
        Slot = GetComponentInParent<InventorySlot>();

        Initialize();
    }

    protected void Start()
    {
        if (Parent == null)
            Parent = Slot.ItemParent;
        MoveToParent();
    }

    public virtual void Initialize()
    {
        if (_item != null)
            _itemIcon.sprite = _item.Icon;
    }

    public void ChangeParent(InventorySlot newSlot)
    {
        if (Slot != null)
            Slot.DetachItem();

        Slot = newSlot;
        Slot.AttachItem(this);

        ChangeParent(Slot.ItemParent);
    }
}
