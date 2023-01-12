using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager _instance;
    public LevelInfo currentLevelInfo;
    public bool levelSelectClicked=false;
    public int highScore=0;
    public bool infiniteMode = false;
    private void Awake()
    {
        _instance= this;
    }
    private void Start()
    {
        if(!PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", 0);
        }
        highScore = PlayerPrefs.GetInt("HighScore");
    }
    public void SetHighScore(int score)
    {
        PlayerPrefs.SetInt("HighScore",score);
        highScore= score;
    }
    public int GetHighScore()
    {
        return PlayerPrefs.GetInt("HighScore");
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
