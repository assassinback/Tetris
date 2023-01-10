using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager _instance;
    public LevelInfo currentLevelInfo;
    public bool levelSelectClicked=false;
    private void Awake()
    {
        _instance= this;
    }
    public void GoHome()
    {
        SceneManager.LoadScene("MainMenu");
        levelSelectClicked=false;
    }
    public void GotoLevelSelect()
    {
        SceneManager.LoadScene("MainMenu");
        levelSelectClicked=true;
    }
}
