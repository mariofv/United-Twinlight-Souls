using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<LevelAsset> gameLevels;

    private int currentLevelIndex = -1;
    private Level currentLevel;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator LoadLevel(int level)
    {
        GameManager.instance.EnterGameState(GameManager.GameState.LOADING_LEVEL, changeGameStateInput: false);

        yield return StartCoroutine(GameManager.instance.scenesManager.ChangeScene(gameLevels[level].levelScene));
        
        currentLevelIndex = level;
        currentLevel = GameObject.FindGameObjectWithTag(TagManager.LEVEL).GetComponent<Level>();

        GameManager.instance.cameraManager.SetCurrentCameraRail(currentLevel.levelCameraRail);
        GameManager.instance.audioManager.SetCurrentLevelMusic(gameLevels[level].levelMusic);

        GameManager.instance.EnterGameState(GameManager.GameState.COMBAT, changeGameStateInput: false);
    }
}
