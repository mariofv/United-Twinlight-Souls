using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDeviceGameObject : InputDeviceDisplayer
{
    [SerializeField] private GameObject pcGameObject;
    [SerializeField] private GameObject psGameObject;
    [SerializeField] private GameObject xboxGameObject;

    public override void SetInputDeviceType(InputManager.InputDeviceType inputDeviceType)
    {
        switch (inputDeviceType)
        {
            case InputManager.InputDeviceType.KEYBOARD:
                pcGameObject.SetActive(true);
                psGameObject.SetActive(false);
                xboxGameObject.SetActive(false);
                break;

            case InputManager.InputDeviceType.PS5_CONTROLLER:
                pcGameObject.SetActive(false);
                psGameObject.SetActive(true);
                xboxGameObject.SetActive(false);
                break;

            case InputManager.InputDeviceType.XBOX_CONTROLLER:
                pcGameObject.SetActive(false);
                psGameObject.SetActive(false);
                xboxGameObject.SetActive(true);
                break;
        }
    }
}