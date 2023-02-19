using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle fppToggle;
    private void Awake()
    {
        if (!PlayerPrefs.HasKey(GameConfig.Constants.PLAYERPREFS_FPPTOGGLE))
        {
            PlayerPrefs.SetInt(GameConfig.Constants.PLAYERPREFS_FPPTOGGLE, 1);
        }
        if (!PlayerPrefs.HasKey(GameConfig.Constants.PLAYERPREFS_MUSIC))
        {
            PlayerPrefs.SetInt(GameConfig.Constants.PLAYERPREFS_MUSIC, 1);
        }
        ToggleMusic(PlayerPrefs.GetInt(GameConfig.Constants.PLAYERPREFS_MUSIC) == 1);
        ToggleFPP(PlayerPrefs.GetInt(GameConfig.Constants.PLAYERPREFS_FPPTOGGLE) == 1);
        fppToggle.SetIsOnWithoutNotify(PlayerPrefs.GetInt(GameConfig.Constants.PLAYERPREFS_FPPTOGGLE) == 1);
        musicToggle.SetIsOnWithoutNotify(PlayerPrefs.GetInt(GameConfig.Constants.PLAYERPREFS_MUSIC) == 1);


    }
    public void ToggleMusic(bool isOn)
    {
        PlayerPrefs.SetInt(GameConfig.Constants.PLAYERPREFS_MUSIC, isOn ? 1 : 0);
        AudioManager.Instance.Mute(!isOn);
    }
    public void ToggleFPP(bool isOn)
    {
        PlayerPrefs.SetInt(GameConfig.Constants.PLAYERPREFS_FPPTOGGLE, isOn ? 1 : 0);
        GameConfig.fppToggle = isOn;
    }
    public void PauseGame(bool isPaused)
    {
        Time.timeScale = isPaused ? 0 : 1;
        _pauseMenu.SetActive(isPaused);

    }
}
