using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : StaticInstances<GameSceneManager>
{
    public enum GameScene
    {
        GAME_START = 0,
        GAME_PLAY = 1,
        GAME_END = 2
    }
    public void SetScene(GameScene scene)
    {
        SceneManager.LoadScene((int)scene);
    }
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
}
