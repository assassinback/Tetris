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
    public GameObject optionScreen;
    public GameObject InfiniteButton;
    private void Awake()
    {
        _instance= this;
    }
    private void Start()
    {
        if(LevelManager._instance.levelSelectClicked)
        {
            levelSelectScreen.SetActive(true);
            homeScreen.SetActive(false);
        }
        LevelManager._instance.levelSelectClicked = false;
        GoogleAdsScript._instance.RequestBanner();
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
        if (levelInfo[levelInfo.Count-1].levelCompleted)
        {
            //GameObject button = Instantiate(InfiniteButton, levelButtonParent.transform);
            //button.GetComponent<Image>().sprite = levelUnlockedIcon;
            //button.GetComponent<Button>().onClick.RemoveAllListeners();
            //button.GetComponent<Button>().onClick.AddListener(InfiniteButtonClicked);
        }
        else
        {
            //GameObject button = Instantiate(InfiniteButton, levelButtonParent.transform);
            //button.GetComponent<Image>().sprite = levelLockedIcon;
        }
    }
    public void PlayButtonClicked()
    {
        for (int i = 0; i < GenerateLevelButtons._instance.levelInfo.Count; i++)
        {
            try
            {
                if (!GenerateLevelButtons._instance.levelInfo[i].levelUnlocked)
                {
                    LevelManager._instance.currentLevelInfo = GenerateLevelButtons._instance.levelInfo[i - 1];
                    SceneManager.LoadScene("Game");
                    break;
                }
            }
            catch (System.Exception)
            {
                //if(GenerateLevelButtons._instance.levelInfo[GenerateLevelButtons._instance.levelInfo.Count - 1].levelCompleted)
                //{
                //    LevelManager._instance.infiniteMode = true;
                //}
                LevelManager._instance.currentLevelInfo = GenerateLevelButtons._instance.levelInfo[GenerateLevelButtons._instance.levelInfo.Count - 1];
                break;
            }
        }
        //if (GenerateLevelButtons._instance.levelInfo[GenerateLevelButtons._instance.levelInfo.Count - 1].levelCompleted)
        //{
        //    LevelManager._instance.infiniteMode = true;
        //}
        LevelManager._instance.currentLevelInfo = GenerateLevelButtons._instance.levelInfo[GenerateLevelButtons._instance.levelInfo.Count - 1];
        SceneManager.LoadScene("Game");
    }
    public void CloseLevelScreen()
    {
        levelSelectScreen.SetActive(false);
        homeScreen.SetActive(true);
    }
    public void CloseOptionScreen()
    {
        optionScreen.SetActive(false);
        homeScreen.SetActive(true);
    }
    public void OpenOptionScreen()
    {
        optionScreen.SetActive(true);
        homeScreen.SetActive(false);
    }
    public void ResumeButtonClicked()
    {
        levelSelectScreen.SetActive(true);
        homeScreen.SetActive(false);
    }
    public void InfiniteButtonClicked()
    {
        //for (int i = 0; i < GenerateLevelButtons._instance.levelInfo.Count; i++)
        //{
        //    try
        //    {
        //        if (!GenerateLevelButtons._instance.levelInfo[i].levelUnlocked)
        //        {
        //            LevelManager._instance.currentLevelInfo = GenerateLevelButtons._instance.levelInfo[i - 1];
        //            LevelManager._instance.infiniteMode = true;
        //            SceneManager.LoadScene("Game");
        //            return;
        //        }
        //    }
        //    catch (System.Exception)
        //    {
        //        LevelManager._instance.currentLevelInfo = GenerateLevelButtons._instance.levelInfo[GenerateLevelButtons._instance.levelInfo.Count - 1];
        //        break;
        //    }

        //}
        LevelManager._instance.currentLevelInfo = GenerateLevelButtons._instance.levelInfo[GenerateLevelButtons._instance.levelInfo.Count - 1];
        LevelManager._instance.infiniteMode= true;
        SceneManager.LoadScene("Game");
    }
}
