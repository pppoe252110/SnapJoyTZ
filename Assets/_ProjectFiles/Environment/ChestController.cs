using UnityEngine;
using VContainer;

public class ChestController : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform _lid;
    [SerializeField] private Vector3 _openRotation = new Vector3(-90, 0, 0);
    [SerializeField] private float _openSpeed = 2f;
    [SerializeField] private string _openedByItemId = "key";

    private ItemHolder _itemHolder;
    private bool _isOpen;
    private Quaternion _closedRotation;
    private Quaternion _targetRotation;

    [Inject]
    public void Construct(ItemHolder itemHolder)
    {
        _itemHolder = itemHolder;
    }

    private void Start()
    {
        _closedRotation = _lid.localRotation;
        _targetRotation = _closedRotation;
    }

    public void Interact(GameObject instigator)
    {
        if (_isOpen) return;

        if (_itemHolder.HeldItem != null && _itemHolder.HeldItem.Id == _openedByItemId)
        {
            Open();
            Destroy(_itemHolder.HeldItem.Transform.gameObject);
            _itemHolder.ClearHeldItem();
        }
        else
        {
            Debug.Log("Нужен ключ");
        }
    }

    private void Open()
    {
        _isOpen = true;
        _targetRotation = Quaternion.Euler(_openRotation);
    }

    private void Update()
    {
        _lid.localRotation = Quaternion.Slerp(_lid.localRotation, _targetRotation, _openSpeed * Time.deltaTime);
    }

    public string GetHoverText(GameObject instigator)
    {
        if (_isOpen) return string.Empty;
        return "E - открыть";
    }

    public void OnHoverEnter() { }
    public void OnHoverExit() { }
}