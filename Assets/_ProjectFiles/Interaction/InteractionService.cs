using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

public class InteractionService : ITickable
{
    private readonly FirstPersonCamera _camera;
    private readonly PlayerConfig _config;
    private readonly PlayerState _playerState;
    private readonly PlayerInputActions _playerInputActions;
    private readonly ItemHolder _itemHolder;

    private GameObject _currentHoverTarget;
    private IHoldInteractable _currentHold;
    private bool _isRotatingItem;

    public event Action<string> OnHoverTextChanged;

    public InteractionService(
        FirstPersonCamera camera, PlayerConfig config,
        PlayerState playerState, PlayerInputActions playerInputActions,
        ItemHolder itemHolder)
    {
        _camera = camera;
        _config = config;
        _playerState = playerState;
        _playerInputActions = playerInputActions;
        _itemHolder = itemHolder;
    }

    public void Tick()
    {
        if (_playerState.CurrentMode == PlayerMode.Inspecting)
        {
            HandleInspectionInput();
            return;
        }

        if (_playerState.CurrentMode != PlayerMode.Free)
        {
            ClearHover();
            return;
        }

        Ray ray = _camera.Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, _config.InteractionRayDistance, _config.InteractionLayers))
        {
            UpdateHover(hit.collider.gameObject);
            ProcessInput(hit.collider.gameObject);
        }
        else
        {
            ClearHover();

            if (_currentHold != null)
            {
                _currentHold.ReleaseHold();
                _currentHold = null;
            }
        }

        if (_currentHold != null && _playerInputActions.Player.Interact.IsPressed())
        {
            _currentHold.HoldUpdate(Time.deltaTime);
        }
        if (_currentHold != null && _playerInputActions.Player.Interact.WasReleasedThisFrame())
        {
            _currentHold.ReleaseHold();
            _currentHold = null;
        }
    }

    private void UpdateHover(GameObject target)
    {
        if (_currentHoverTarget == target) return;

        if (_currentHoverTarget != null && _currentHoverTarget.TryGetComponent<IHoverable>(out var prevHover))
            prevHover.OnHoverExit();

        if (_currentHold != null && _currentHoverTarget != target)
        {
            _currentHold.ReleaseHold();
            _currentHold = null;
        }

        _currentHoverTarget = target;

        if (_currentHoverTarget.TryGetComponent<IHoverable>(out var newHover))
        {
            newHover.OnHoverEnter();
            OnHoverTextChanged?.Invoke(newHover.GetHoverText(_camera.gameObject));
        }
        else
        {
            OnHoverTextChanged?.Invoke(string.Empty);
        }
    }

    private void HandleInspectionInput()
    {
        if (_playerInputActions.Player.Interact.WasPressedThisFrame())
        {
            _itemHolder.ExitInspectMode();
            return;
        }

        if (_playerInputActions.Player.Attack.WasPressedThisFrame())
        {
            Ray ray = _camera.Camera.ScreenPointToRay(_playerInputActions.UI.Point.ReadValue<Vector2>());

            if (Physics.Raycast(ray, out RaycastHit hit, _config.InteractionRayDistance))
            {
                IPickable hitItem = hit.collider.GetComponentInParent<IPickable>();
                if (hitItem != null && hitItem == _itemHolder.HeldItem && _itemHolder.IsInspecting)
                {
                    _isRotatingItem = true;
                }
            }
        }

        if (_playerInputActions.Player.Attack.IsPressed() && _isRotatingItem)
        {
            Vector2 look = _playerInputActions.Player.Look.ReadValue<Vector2>();
            if (look != Vector2.zero)
            {
                _itemHolder.RotateInspectedItem(look.x, look.y, _config.InspectRotationSpeed);
            }
        }

        if (_playerInputActions.Player.Attack.WasReleasedThisFrame())
        {
            _isRotatingItem = false;
        }
    }

    private void ProcessInput(GameObject target)
    {
        if (_playerInputActions.Player.Interact.WasPressedThisFrame())
        {
            if (target.TryGetComponent<IInteractable>(out var interactable))
            {
                interactable.Interact(_camera.gameObject);
            }
            else if (target.TryGetComponent<IHoldInteractable>(out var holdable))
            {
                _currentHold = holdable;
                holdable.StartHold(_camera.gameObject);
            }
        }
    }


    private void ClearHover()
    {
        if (_currentHoverTarget != null)
        {
            if (_currentHoverTarget.TryGetComponent<IHoverable>(out var hoverable)) hoverable.OnHoverExit();
            _currentHoverTarget = null;
            OnHoverTextChanged?.Invoke(string.Empty);
        }
    }
}