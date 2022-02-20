using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public enum GameState
    {
        NONE,
        MAIN_MENU,
        COMBAT,
        CINEMATIC,
        PAUSE,
        LOADING_LEVEL,
    }

    [SerializeField] private GameState gameState;
    [SerializeField] private GameState lastGameState;

    public PlayerManager player;

    public AudioManager audioManager;
    public CameraManager cameraManager;
    public DebugManager debugManager;
    public InputManager inputManager;
    public LevelManager levelManager;
    public ScenesManager scenesManager;
    public UIManager uiManager;

    // Start is called before the first frame update
    void Start()
    {
        CursorHider.ShowCursor();

        if ((Debug.isDebugBuild || Application.isEditor) && !debugManager.loadMainMenu)
        {
            InitGame();
        }
        else
        {
            InitMainMenu();
        }

    }
    public void InitMainMenu()
    {
        StartCoroutine(InitMainMenuAsync());
    }

    public IEnumerator InitMainMenuAsync()
    {
        if (IsGamePaused())
        {
            ResumeGame();
        }

        uiManager.levelTransitionUIManager.FadeOut();
        while (uiManager.levelTransitionUIManager.IsFadingOut())
        {
            yield return new WaitForFixedUpdate();
        }

        EnterGameState(GameState.MAIN_MENU);

        StartCoroutine(uiManager.UnLoadGameUI());
        StartCoroutine(uiManager.LoadMainMenuUI());

        uiManager.levelTransitionUIManager.FadeIn();
        while (uiManager.levelTransitionUIManager.IsFadingIn())
        {
            yield return new WaitForFixedUpdate();
        }
    }

    public void InitGame()
    {
        StartCoroutine(LoadGame(0));
    }

    private IEnumerator LoadGame(int level)
    {
        CursorHider.HideCursor();
        uiManager.levelTransitionUIManager.FadeOut();
        while (uiManager.levelTransitionUIManager.IsFadingOut())
        {
            yield return new WaitForFixedUpdate();
        }

        yield return StartCoroutine(uiManager.UnLoadMainMenuUI());
        yield return StartCoroutine(uiManager.LoadGameUI());

        uiManager.gameUIManager.HideAll();

        yield return StartCoroutine(levelManager.LoadLevel(level, fade:false));

        uiManager.levelTransitionUIManager.FadeIn();
        while (uiManager.levelTransitionUIManager.IsFadingIn())
        {
            yield return new WaitForFixedUpdate();
        }
        CursorHider.ShowCursor();
    }
    public void CloseGame()
    {
        Application.Quit();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        //audioManager.ResumeAllAudio();
        EnterGameState(lastGameState);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        //audioManager.PauseAllAudio();
        EnterGameState(GameState.PAUSE);
    }

    public bool IsGamePaused()
    {
        return gameState == GameState.PAUSE;
    }

    private bool IsGameRunning()
    {
        return gameState != GameState.MAIN_MENU;
    }

    public void EnterGameState(GameState state, bool changeGameStateInput = true)
    {
        bool unloadInstant = true;
        uiManager.UnloadGameStateUI(gameState, instant: unloadInstant);

        lastGameState = gameState;
        gameState = state;

        if (changeGameStateInput)
        {
            inputManager.DisableGameStateInput(lastGameState);
            inputManager.EnableGameStateInput(gameState);
        }

        uiManager.LoadGameStateUI(gameState);
    }

    public GameState GetCurrentGameState()
    {
        return gameState;
    }

    public void OnPauseInput(InputAction.CallbackContext context)
    {
        if (IsGamePaused())
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

}
