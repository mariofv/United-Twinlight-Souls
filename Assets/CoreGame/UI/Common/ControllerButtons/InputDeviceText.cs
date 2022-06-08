using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputDeviceText : InputDeviceDisplayer
{
    private TextMeshProUGUI text;

    [SerializeField] private string pcText;
    [SerializeField] private string psText;
    [SerializeField] private string xboxText;

    protected override void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        base.Awake();
    }

    public override void SetInputDeviceType(InputManager.InputDeviceType inputDeviceType)
    {
        switch (inputDeviceType)
        {
            case InputManager.InputDeviceType.KEYBOARD:
                text.text = pcText;
                break;

            case InputManager.InputDeviceType.PS5_CONTROLLER:
                text.text = psText;
                break;

            case InputManager.InputDeviceType.XBOX_CONTROLLER:
                text.text = xboxText;
                break;
        }
    }
}
