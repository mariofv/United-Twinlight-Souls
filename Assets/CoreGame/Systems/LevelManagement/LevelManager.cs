using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<LevelAsset> gameLevels;

    private int currentLevelIndex = -1;
    private Level currentLevel;

    public void LoadLevel(int level, bool waitLoadingScreenTime, bool fade = true)
    {
        StartCoroutine(LoadLevelAsync(level, waitLoadingScreenTime, fade));
    }

    public IEnumerator LoadLevelAsync(int level, bool waitLoadingScreenTime, bool fade)
    {
        if (fade)
        {
            CursorHider.HideCursor();
            GameManager.instance.uiManager.levelTransitionUIManager.FadeOut();
            while (GameManager.instance.uiManager.levelTransitionUIManager.IsFadingOut())
            {
                yield return new WaitForFixedUpdate();
            }
        }
        GameManager.instance.EnterGameState(GameManager.GameState.LOADING_LEVEL, changeGameStateInput: true);

        GameManager.instance.uiManager.loadingScreenUIManager.Show();

        if (waitLoadingScreenTime)
        {
            yield return new WaitForSeconds(UISettings.GameUISettings.LOADING_SCREEN_TIP_DISPLAY_TIME);
        }

        GameManager.instance.enemyManager.KillAllEnemies();

        yield return StartCoroutine(GameManager.instance.scenesManager.ChangeScene(gameLevels[level].levelScene));


        currentLevelIndex = level;
        currentLevel = GameObject.FindGameObjectWithTag(TagManager.LEVEL).GetComponent<Level>();

        GameManager.instance.audioManager.SetCurrentLevelMusic(gameLevels[level].levelMusic);
        GameManager.instance.player.GetNotControlledCharacter().Teleport(currentLevel.voidPosition.position);
        GameManager.instance.player.GetControlledCharacter().Teleport(currentLevel.startPosition.position);
        GameManager.instance.cameraManager.LoadCamera(currentLevel.GetCurrentCamera());

        GameManager.instance.EnterGameState(GameManager.GameState.COMBAT, changeGameStateInput: true);

        GameManager.instance.uiManager.loadingScreenUIManager.Hide();

        if (fade)
        {
            GameManager.instance.uiManager.levelTransitionUIManager.FadeIn();
            while (GameManager.instance.uiManager.levelTransitionUIManager.IsFadingIn())
            {
                yield return new WaitForFixedUpdate();
            }
            CursorHider.ShowCursor();
        }
    }

    public Level GetCurrentLevel()
    {
        return currentLevel;
    }

    public ZonedLevel GetCurrentLevelAsZoned()
    {
        return currentLevel as ZonedLevel;
    }

    public BossLevel GetCurrentLevelAsBoss()
    {
        return currentLevel as BossLevel;
    }

    public bool IsCurrentLevelBoss()
    {
        return currentLevelIndex == 2;
    }
}
