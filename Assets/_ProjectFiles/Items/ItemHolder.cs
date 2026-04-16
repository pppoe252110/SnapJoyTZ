using System;
using UniMediator.Runtime;
using UnityEngine;

public class ItemHolder
{
    private readonly IMediator _mediator;
    private readonly IItemHolderPivots _pivots;
    private readonly FirstPersonCamera _firstPersonCamera;
    private readonly PlayerState _playerState;

    public IPickable HeldItem { get; private set; }
    public bool IsInspecting { get; private set; }

    public ItemHolder(
        IMediator mediator,
        IItemHolderPivots pivots,
        FirstPersonCamera firstPersonCamera,
        PlayerState playerState)
    {
        _mediator = mediator;
        _pivots = pivots;
        _firstPersonCamera = firstPersonCamera;
        _playerState = playerState;
    }

    public void ExitInspectMode()
    {
        IsInspecting = false;
        _playerState.SetMode(PlayerMode.Free);

        if (HeldItem != null)
        {
            AttachToHand();
            _mediator.Publish(new ItemPickedUpNotification { Item = HeldItem });
        }
    }

    public void DropCurrentItem()
    {
        if (HeldItem == null) return;

        HeldItem.IsHeld = false;
        HeldItem.Transform.SetParent(null);

        HeldItem.OnDropped();

        _mediator.Publish(new ItemDroppedNotification { Item = HeldItem });

        HeldItem = null;

        if (IsInspecting)
        {
            ExitInspectMode();
        }
    }

    public void EnterInspectMode()
    {
        if (HeldItem == null) return;

        HeldItem.OnInspect();

        IsInspecting = true;
        _playerState.SetMode(PlayerMode.Inspecting);

        HeldItem.Transform.SetParent(_pivots.InspectPivot);
        HeldItem.Transform.localPosition = Vector3.zero;
        HeldItem.Transform.localRotation = Quaternion.identity;
    }

    private void AttachToHand()
    {
        HeldItem.OnPickedUp();

        HeldItem.Transform.SetParent(_pivots.HandPivot);
        HeldItem.Transform.localPosition = Vector3.zero;
        HeldItem.Transform.localRotation = Quaternion.identity;
    }

    public void PickUp(IPickable item, bool inspectFirst = false)
    {
        if (HeldItem != null)
            DropCurrentItem();

        HeldItem = item;
        HeldItem.IsHeld = true;

        if (inspectFirst)
        {
            EnterInspectMode();
        }
        else
        {
            AttachToHand();
            _mediator.Publish(new ItemPickedUpNotification { Item = item });
        }
    }

    public void ClearHeldItem()
    {
        if (HeldItem != null)
        {
            _mediator.Publish(new ItemDroppedNotification { Item = HeldItem });
        }
        HeldItem = null;
        IsInspecting = false;
    }
    public void RotateInspectedItem(float mouseX, float mouseY, float speed)
    {
        if (HeldItem == null || !IsInspecting) return;

        float deltaX = -mouseX * speed * Time.deltaTime;
        float deltaY = mouseY * speed * Time.deltaTime;

        Vector3 camUp = _firstPersonCamera.Camera.transform.up;
        Vector3 camRight = _firstPersonCamera.Camera.transform.right;

        Quaternion rotation = Quaternion.AngleAxis(deltaX, camUp) * Quaternion.AngleAxis(deltaY, camRight);

        HeldItem.Transform.rotation = rotation * HeldItem.Transform.rotation;
    }
}