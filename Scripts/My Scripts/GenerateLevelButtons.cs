using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GenerateLevelButtons : MonoBehaviour
{
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
            int rotationCount = 0;
            float speed = 0.35f;
            for (int i = 0; i < 50; i++)
            {

                LevelInfo levelData = new LevelInfo();
                levelData.levelCompleted = true;
                levelData.levelUnlocked = true;
                if (i > 0)
                {
                    levelData.levelUnlocked = true;
                }
                levelData.levelName = (i + 1).ToString();
                
                if (rotationCount<5)
                {
                    levelData.rotation = false;
                }
                else if(rotationCount>=5 && rotationCount<10)
                {
                    levelData.rotation = true;
                }
                else
                {
                    rotationCount = 0;
                }
                rotationCount++;
                speed -= 0.00048f;
                levelData.speed = speed;
                if(speed<=0)
                {
                    speed = 0.025f;
                }
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
        UIManager._instance.ShowLevelInfo();

    }
}


[System.Serializable]
public class LevelInfo
{
    public string levelName = "";
    public bool levelCompleted = false;
    public bool levelUnlocked = false;
    public bool rotation;
    public float speed = 1;
}
