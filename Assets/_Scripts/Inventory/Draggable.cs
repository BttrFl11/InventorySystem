using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IDraggable
{
    [SerializeField] private float _selectAlfa = 0.6f;

    private CanvasGroup _canvasGroup;
    private RectTransform _rTransform;
    private RectTransform _parent;

    private static Transform _tempParent;
    private static Canvas _canvas;

    public RectTransform Parent
    {
        get => _parent;
        protected set => _parent = value;
    }

    protected virtual void Awake()
    {
        _rTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();

        if (_tempParent == null)
        {
            _tempParent = FindObjectOfType<InventoryManager>().transform;
            _canvas = GetComponentInParent<Canvas>();
        }
    }

    protected virtual void OnEnable()
    {
        Parent = transform.parent.GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(_tempParent);

        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = _selectAlfa;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        MoveToParent();
    }

    public void ChangeParent(RectTransform newParent)
    {
        Parent = newParent;

        MoveToParent();
    }

    public void MoveToParent()
    {
        transform.SetParent(Parent);
        _rTransform.anchoredPosition = Parent.anchoredPosition;

        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1f;
    }
}
