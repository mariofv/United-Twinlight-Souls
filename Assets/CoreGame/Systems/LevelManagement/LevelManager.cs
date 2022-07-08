using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<LevelAsset> gameLevels;

    private int currentLevelIndex = -1;
    private Level currentLevel;
    private Checkpoint lastCheckPoint;

    public void LoadLevel(int level, bool waitLoadingScreenTime, bool fade = true)
    {
        StartCoroutine(LoadLevelAsync(level, waitLoadingScreenTime, fade));
    }

    public IEnumerator LoadLevelAsync(int level, bool waitLoadingScreenTime, bool fade)
    {
        if (level != 2)
        {
            GameManager.instance.uiManager.gameUIManager.bossHealthBarUI.Hide(instant: true);
        }

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

        GameManager.instance.enemyManager.KillAllEnemies(spawnLoot: false);

        yield return StartCoroutine(GameManager.instance.scenesManager.ChangeScene(gameLevels[level].levelScene));


        currentLevelIndex = level;
        currentLevel = GameObject.FindGameObjectWithTag(TagManager.LEVEL).GetComponent<Level>();
        lastCheckPoint = null;

        GameManager.instance.audioManager.SetCurrentLevelMusic(gameLevels[level].levelMusic);

        if (currentLevel.introLevelCinematic == null)
        {
            GameManager.instance.player.GetControlledCharacter().Teleport(currentLevel.startPosition.position);
        }

        GameManager.instance.player.GetNotControlledCharacter().Teleport(currentLevel.voidPosition.position);
        GameManager.instance.player.GetControlledCharacter().characterStatsManager.SetFullHealth();
        GameManager.instance.cameraManager.LoadCamera(currentLevel.GetCurrentCamera());

        GameManager.instance.EnterGameState(GameManager.GameState.COMBAT, changeGameStateInput: true);
        GameManager.instance.progressionManager.SetMaxLevelUnlocked(level);

        GameManager.instance.saveManager.SaveGame();

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

        if (currentLevel.introLevelCinematic != null)
        {
            GameManager.instance.cinematicManager.PlayCinematic(currentLevel.introLevelCinematic);
            currentLevel.introLevelCinematic.onCinematicEnd.AddListener(OnIntroCinematicEnd);
        }
    }

    private void OnIntroCinematicEnd()
    {
        currentLevel.introLevelCinematic.onCinematicEnd.RemoveListener(OnIntroCinematicEnd);
        GameManager.instance.player.GetControlledCharacter().Teleport(currentLevel.startPosition.position);
    }

    public void Respawn()
    {
        StartCoroutine(RespawnAsync());
    }

    public IEnumerator RespawnAsync()
    {
        CursorHider.HideCursor();
        GameManager.instance.uiManager.levelTransitionUIManager.FadeOut();
        while (GameManager.instance.uiManager.levelTransitionUIManager.IsFadingOut())
        {
            yield return new WaitForFixedUpdate();
        }

        GameManager.instance.EnterGameState(GameManager.GameState.LOADING_LEVEL, changeGameStateInput: true); 

        if (!IsCurrentLevelBoss())
        {
            ZonedLevel zonedLevel = GetCurrentLevelAsZoned();
            if (zonedLevel.IsPlayerInCombatArea())
            {
                zonedLevel.ResetCombatArea();
            }
        }
        GameManager.instance.enemyManager.KillAllEnemies(spawnLoot: false);

        if (IsCurrentLevelBoss())
        {
            BossLevel bossLevel = GetCurrentLevelAsBoss();
            bossLevel.SpawnBoss();
        }

        GameManager.instance.audioManager.SetCurrentLevelMusic(gameLevels[currentLevelIndex].levelMusic);
        GameManager.instance.player.GetNotControlledCharacter().Teleport(currentLevel.voidPosition.position);

        Vector3 spawnPosition;
        if (lastCheckPoint != null)
        {
            spawnPosition = lastCheckPoint.transform.position;
        }
        else
        {
            spawnPosition = currentLevel.startPosition.position;
        }
        GameManager.instance.player.GetControlledCharacter().Teleport(spawnPosition);
        GameManager.instance.cameraManager.LoadCamera(currentLevel.GetCurrentCamera());
        yield return new WaitForSeconds(UISettings.GameUISettings.RESPAWN_TIME);

        if (GameManager.instance.player.GetControlledCharacter().IsDead())
        {
            GameManager.instance.player.GetControlledCharacter().Revive();
        }
        
        GameManager.instance.EnterGameState(GameManager.GameState.COMBAT, changeGameStateInput: true);


        GameManager.instance.uiManager.levelTransitionUIManager.FadeIn();
        while (GameManager.instance.uiManager.levelTransitionUIManager.IsFadingIn())
        {
            yield return new WaitForFixedUpdate();
        }
        CursorHider.ShowCursor();
    }

    public void SetLastCheckpoint(Checkpoint checkpoint)
    {
        lastCheckPoint = checkpoint;
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
