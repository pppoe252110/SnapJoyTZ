using UnityEngine;

public class LightSwitchController : MonoBehaviour, IInteractable
{
    [SerializeField] private Light _targetLight;
    private bool _isOn = true;

    public void Interact(GameObject instigator)
    {
        _isOn = !_isOn;
        if (_targetLight != null) _targetLight.enabled = _isOn;
    }

    public string GetHoverText(GameObject instigator)
    {
        return _isOn ? "E - выключить свет" : "E - включить свет";
    }

    public void OnHoverEnter() { }
    public void OnHoverExit() { }
}