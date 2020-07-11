using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using VrLifeClient;

public enum SceneType
{
    MAINMENU = 1,
    ROOM = 2
}

public class SceneController : MonoBehaviour
{
    public static SceneController current;

    private int currentScene = (int)SceneType.MAINMENU;
    private ConcurrentQueue<int> _sceneQueue = new ConcurrentQueue<int>();
    private bool _sceneLoaded = true;

    private void Awake()
    {
        current = this;
        SceneManager.LoadSceneAsync(currentScene, LoadSceneMode.Additive);
        StartCoroutine(SceneCoroutine());
    }

    public void ToMainMenu()
    {
        Cursor.visible = true;
        _sceneQueue.Enqueue((int)SceneType.MAINMENU);
    }

    public void ToRoom()
    {
        Cursor.visible = false;
        _sceneQueue.Enqueue((int)SceneType.ROOM);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void ChangeScene(int sceneIdx)
    {
        if (VrLifeCore.IsExiting || currentScene == sceneIdx)
        {
            return;
        }
        _sceneLoaded = false;
        var scene = SceneManager.LoadSceneAsync(sceneIdx, LoadSceneMode.Additive);
        
        SceneManager.UnloadSceneAsync(currentScene);
        currentScene = sceneIdx;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene);
        _sceneLoaded = true;
    }

    IEnumerator SceneCoroutine()
    {
        while(true)
        {
            while(_sceneQueue.IsEmpty)
            {
                yield return new WaitForSeconds(0.2f);
            }
            int sceneIdx;
            while (!_sceneQueue.TryDequeue(out sceneIdx));
            ChangeScene(sceneIdx);
            while(!_sceneLoaded)
            {
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}
