using UnityEngine;

public interface IHoverable
{
    string GetHoverText(GameObject instigator);
    void OnHoverEnter();
    void OnHoverExit();
}

public interface IInteractable : IHoverable
{
    void Interact(GameObject instigator);
}

public interface IHoldInteractable : IHoverable
{
    void StartHold(GameObject instigator);
    void HoldUpdate(float deltaTime);
    void ReleaseHold();
}