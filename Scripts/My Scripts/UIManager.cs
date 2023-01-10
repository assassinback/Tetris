using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager _instance;
    public GameObject levelButton;
    public GameObject levelButtonParent;
    public Sprite levelUnlockedIcon;
    public Sprite levelLockedIcon;
    public Button resumeButton;
    public Button playButton;
    public Button optionsButton;
    public GameObject levelSelectScreen;
    public GameObject homeScreen;
    private void Awake()
    {
        _instance= this;
    }
    public void ShowLevelInfo()
    {
        List<LevelInfo> levelInfo= GenerateLevelButtons._instance.levelInfo;
        for(int i=0;i<levelInfo.Count;i++)
        {
            if (levelInfo[i].levelUnlocked)
            {
                GameObject button = Instantiate(levelButton,levelButtonParent.transform);
                button.AddComponent<LevelStartButton>();
                button.GetComponent<Image>().sprite = levelUnlockedIcon;
                button.GetComponent<LevelStartButton>().levelInfo= levelInfo[i];
                button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = levelInfo[i].levelName;
            }
            else
            {
                GameObject button = Instantiate(levelButton, levelButtonParent.transform);
                button.GetComponent<Image>().sprite = levelLockedIcon;
            }
        }
    }
    //Complete Commented Code Later
    public void PlayButtonClicked()
    {
        for (int i = 0; i < GenerateLevelButtons._instance.levelInfo.Count; i++)
        {
            try
            {
                if (!GenerateLevelButtons._instance.levelInfo[i].levelUnlocked)
                {
                    //currentLevelInfo = GenerateLevelButtons._instance.levelInfo[i - 1];
                    //BoardManager._instance.table = LevelManager._instance.levelPattern[i - 1];
                    break;
                }
            }
            catch (System.Exception)
            {
                //currentLevelInfo = GenerateLevelButtons._instance.levelInfo[GenerateLevelButtons._instance.levelInfo.Count - 1];
                //BoardManager._instance.table = GenerateLevelButtons._instance.levelInfo[GenerateLevelButtons._instance.levelInfo.Count - 1];
                break;
            }

        }
        //currentLevelInfo = LevelManager._instance.levelCount[LevelManager._instance.levelCount.Count - 1];
        //BoardManager._instance.table = LevelManager._instance.levelPattern[LevelManager._instance.levelPattern.Count - 1];
        SceneManager.LoadScene("Game");
        //UIManager._instance.SetLevelNameText("Level " + currentLevelInfo.levelName);
    }
    public void CloseLevelScreen()
    {
        levelSelectScreen.SetActive(false);
        homeScreen.SetActive(true);
    }
    public void ResumeButtonClicked()
    {
        levelSelectScreen.SetActive(true);
        homeScreen.SetActive(false);
    }
}
