using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    private int lines;

    public int level = 1;
    public int currentLevel = 0;
    public int linesPerLevel = 3;

    public Text linesText;
    public Text levelText;
    public Text scoreText;
    public bool infiniteMode;
    public bool didLevelUp = false;

    private const int minLines = 1;
    private const int maxLines = 4;

    public ParticlePlayer levelUpFx;

    private void Start() => Reset();

    private void UpdateUIText()
    {
        if (linesText)
        {
            linesText.text = lines.ToString();
        }
        if (levelText)
        {
            levelText.text = level.ToString();
        }
        if (scoreText)
        {
            scoreText.text = PadZero(score, 5);
        }
    }

    private string PadZero(int n, int padDigits)
    {
        string nStr = n.ToString();

        while (nStr.Length < padDigits)
        {
            nStr = "0" + nStr;
        }

        return nStr;
    }

    public void ScoreLines(int n)
    {
        didLevelUp = false;
        if(!infiniteMode)
        {
            currentLevel = int.Parse(LevelManager._instance.currentLevelInfo.levelName);
        }
        else
        {
            currentLevel = level;
        }
        n = Mathf.Clamp(n, minLines, maxLines);
        if(!infiniteMode)
        {
            switch (n)
            {
                case 1:
                    score += 40 * level * currentLevel;
                    break;
                case 2:
                    score += 100 * level * currentLevel;
                    break;
                case 3:
                    score += 300 * level * currentLevel;
                    break;
                case 4:
                    score += 1200 * level * currentLevel;
                    break;
            }
        }
        else
        {
            switch (n)
            {
                case 1:
                    score += 40 * level;
                    break;
                case 2:
                    score += 100 * level;
                    break;
                case 3:
                    score += 300 * level;
                    break;
                case 4:
                    score += 1200 * level;
                    break;
            }
        }
        

        lines -= n;

        if (lines <= 0)
        {
            LevelUp();
        }

        UpdateUIText(); 
    }

    public void Reset()
    {
        if(LevelManager._instance.infiniteMode)
        {
            infiniteMode = true;
        }
        level = 1;
        
        UpdateUIText();
        if (LevelManager._instance.infiniteMode)
        {
            level=int.Parse(GameController._instance.levelInfo.levelName);
        }
        lines = linesPerLevel * level;
    }

    public void LevelUp()
    {
        level++;
        lines = linesPerLevel * level;
        didLevelUp = true;
        if(infiniteMode)
        {
            
            GameController._instance.levelInfo.levelCompleted = true;
            if (int.Parse(GameController._instance.levelInfo.levelName) < GenerateLevelButtons._instance.levelInfo.Count)
            {
                GenerateLevelButtons._instance.levelInfo[int.Parse(GameController._instance.levelInfo.levelName) + 1].levelUnlocked = true;
                GenerateLevelButtons._instance.SaveLevelInfo();
            }
            //GameController._instance.dropInterval -= 0.005f;
            //GameController._instance.dropIntervalModded -= 0.005f;
        }
        
        if (levelUpFx)
        {
            levelUpFx.Play();
        }

    }
}
