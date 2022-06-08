using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(Image))]
public class InputDeviceButtonImage : InputDeviceDisplayer
{
    private Image image;

    [SerializeField] private Sprite pcImage;
    [SerializeField] private Sprite psImage;
    [SerializeField] private Sprite xboxImage;

    protected override void Awake()
    {
        image = GetComponent<Image>();
        base.Awake();
    }

    public override void SetInputDeviceType(InputManager.InputDeviceType inputDeviceType)
    {
        switch (inputDeviceType)
        {
            case InputManager.InputDeviceType.KEYBOARD:
                image.sprite = pcImage;
                break;

            case InputManager.InputDeviceType.PS5_CONTROLLER:
                image.sprite = psImage;
                break;

            case InputManager.InputDeviceType.XBOX_CONTROLLER:
                image.sprite = xboxImage;
                break;
        }
    }
}
