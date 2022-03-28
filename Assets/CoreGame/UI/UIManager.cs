using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public LevelTransitionUIManager levelTransitionUIManager;
    public LoadingScreenUIManager loadingScreenUIManager;

    [Header("Game UI")]
    public string gameUIScene;
    public GameUIManager gameUIManager;

    [Header("MainMenu UI")]
    public string mainMenuScene;
    public MainMenuUIManager mainMenuUIManager;

    public IEnumerator LoadMainMenuUI()
    {
        yield return StartCoroutine(GameManager.instance.scenesManager.ChangeScene(mainMenuScene));
        mainMenuUIManager = GameObject.FindGameObjectWithTag(TagManager.MAIN_MENU_UI).GetComponent<MainMenuUIManager>();
    }

    public IEnumerator UnLoadMainMenuUI()
    {
        yield return StartCoroutine(GameManager.instance.scenesManager.UnLoadScene(mainMenuScene));
        mainMenuUIManager = null;
    }

    public IEnumerator LoadGameUI()
    {
        yield return StartCoroutine(GameManager.instance.scenesManager.LoadGameUIScene(gameUIScene));
        gameUIManager = GameObject.FindGameObjectWithTag(TagManager.GAME_UI).GetComponent<GameUIManager>();
    }

    public IEnumerator UnLoadGameUI()
    {
        yield return StartCoroutine(GameManager.instance.scenesManager.UnLoadGameUIScene());
        gameUIManager = null;
    }

    public void LoadGameStateUI(GameManager.GameState gameState, bool instant = false)
    {
        switch (gameState)
        {
            case GameManager.GameState.NONE:
                break;

            case GameManager.GameState.MAIN_MENU:
                break;

            case GameManager.GameState.COMBAT:
                gameUIManager.ShowCombatUI(instant);
                break;

            case GameManager.GameState.CINEMATIC:
                break;

            case GameManager.GameState.PAUSE:
                gameUIManager.ShowPauseUI(instant);
                break;

            case GameManager.GameState.LOADING_LEVEL:
                break;
        }
    }

    public void UnloadGameStateUI(GameManager.GameState gameState, bool instant = false)
    {
        switch (gameState)
        {
            case GameManager.GameState.MAIN_MENU:
                break;

            case GameManager.GameState.COMBAT:
                gameUIManager.HideCombatUI(instant);
                break;

            case GameManager.GameState.CINEMATIC:
                break;

            case GameManager.GameState.PAUSE:
                gameUIManager.HidePauseUI(instant);
                break;
        }
    }
}
