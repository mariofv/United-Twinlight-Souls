using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Components")]
    private PlayerInput playerInput;
    private float currentRumbleDuration = 0f;
    private float rumbleDuration = 0f;
    private bool rumbling = false;

    void Awake()
    {
        playerInput = new PlayerInput();

        playerInput.Combat.Movement.performed += ctx => OnMovementInput(ctx);
        playerInput.Combat.Movement.canceled += ctx => OnMovementInput(ctx);

        playerInput.Pause.Pause.started += ctx => GameManager.instance.OnPauseInput(ctx);

        if (Debug.isDebugBuild)
        {
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (rumbling)
        {
            currentRumbleDuration += Time.deltaTime;
            if (currentRumbleDuration >= rumbleDuration)
            {
                rumbling = false;
                
                if (Gamepad.current != null)
                {
                    Gamepad.current.SetMotorSpeeds(0f, 0f);
                }
                
            }
        }
    }

    public void Rumble(float leftMotorSpeed, float rightMotorSpeed, float duration)
    {
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(leftMotorSpeed, rightMotorSpeed);
        }
        rumbling = true;
        rumbleDuration = duration;
        currentRumbleDuration = 0f;
    }

    void OnEnable()
    {
        playerInput.Enable();
    }

    void OnDisable()
    {
        playerInput.Disable();
    }

    public void OnMovementInput(InputAction.CallbackContext context)
    {
       GameManager.instance.player.Move(context.ReadValue<Vector2>());
    }
}