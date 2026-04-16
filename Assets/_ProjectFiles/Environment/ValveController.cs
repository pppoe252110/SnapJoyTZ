using UnityEngine;
using VContainer;

public class ValveController : MonoBehaviour, IHoldInteractable
{
    [SerializeField] private Transform _wheelTransform;
    [SerializeField] private DoorController _door;
    [SerializeField] private float _maxAngle = 720f;
    [SerializeField] private Vector3 _rotationAxis = Vector3.forward;

    private InteractionConfig _config;
    private float _currentAngle;
    private bool _isHeld;
    private Quaternion _initialRotation;

    [Inject]
    public void Construct(InteractionConfig config)
    {
        _config = config;
    }

    private void Start()
    {
        if (_wheelTransform != null)
            _initialRotation = _wheelTransform.localRotation;
    }

    public void StartHold(GameObject instigator)
    {
        _isHeld = true;
    }

    public void HoldUpdate(float deltaTime)
    {
        if (!_isHeld) return;

        float delta = _config.ValveRotateSpeed * deltaTime;
        _currentAngle = Mathf.Clamp(_currentAngle + delta, 0f, _maxAngle);
        ApplyRotation();

        float progress = _currentAngle / _maxAngle;
        _door.SetOpenProgress(progress);
    }

    public void ReleaseHold()
    {
        _isHeld = false;
    }

    private void Update()
    {
        if (!_isHeld && _currentAngle > 0f)
        {
            float delta = _config.ValveReturnSpeed * Time.deltaTime;
            _currentAngle = Mathf.Max(0f, _currentAngle - delta);
            ApplyRotation();
            _door.SetOpenProgress(_currentAngle / _maxAngle);
        }
    }

    private void ApplyRotation()
    {
        if (_wheelTransform != null)
        {
            _wheelTransform.localRotation = _initialRotation * Quaternion.Euler(_rotationAxis * _currentAngle);
        }
    }

    public string GetHoverText(GameObject instigator) => "E - крутить";
    public void OnHoverEnter() { }
    public void OnHoverExit() { }
}