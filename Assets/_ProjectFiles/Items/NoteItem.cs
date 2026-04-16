using UnityEngine;

public class NoteItem : PickableItem
{
    [SerializeField] private Transform _bookCover;
    [SerializeField] private Vector3 _openRotation = new Vector3(0, 0, 180);
    [SerializeField] private float _animationSpeed = 5f;

    private bool _isReading;
    private Quaternion _closedRotation;

    private void Start()
    {
        if (_bookCover != null)
            _closedRotation = _bookCover.localRotation;
    }

    private void Update()
    {
        if (_bookCover == null) return;

        Quaternion targetRot = _isReading ? Quaternion.Euler(_openRotation) : _closedRotation;
        _bookCover.localRotation = Quaternion.Slerp(_bookCover.localRotation, targetRot, Time.deltaTime * _animationSpeed);
    }

    public override void Interact(GameObject instigator)
    {
        base.Interact(instigator);

        UpdateReadingState();
    }

    public override void OnPickedUp()
    {
        base.OnPickedUp();
        _isReading = false;
    }

    public override void OnPutDown()
    {
        base.OnPutDown();
        _isReading = false;
    }

    public override void OnDropped()
    {
        base.OnDropped();
        _isReading = false;
    }

    private void UpdateReadingState()
    {
        _isReading = IsHeld && IsBeingInspected();
    }

    private bool IsBeingInspected()
    {
        return transform.parent != null &&
               transform.parent.name.Contains("Inspect");
    }
}