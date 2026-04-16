using UnityEngine;
using VContainer;

public class PickableItem : MonoBehaviour, IPickable
{
    public Sprite Icon => _icon;
    public string ItemName => _itemName;

    [SerializeField] private string _id = "item";
    [SerializeField] private string _itemName = "Предмет";
    [SerializeField] private string _description = "Обычный предмет.";
    [SerializeField] private bool _canBeInspected = true;
    [SerializeField] private Sprite _icon;

    private ItemHolder _itemHolder;

    public Transform Transform => transform;
    public bool IsHeld { get; set; }
    public string Id => _id;
    public string Description => _description;
    public bool CanBeInspected => _canBeInspected;

    private Rigidbody _rb;
    private Collider _collider;

    [Inject]
    public void Construct(ItemHolder itemHolder)
    {
        _itemHolder = itemHolder;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();

        _rb.isKinematic = true;
    }

    public virtual void Interact(GameObject instigator)
    {
        if (!IsHeld)
        {
            if (_canBeInspected)
                _itemHolder.PickUp(this, inspectFirst: true);
            else
                _itemHolder.PickUp(this, inspectFirst: false);
        }
        else if (_itemHolder.IsInspecting)
        {
            _itemHolder.ExitInspectMode();
        }
    }

    public virtual void OnPickedUp()
    {
        if (_rb != null)
        {
            _rb.isKinematic = true;
        }
        if (_collider != null)
        {
            _collider.enabled = false;
        }
    }

    public virtual void OnInspect()
    {
        if (_rb != null)
        {
            _rb.isKinematic = true;
        }
        if (_collider != null)
        {
            _collider.enabled = true;
        }
    }

    public virtual void OnPutDown()
    {
        if (_rb != null)
        {
            _rb.isKinematic = true;
        }
        if (_collider != null)
        {
            _collider.enabled = true;
        }
    }

    public virtual void OnDropped()
    {
        if (_rb != null)
        {
            _rb.isKinematic = false;
            _rb.useGravity = true;
        }
        if (_collider != null)
        {
            _collider.enabled = true;
        }
    }

    public void PutDown(Transform socket)
    {
        IsHeld = false;

        transform.SetParent(socket);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        if (_rb != null)
            _rb.isKinematic = true;

        if (_collider != null)
            _collider.enabled = true;
    }

    public string GetHoverText(GameObject instigator)
    {
        if (IsHeld)
            return _itemHolder.IsInspecting ? "E - взять в руку" : "E - осмотреть";
        else
            return $"E - поднять {_itemName}";
    }

    public void OnHoverEnter() { }
    public void OnHoverExit() { }
}