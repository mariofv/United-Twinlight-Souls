using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<Level> gameLevels;

    private int currentLevel = -1;


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

        GameManager.instance.EnterGameState(GameManager.GameState.COMBAT, changeGameStateInput: false);
    }
}
