using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    private string currentScene;
    private string gameUIScene;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator ChangeScene(string newScene)
    {
        if (currentScene != null)
        {
            yield return StartCoroutine(UnLoadScene(currentScene));
        }

        currentScene = newScene;
        yield return StartCoroutine(LoadScene(newScene));
    }

    public IEnumerator LoadScene(string scene)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByPath(scene));
    }

    public IEnumerator UnLoadScene(string scene)
    {
        if (IsSceneLoaded(scene))
        {
            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(scene);

            while (!asyncOperation.isDone)
            {
                yield return null;
            }
        }
    }

    public IEnumerator LoadGameUIScene(string scene)
    {
        yield return StartCoroutine(LoadScene(scene));
        gameUIScene = scene;
    }

    public IEnumerator UnLoadGameUIScene()
    {
        if (gameUIScene != null)
        {
            yield return StartCoroutine(UnLoadScene(gameUIScene));
        }
        gameUIScene = null;
    }

    public bool IsSceneLoaded(string scene)
    {
        return SceneManager.GetSceneByName(scene).IsValid() || SceneManager.GetSceneByPath(scene).IsValid();
    }
}
