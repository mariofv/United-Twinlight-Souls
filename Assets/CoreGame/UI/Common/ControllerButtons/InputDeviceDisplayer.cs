using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputDeviceDisplayer : MonoBehaviour
{
    protected virtual void Awake()
    {
        GameManager.instance.uiManager.RegisterInputDeviceDisplayer(this);
        SetInputDeviceType(GameManager.instance.inputManager.GetInputDeviceType());
    }

    private void OnDestroy()
    {
        GameManager.instance.uiManager.UnregisterInputDeviceDisplayer(this);
    }

    public abstract void SetInputDeviceType(InputManager.InputDeviceType inputDeviceType);
}
