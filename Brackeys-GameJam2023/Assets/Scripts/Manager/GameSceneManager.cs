using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : StaticInstances<GameSceneManager>
{
    int totalCount;
    int iterator;
    public enum GameScene
    {
        GAME_START = 0,
        GAME_PLAY = 1,
        GAME_END = 2
    }
    public void SetScene(GameScene scene)
    {
        int sceneNumber = 0;
        switch (scene)
        {
            case GameScene.GAME_PLAY:
                sceneNumber = iterator + 1;
                iterator = ((iterator+1) % totalCount);
                break;
            case GameScene.GAME_END:
                sceneNumber = iterator + 1;
                break;
            default:
                sceneNumber = 0;
                break;
        }
        SceneManager.LoadScene(sceneNumber);
    }
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        totalCount = SceneManager.sceneCount - 2;
        iterator = 0;
    }
}
