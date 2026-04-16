using UnityEngine;

public interface IPickable : IInteractable
{
    string Id { get; }
    string ItemName { get; }
    string Description { get; }
    Transform Transform { get; }
    bool IsHeld { get; set; }
    bool CanBeInspected { get; }
    Sprite Icon { get; }

    void OnPickedUp();
    void OnPutDown();
    void PutDown(Transform socket);
    void OnInspect();

    void OnDropped();
}