using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    MAINMENU = 1,
    ROOM = 2
}

public class SceneController : MonoBehaviour
{
    public static SceneController current;

    private int currentScene = (int)SceneType.MAINMENU;

    private void Awake()
    {
        current = this;
        SceneManager.LoadSceneAsync(currentScene, LoadSceneMode.Additive);
    }

    public void ToMainMenu()
    {
        ChangeScene((int)SceneType.MAINMENU);
    }

    public void ToRoom()
    {
        ChangeScene((int)SceneType.ROOM);
    }

    private void ChangeScene(int sceneIdx)
    {
        var scene = SceneManager.LoadSceneAsync(sceneIdx, LoadSceneMode.Additive);
        scene.completed += _ => SceneManager.SetActiveScene(SceneManager.GetSceneAt(sceneIdx));
        SceneManager.UnloadSceneAsync(currentScene);
        currentScene = sceneIdx;
    }
}
