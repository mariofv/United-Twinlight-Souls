using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialAsset", menuName = "TutorialAsset", order = 1)]
[System.Serializable]
public class Tutorial : ScriptableObject
{
    [SerializeField] private string pcText;
    [SerializeField] private string psText;
    [SerializeField] private string xboxText;

    public string GetText(InputManager.InputDeviceType inputDeviceType)
    {
        switch (inputDeviceType)
        {
            case InputManager.InputDeviceType.KEYBOARD:
                return pcText;

            case InputManager.InputDeviceType.PS5_CONTROLLER:
                return psText;

            case InputManager.InputDeviceType.XBOX_CONTROLLER:
                return xboxText;

            default:
                return "";
        }
    }
}
