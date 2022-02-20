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
        playerInput.Combat.Jump.started += ctx => OnJumpInput(ctx);

        playerInput.Pause.Pause.started += ctx => GameManager.instance.OnPauseInput(ctx);

        playerInput.LoadingScreen.PreviousTip.started += ctx => OnPreviousTipInput(ctx);
        playerInput.LoadingScreen.NextTip.started += ctx => OnNextTipInput(ctx);

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
        DisableAllInputs();
    }

    void OnDisable()
    {
        playerInput.Disable();
    }

    public void DisableGameStateInput(GameManager.GameState gameState)
    {
        switch (gameState)
        {
            case GameManager.GameState.MAIN_MENU:
                break;
            case GameManager.GameState.COMBAT:
                playerInput.Combat.Disable();
                break;
            case GameManager.GameState.CINEMATIC:
                //playerInput.Cinematic.Disable();
                break;
            case GameManager.GameState.PAUSE:
                playerInput.Pause.Disable();
                break;
            case GameManager.GameState.NONE:
                break;
            case GameManager.GameState.LOADING_LEVEL:
                playerInput.LoadingScreen.Disable();
                break;
        }
    }

    public void EnableGameStateInput(GameManager.GameState gameState)
    {
        switch (gameState)
        {
            case GameManager.GameState.MAIN_MENU:
                break;
            case GameManager.GameState.COMBAT:
                playerInput.Combat.Enable();
                break;
            case GameManager.GameState.CINEMATIC:
                //playerInput.Cinematic.Enable();
                break;
            case GameManager.GameState.PAUSE:
                playerInput.Pause.Enable();
                break;
            case GameManager.GameState.NONE:
                break;
            case GameManager.GameState.LOADING_LEVEL:
                playerInput.LoadingScreen.Enable();
                break;
        }
    }

    private void DisableAllInputs()
    {
        playerInput.Combat.Disable();
        playerInput.Pause.Disable();
        playerInput.LoadingScreen.Disable();
    }

    public void OnMovementInput(InputAction.CallbackContext context)
    {
       GameManager.instance.player.Move(context.ReadValue<Vector2>());
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        GameManager.instance.player.Jump();
    }

    public void OnPreviousTipInput(InputAction.CallbackContext context)
    {
        GameManager.instance.uiManager.loadingScreenUIManager.ShowPreviousTip();
    }

    public void OnNextTipInput(InputAction.CallbackContext context)
    {
        GameManager.instance.uiManager.loadingScreenUIManager.ShowNextTip();
    }
}