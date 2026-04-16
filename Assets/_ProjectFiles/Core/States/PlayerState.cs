using System;
using UnityEngine;

public enum PlayerMode
{
    Free,
    Inspecting,
    Dialogue,
    Paused
}

public class PlayerState
{
    public PlayerMode CurrentMode { get; private set; } = PlayerMode.Free;
    public event Action<PlayerMode> OnModeChanged;

    public void SetMode(PlayerMode newMode)
    {
        if (CurrentMode == newMode) return;
        CurrentMode = newMode;
        OnModeChanged?.Invoke(CurrentMode);

        switch (newMode)
        {
            case PlayerMode.Free:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
            case PlayerMode.Dialogue:
            case PlayerMode.Inspecting:
            case PlayerMode.Paused:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
        }
    }

    public bool IsInputBlocked => CurrentMode != PlayerMode.Free;
}