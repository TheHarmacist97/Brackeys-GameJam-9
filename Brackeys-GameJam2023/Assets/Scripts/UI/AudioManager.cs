using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : StaticInstances<AudioManager>
{
    [SerializeField] private List<AudioClip> backGroundMusic;
    private AudioSource audioSource;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        SetMusic(GameSceneManager.Instance.GetCuerrentScene());
    }
    private void OnEnable() => GameSceneManager.Instance.sceneChanged += SetMusic;
    private void OnDisable() => GameSceneManager.Instance.sceneChanged -= SetMusic;
    private void SetMusic(GameSceneManager.GameScene scene)
    {
        if (scene == GameSceneManager.GameScene.GAME_PLAY)
        {
            audioSource.clip = backGroundMusic[1];
        }
        else
        {
            audioSource.clip = backGroundMusic[0];
        }
        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
    public void Mute(bool shouldMute)
    {
       audioSource.mute = shouldMute;
    }
}
