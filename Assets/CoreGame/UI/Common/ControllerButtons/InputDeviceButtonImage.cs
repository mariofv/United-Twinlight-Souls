using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(Image))]
public class InputDeviceButtonImage : MonoBehaviour
{
    private Image image;

    [SerializeField] private Sprite pcImage;
    [SerializeField] private Sprite psImage;
    [SerializeField] private Sprite xboxImage;

    private void Awake()
    {
        image = GetComponent<Image>();
        GameManager.instance.uiManager.RegisterInputDeviceButtonImage(this);
        SetInputDeviceImage(GameManager.instance.inputManager.GetInputDeviceType());
    }

    private void OnDestroy()
    {
        GameManager.instance.uiManager.UnregisterInputDeviceButtonImage(this);
    }

    public void SetInputDeviceImage(InputManager.InputDeviceType inputDeviceType)
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
