using UnityEngine;
using VContainer;
using VContainer.Unity;

public class ItemSocket : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform _placePoint;
    [SerializeField] private bool _startWithItem = false;
    [SerializeField] private GameObject _initialItemPrefab;

    private ItemHolder _itemHolder;
    private IPickable _currentItem;
    private IObjectResolver _resolver;

    [Inject]
    public void Construct(ItemHolder itemHolder, IObjectResolver resolver)
    {
        _itemHolder = itemHolder;
        _resolver = resolver;
    }

    private void Start()
    {
        if (_startWithItem && _initialItemPrefab != null)
        {
            var itemGo = _resolver.Instantiate(_initialItemPrefab, _placePoint);
            itemGo.transform.position = _placePoint.position;
            _currentItem = itemGo.GetComponent<IPickable>();
            _currentItem.IsHeld = false;
        }
    }

    private void Update()
    {
        if (_currentItem != null && _currentItem.Transform.parent != _placePoint)
        {
            _currentItem = null;
        }
    }

    public void Interact(GameObject instigator)
    {
        if (_currentItem != null && _currentItem.Transform.parent != _placePoint)
        {
            _currentItem = null;
        }

        if (_itemHolder.HeldItem != null && _currentItem == null)
        {
            _currentItem = _itemHolder.HeldItem;
            _itemHolder.HeldItem.PutDown(_placePoint);
            _itemHolder.ClearHeldItem();
        }
        else if (_currentItem != null && _itemHolder.HeldItem == null)
        {
            _itemHolder.PickUp(_currentItem);
            _currentItem = null;
        }
    }

    public string GetHoverText(GameObject instigator)
    {
        if (_currentItem != null && _itemHolder.HeldItem == null)
            return $"E - взять {_currentItem.Transform.name}";
        else if (_itemHolder.HeldItem != null && _currentItem == null)
            return "E - положить";
        else
            return string.Empty;
    }

    public void OnHoverEnter() { }
    public void OnHoverExit() { }
}