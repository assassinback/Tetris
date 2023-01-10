using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GenerateLevelButtons : MonoBehaviour
{
    //Dummy class. Use different settings or provide .NET dll files for better decompilation output
    public static GenerateLevelButtons _instance;
    public List<LevelInfo> levelInfo;
    public string levelFileName = "LevelInfo";
    private void Awake()
    {
        _instance = this;

    }
    public void GetLevelInfo()
    {
        SaveSystem saveSystem = new SaveSystem();
        try
        {
            var x = ((Newtonsoft.Json.Linq.JArray)saveSystem.GetData(levelFileName)).ToObject<List<LevelInfo>>();
            levelInfo = x;
        }
        catch (System.Exception)
        {

        }
        if (levelInfo.Count == 0)
        {
            levelInfo = new List<LevelInfo>();
            for (int i = 0; i < 50; i++)
            {

                LevelInfo levelData = new LevelInfo();
                levelData.levelCompleted = false;
                levelData.levelUnlocked = true;
                if (i > 0)
                {
                    levelData.levelUnlocked = false;
                }
                levelData.levelName = (i + 1).ToString();
                levelInfo.Add(levelData);
            }
            saveSystem.SaveData(levelInfo, levelFileName);

        }
    }
    public void SaveLevelInfo()
    {
        SaveSystem saveSystem = new SaveSystem();
        saveSystem.SaveData(levelInfo, levelFileName);
    }
    private void Start()
    {
        GetLevelInfo();

    }
}


[System.Serializable]
public class LevelInfo
{
    public string levelName = "";
    public bool levelCompleted = false;
    public bool levelUnlocked = false;
    public bool rotation;
    //public int levelStars = 0;
    //public float levelTime = 0;
    //public int rows=0;
    //public int columns = 0;
    //public int layers = 0;
}
