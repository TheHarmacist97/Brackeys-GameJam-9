using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCanvasManager : MonoBehaviour
{
    private void Awake()
    {
        
    }
    public void LoadGamePlay()
    {
        GameSceneManager.Instance.SetScene(GameSceneManager.GameScene.GAME_PLAY);
    }
    public void QuitApp()
    {
        Debug.Log("Application Quit");
        Application.Quit();
    }
}
