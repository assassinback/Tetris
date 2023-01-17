using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelStartButton : MonoBehaviour
{
    public LevelInfo levelInfo;
    public Button startLevelButton;

    private void Start()
    {
        startLevelButton = GetComponent<Button>();
        startLevelButton.onClick.AddListener(StartLevel);
    }
    private void StartLevel()
    {
        if (LevelManager._instance.oldGameScene != null)
        {
            Debug.Log(SceneManager.sceneCount);
            if (SceneManager.sceneCount==1)
            {
                SceneManager.LoadScene("Game");
            }
            SceneManager.UnloadSceneAsync("MainMenu");
            
        }
        else
        {
            SceneManager.LoadScene("Game");
        }
        //TileManager._instance.currentLevelInfo = levelInfo;
        LevelManager._instance.currentLevelInfo = levelInfo;
    }
}
