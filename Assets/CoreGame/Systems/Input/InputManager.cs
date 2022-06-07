using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using UnityEngine.InputSystem.UI;

public class InputManager : MonoBehaviour
{
    public enum InputDeviceType
    {
        KEYBOARD,
        PS5_CONTROLLER,
        XBOX_CONTROLLER
    }

    private InputDeviceType currentInputDeviceType;

    [Header("Components")]
    private PlayerInput playerInput;
    [SerializeField] private InputSystemUIInputModule uiInputModule;

    private float currentRumbleDuration = 0f;
    private float rumbleDuration = 0f;
    private bool rumbling = false;


    void Awake()
    {
        playerInput = new PlayerInput();

        playerInput.DeviceCheck.AnyKey.performed += ctx => CheckInputDevice(ctx);

        playerInput.MainMenu.AnyKey.performed += ctx => GameManager.instance.uiManager.mainMenuUIManager.OnAnyKeyPressed(ctx);
        playerInput.MainMenu.RightNavigation.performed += ctx => GameManager.instance.uiManager.mainMenuUIManager.OnRightPressed(ctx);
        playerInput.MainMenu.LeftNavigation.performed += ctx => GameManager.instance.uiManager.mainMenuUIManager.OnLeftPressed(ctx);
        playerInput.MainMenu.Confirm.performed += ctx => GameManager.instance.uiManager.mainMenuUIManager.OnConfirmPressed(ctx);
        playerInput.MainMenu.Cancel.performed += ctx => GameManager.instance.uiManager.mainMenuUIManager.OnCancelPressed(ctx);

        playerInput.Combat.Movement.performed += ctx => OnMovementInput(ctx);
        playerInput.Combat.Movement.canceled += ctx => OnMovementInput(ctx);
        playerInput.Combat.Jump.started += ctx => OnJumpInput(ctx);
        playerInput.Combat.LightAttack.started += ctx => OnLightAttackInput(ctx);
        playerInput.Combat.Dash.started += ctx => OnDashInput(ctx);
        playerInput.Combat.Shield.started += ctx => OnShieldInputStart(ctx);
        playerInput.Combat.Shield.canceled += ctx => OnShieldInputEnd(ctx);
        playerInput.Combat.LockEnemy.started += ctx => OnLockEnemyInput(ctx);
        playerInput.Combat.ChangeLockedEnemy.started += ctx => OnChangeLockedEnemyInput(ctx);
        playerInput.Combat.Interact.started += ctx => OnInteractInput(ctx)
        ;
        playerInput.Dialogue.Interact.started += ctx => OnInteractInput(ctx);

        playerInput.Cinematic.AnyKey.started += ctx => OnCinematicAnyKeyInput(ctx);
        playerInput.Cinematic.SkipCinematic.started += ctx => OnSkipCinematicStartedInput(ctx);
        playerInput.Cinematic.SkipCinematic.performed += ctx => OnSkipCinematicPerformedInput(ctx);
        playerInput.Cinematic.SkipCinematic.canceled += ctx => OnSkipCinematicCanceledInput(ctx);

        playerInput.Tutorial.AnyKey.started += ctx => OnTutorialAnyKeyInput(ctx);

        playerInput.Pause.Pause.started += ctx => GameManager.instance.OnPauseInput(ctx);

        playerInput.LoadingScreen.PreviousTip.started += ctx => OnPreviousTipInput(ctx);
        playerInput.LoadingScreen.NextTip.started += ctx => OnNextTipInput(ctx);

        if (Debug.isDebugBuild)
        {
            playerInput.Debug.OpenScenesDebugMenu.performed += ctx => GameManager.instance.debugManager.OnOpenScenesDebugMenu(ctx);
            playerInput.Debug.OpenCharacterDebugMenu.performed += ctx => GameManager.instance.debugManager.OnOpenCharacterDebugMenu(ctx);
            playerInput.Debug.OpenEnemyDebugMenu.performed += ctx => GameManager.instance.debugManager.OnOpenEnemyDebugMenu(ctx);
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
                playerInput.MainMenu.Disable();
                RestoreSubmitAction();
                break;
            case GameManager.GameState.COMBAT:
                playerInput.Combat.Disable();
                break;
            case GameManager.GameState.DIALOGUE:
                playerInput.Dialogue.Disable();
                break;
            case GameManager.GameState.CINEMATIC:
                playerInput.Cinematic.Disable();
                break;
            case GameManager.GameState.TUTORIAL:
                playerInput.Tutorial.Disable();
                break;
            case GameManager.GameState.PAUSE:
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
                playerInput.MainMenu.Enable();
                SetSubmitActionToAnyKey();
                break;
            case GameManager.GameState.COMBAT:
                playerInput.Combat.Enable();
                break;
            case GameManager.GameState.DIALOGUE:
                playerInput.Dialogue.Enable();
                break;
            case GameManager.GameState.CINEMATIC:
                playerInput.Cinematic.Enable();
                break;
            case GameManager.GameState.TUTORIAL:
                playerInput.Tutorial.Enable();
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
        playerInput.Dialogue.Disable();
        playerInput.Cinematic.Disable();
        playerInput.Pause.Disable();
        playerInput.LoadingScreen.Disable();
        playerInput.MainMenu.Disable();
    }

    public void EnablePauseInput(bool enable)
    {
        if (enable)
        {
            playerInput.Pause.Enable();
        }
        else
        {
            playerInput.Pause.Disable();
        }
    }

    public void RestoreSubmitAction()
    {
        playerInput.UI.Enable();
        uiInputModule.submit = InputActionReference.Create(playerInput.UI.Submit);
    }

    public void SetSubmitActionToAnyKey()
    {
        playerInput.UI.Disable();
        uiInputModule.submit = InputActionReference.Create(playerInput.MainMenu.AnyKey);
    }

    public InputDeviceType GetInputDeviceType()
    {
        return currentInputDeviceType;
    }

    public void CheckInputDevice(InputAction.CallbackContext context)
    {
        InputDeviceType newInputDeviceType = InputDeviceType.KEYBOARD;
        if (context.control.device is Keyboard || context.control.device is Mouse)
        {
            newInputDeviceType = InputDeviceType.KEYBOARD;
        }
        else if (context.control.device is DualShockGamepad)
        {
            newInputDeviceType = InputDeviceType.PS5_CONTROLLER;
            Debug.Log(context.control.device);
        }
        else if (context.control.device is XInputController)
        {
            newInputDeviceType = InputDeviceType.XBOX_CONTROLLER;
            Debug.Log(context.control.device);
        }

        if (currentInputDeviceType == newInputDeviceType)
        {
            return;
        }

        currentInputDeviceType = newInputDeviceType;
        GameManager.instance.uiManager.ChangeInputDeviceType(currentInputDeviceType);
        if (GameManager.instance.tutorialManager.GetCurrentTutorial() != null)
        {
            GameManager.instance.tutorialManager.GetCurrentTutorial().OnInputDeviceChanged(currentInputDeviceType);
        }
    }

    public void OnMovementInput(InputAction.CallbackContext context)
    {
        GameManager.instance.player.GetControlledCharacter().characterInputManager.movementVector = context.ReadValue<Vector2>();
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (!GameManager.instance.player.GetControlledCharacter().characterInputManager.IsInputAcceptedInCurrentState(CharacterInputManager.CharacterInputAction.JUMP))
        {
            return;
        }
        GameManager.instance.player.GetControlledCharacter().Jump();
    }

    public void OnLightAttackInput(InputAction.CallbackContext context)
    {
        if (!GameManager.instance.player.GetControlledCharacter().characterInputManager.IsInputAcceptedInCurrentState(CharacterInputManager.CharacterInputAction.ATTACK))
        {
            return;
        }
        GameManager.instance.player.GetControlledCharacter().LightAttack();
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (!GameManager.instance.player.GetControlledCharacter().characterInputManager.IsInputAcceptedInCurrentState(CharacterInputManager.CharacterInputAction.DASH))
        {
            return;
        }
        GameManager.instance.player.GetControlledCharacter().StartDash();
    }

    public void OnShieldInputStart(InputAction.CallbackContext context)
    {
        if (!GameManager.instance.player.GetControlledCharacter().characterInputManager.IsInputAcceptedInCurrentState(CharacterInputManager.CharacterInputAction.SHIELD))
        {
            return;
        }
        GameManager.instance.player.GetControlledCharacter().RaiseShield();
    }

    public void OnShieldInputEnd(InputAction.CallbackContext context)
    {
        GameManager.instance.player.GetControlledCharacter().ReleaseShield();
    }

    public void OnLockEnemyInput(InputAction.CallbackContext context)
    {
        if (!GameManager.instance.player.GetControlledCharacter().characterInputManager.IsInputAcceptedInCurrentState(CharacterInputManager.CharacterInputAction.LOCK_ENEMY))
        {
            return;
        }
        GameManager.instance.player.GetControlledCharacter().characterLockManager.SwitchLockEnemy();
    }

    public void OnChangeLockedEnemyInput(InputAction.CallbackContext context)
    {
        if (!GameManager.instance.player.GetControlledCharacter().characterInputManager.IsInputAcceptedInCurrentState(CharacterInputManager.CharacterInputAction.LOCK_ENEMY))
        {
            return;
        }
        GameManager.instance.player.GetControlledCharacter().characterLockManager.ChangeLockedEnemy();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (!GameManager.instance.player.GetControlledCharacter().characterInputManager.IsInputAcceptedInCurrentState(CharacterInputManager.CharacterInputAction.INTERACT))
        {
            return;
        }
        GameManager.instance.player.GetControlledCharacter().Interact();
    }

    public void OnCinematicAnyKeyInput(InputAction.CallbackContext context)
    {
        GameManager.instance.uiManager.cinematicUIManager.ShowSkipPrompt();
    }

    public void OnSkipCinematicStartedInput(InputAction.CallbackContext context)
    {
        GameManager.instance.uiManager.cinematicUIManager.StartSkipCinematic();
    }

    public void OnSkipCinematicPerformedInput(InputAction.CallbackContext context)
    {
        GameManager.instance.cinematicManager.SkipCinematic();
    }

    public void OnSkipCinematicCanceledInput(InputAction.CallbackContext context)
    {
        GameManager.instance.uiManager.cinematicUIManager.EndSkipCinematic();
    }

    public void OnTutorialAnyKeyInput(InputAction.CallbackContext context)
    {
        if (GameManager.instance.tutorialManager.GetCurrentTutorial() != null)
        {
            GameManager.instance.tutorialManager.GetCurrentTutorial().AnyKeyPressed();
        }
    }

    public void OnPreviousTipInput(InputAction.CallbackContext context)
    {
        GameManager.instance.uiManager.loadingScreenUIManager.ShowPreviousTip();
    }

    public void OnNextTipInput(InputAction.CallbackContext context)
    {
        GameManager.instance.uiManager.loadingScreenUIManager.ShowNextTip();
    }

    public void OnNavigationUI(InputAction.CallbackContext context)
    {
        Debug.Log("ASODlajsdS");
    }
}