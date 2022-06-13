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
        DIALOGUE,
        TUTORIAL,
        CINEMATIC,
        PAUSE,
        LOADING_LEVEL,
    }

    [SerializeField] private GameState gameState;
    [SerializeField] private GameState lastGameState;

    public PlayerManager player;

    public AudioManager audioManager;
    public CameraManager cameraManager;
    public CinematicManager cinematicManager;
    public DebugManager debugManager;
    public DialogueManager dialogueManager;
    public EnemyManager enemyManager;
    public InputManager inputManager;
    public LevelManager levelManager;
    public LootManager lootManager;
    public ProgressionManager progressionManager;
    public ScenesManager scenesManager;
    public SettingsManager settingsManager;
    public TutorialManager tutorialManager;
    public UIManager uiManager;

    // Start is called before the first frame update
    void Start()
    {
        CursorHider.ShowCursor();

        if ((Debug.isDebugBuild || Application.isEditor) && !debugManager.loadMainMenu)
        {
            InitGame(debugManager.selectedLevel, false);
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

        player.DeselectCurrentCharacter();
        EnterGameState(GameState.MAIN_MENU);

        StartCoroutine(uiManager.UnLoadGameUI());
        yield return StartCoroutine(uiManager.LoadMainMenuUI());

        audioManager.PlayMainMenuMusic();
        inputManager.EnablePauseInput(false);

        enemyManager.KillAllEnemies(spawnLoot: false);
        enemyManager.EmptyEnemyPools();

        lootManager.EmptyPickupPools();

        uiManager.levelTransitionUIManager.FadeIn();
        while (uiManager.levelTransitionUIManager.IsFadingIn())
        {
            yield return new WaitForFixedUpdate();
        }
    }

    public void InitGame(int level, bool waitLoadingScreenTime = true)
    {
        StartCoroutine(LoadGame(level, waitLoadingScreenTime));
    }

    private IEnumerator LoadGame(int level, bool waitLoadingScreenTime)
    {
        CursorHider.HideCursor();
        uiManager.levelTransitionUIManager.FadeOut();
        while (uiManager.levelTransitionUIManager.IsFadingOut())
        {
            yield return new WaitForFixedUpdate();
        }

        if (!player.HasCharacterSelected())
        {
            player.ControlIlona();
        }

        yield return StartCoroutine(uiManager.UnLoadMainMenuUI());
        yield return StartCoroutine(uiManager.LoadGameUI());

        uiManager.gameUIManager.HideAll();

        enemyManager.InitializedEnemyPools();
        lootManager.InitializePickupPools();
        yield return StartCoroutine(levelManager.LoadLevelAsync(level, waitLoadingScreenTime: waitLoadingScreenTime, fade:false));

        inputManager.EnablePauseInput(true);

        uiManager.levelTransitionUIManager.FadeIn();
        while (uiManager.levelTransitionUIManager.IsFadingIn())
        {
            yield return new WaitForFixedUpdate();
        }
        CursorHider.ShowCursor();
        player.GetControlledCharacter().EnableMovement();
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

    public void EnterGameState(GameState state, bool changeGameStateInput = true)
    {
        bool unloadInstant = true;

        uiManager.UnloadGameStateUI(gameState, instant: unloadInstant);
        ExitGameState(gameState);

        lastGameState = gameState;
        gameState = state;

        if (changeGameStateInput)
        {
            inputManager.DisableGameStateInput(lastGameState);
            inputManager.EnableGameStateInput(gameState);
        }

        uiManager.LoadGameStateUI(gameState);
    }

    private void ExitGameState(GameState state)
    {
        switch (state)
        {
            case GameState.NONE:
                break;
            case GameState.MAIN_MENU:
                break;
            case GameState.COMBAT:
                break;
            case GameState.DIALOGUE:
                break;
            case GameState.TUTORIAL:
                break;
            case GameState.CINEMATIC:
                GameManager.instance.cinematicManager.EndCurrentCinematic();
                break;
            case GameState.PAUSE:
                break;
            case GameState.LOADING_LEVEL:
                break;
        }
    }

    public GameState GetCurrentGameState()
    {
        return gameState;
    }

    public void OnPauseInput(InputAction.CallbackContext context)
    {
        if (uiManager.gameUIManager.pauseUI.IsSystemsMenuOpen())
        {
            return;
        }

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
