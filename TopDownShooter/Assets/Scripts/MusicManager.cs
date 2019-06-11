﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioClip mainTheme;
    public AudioClip menuTheme;
    string sceneName;
    void Start () {
        OnLevelWasLoaded (0);
    }

    void Update () {
        if (Input.GetKeyDown (KeyCode.Space)) {
            AudioManager.instance.PlayMusic (mainTheme, 2);
        }
    }

    void OnLevelWasLoaded (int sceneIndex) {
        string newSceneName = SceneManager.GetActiveScene ().name;
        if (newSceneName != sceneName) {
            sceneName = newSceneName;
            Invoke ("PlayMusic",.2f);
        }
    }

    void PlayMusic () {
        AudioClip clipToPlay = null;
        switch (sceneName) {
            case "Menu":
                clipToPlay = menuTheme;
                break;
            case "Game":
                clipToPlay = mainTheme;
                break;
            default:
                break;
        }
        if (clipToPlay != null) {
            AudioManager.instance.PlayMusic (clipToPlay, 2);
            Invoke ("PlayMusic", clipToPlay.length);
        }
    }
}