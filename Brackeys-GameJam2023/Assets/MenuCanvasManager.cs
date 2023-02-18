using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCanvasManager : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            GameSceneManager.Instance.SetScene(GameSceneManager.GameScene.GAME_PLAY);
    }
}
