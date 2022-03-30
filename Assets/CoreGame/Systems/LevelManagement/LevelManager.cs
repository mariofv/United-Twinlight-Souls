using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<LevelAsset> gameLevels;

    private int currentLevelIndex = -1;
    private Level currentLevel;

    public IEnumerator LoadLevel(int level, bool waitLoadingScreenTime, bool fade = true)
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
        yield return StartCoroutine(GameManager.instance.scenesManager.ChangeScene(gameLevels[level].levelScene));


        currentLevelIndex = level;
        currentLevel = GameObject.FindGameObjectWithTag(TagManager.LEVEL).GetComponent<Level>();

        GameManager.instance.audioManager.SetCurrentLevelMusic(gameLevels[level].levelMusic);
        GameManager.instance.player.GetNotControlledCharacter().Teleport(GetCurrentLevelVoidPosition());
        GameManager.instance.player.GetControlledCharacter().Teleport(currentLevel.startPosition.position);
        GameManager.instance.cameraManager.LoadCamera(currentLevel.GetCurrentZoneCamera());

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

    public void AdvanceCurrentLevelZone()
    {
        currentLevel.AdvanceZone();
    }

    public Vector3 GetCurrentLevelStartPosition()
    {
        return currentLevel.startPosition.position;
    }

    public Vector3 GetCurrentLevelVoidPosition()
    {
        return currentLevel.voidPosition.position;
    }

    public Transform GetCurrentLevelSpawnedEntitiesHolder()
    {
        return currentLevel.spawnedEntitiesHolder;
    }
}
