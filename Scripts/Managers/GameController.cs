﻿using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    private Board gameBoard;
    private Spawner spawner;
    private Shape activeShape;
    private Ghost ghost;
    private Holder holder;
    public static GameController _instance;
    private SoundManager soundManager;
    private ScoreManager scoreManager;
    public bool infiniteMode=false;
    public TextMeshProUGUI levelNameText;
    public TextMeshProUGUI levelScoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI levelScoreBigText;
    public float dropInterval = 1f;
    public float dropIntervalModded;
    public Button nextLevelButton;
    private float timeToDrop;
    private float timeToNextKeyLeftRight;
    private float timeToNextKeyDown;
    private float timeToNextKeyRotate;

    private float timeToNextSwipeLeftRight;
    private float timeToNextSwipeDown;
    public LevelInfo levelInfo;
    [Range(0.02f, 1)]
    public float keyRepeatRateLeftRight = 0.1f;

    [Range(0.02f, 1)]
    public float keyRepeatRateDown = 0.05f;

    [Range(0.02f, 1)]
    public float keyRepeatRateRotate = 0.05f;

    [Range(0.02f, 1)]
    public float swipeRepeatRateLeftRight = 0.25f;

    [Range(0.02f, 1)]
    public float swipeRepeatRateDown = 0.05f;

    public GameObject pausePanel;
    public GameObject gameOverPanel;

    public IconToggle rotateIconToggle;
    public ParticlePlayer gameOverFx;

    private bool gameOver = false;
    private bool rotateClockwise = true;

    public bool isPaused = false;

    private enum Direction { none, left, right, up, down }

    private Direction swipeDirection = Direction.none;
    private Direction swipeEndDirection = Direction.none;
    private void Awake()
    {
        _instance = this;
    }
    private void OnEnable()
    {
        TouchController.SwipeEvent += SwipeHandler;
        TouchController.SwipeEndEvent += SwipeEndHandler;
    }

    private void OnDisable()
    {
        TouchController.SwipeEvent -= SwipeHandler;
        TouchController.SwipeEndEvent -= SwipeEndHandler;
    }

    private void Start()
    {
        GoogleAdsScript._instance.RequestBanner();
        if (LevelManager._instance.infiniteMode)
        {
            infiniteMode = true;
        }
        
        gameBoard = GameObject.FindObjectOfType<Board>();
        spawner = GameObject.FindObjectOfType<Spawner>();

        ghost = GameObject.FindObjectOfType<Ghost>();
        holder = GameObject.FindObjectOfType<Holder>();

        soundManager = GameObject.FindObjectOfType<SoundManager>();
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();
        //scoreManager.level = 1;
        timeToNextKeyLeftRight = Time.time + keyRepeatRateLeftRight;
        timeToNextKeyRotate = Time.time + keyRepeatRateRotate;
        timeToNextKeyDown = Time.time + keyRepeatRateDown;

        if (!gameBoard)
        {
            Debug.Log("WARNING! There is no game board defined!");
        }

        if (!soundManager)
        {
            Debug.Log("WARNING! There is no sound manager defined!");
        }

        if (!scoreManager)
        {
            Debug.Log("WARNING! There is no score manager defined!");
        }


        if (!spawner)
        {
            Debug.Log("WARNING! There is no spawner defined!");
        }
        else
        {
            if (activeShape == null)
            {
                activeShape = spawner.SpawnShape();
            }

            spawner.transform.position = Vectorf.Round(spawner.transform.position);
        }

        if (gameOverPanel)
        {
            gameOverPanel.SetActive(false);
        }

        if (pausePanel)
        {
            pausePanel.SetActive(false);
        }
        
        levelInfo = LevelManager._instance.currentLevelInfo;
        dropIntervalModded = dropInterval;
        dropInterval = levelInfo.speed;
        dropIntervalModded = levelInfo.speed;
        if (levelInfo.rotation)
        {
            Camera.main.transform.rotation = Quaternion.Euler(0, 0, 180);
            Camera.main.transform.position = new Vector3(4.5f, 9.8f, -5);
        }
        else
        {
            Camera.main.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        levelNameText.text = levelInfo.levelName;
        if(infiniteMode)
        {
            scoreManager.level = int.Parse(levelInfo.levelName);
        }
        
        highScoreText.text = LevelManager._instance.highScore.ToString();
    }

    private void MoveRight()
    {
        activeShape.MoveRight();
        timeToNextKeyLeftRight = Time.time + keyRepeatRateLeftRight;
        timeToNextSwipeLeftRight = Time.time + swipeRepeatRateLeftRight;

        if (!gameBoard.IsValidPosition(activeShape))
        {
            activeShape.MoveLeft();
            PlaySound(soundManager.errorSound, 0.5f);
        }
        else
        {
            PlaySound(soundManager.moveSound, 0.5f);
        }
    }

    private void MoveLeft()
    {
        activeShape.MoveLeft();
        timeToNextKeyLeftRight = Time.time + keyRepeatRateLeftRight;
        timeToNextSwipeLeftRight = Time.time + swipeRepeatRateLeftRight;

        if (!gameBoard.IsValidPosition(activeShape))
        {
            activeShape.MoveRight();
            PlaySound(soundManager.errorSound, 0.5f);
        }
        else
        {
            PlaySound(soundManager.moveSound, 0.5f);
        }
    }

    private void Rotate()
    {
        activeShape.RotateClockwise(rotateClockwise);
        timeToNextKeyRotate = Time.time + keyRepeatRateRotate;

        if (!gameBoard.IsValidPosition(activeShape))
        {
            activeShape.RotateClockwise(!rotateClockwise);
            PlaySound(soundManager.errorSound, 0.5f);
        }
        else
        {
            PlaySound(soundManager.moveSound, 0.5f);
        }
    }

    private void MoveDown()
    {
        timeToDrop = Time.time + dropIntervalModded;
        timeToNextKeyDown = Time.time + keyRepeatRateDown;
        timeToNextSwipeDown = Time.time + swipeRepeatRateDown;

        if (activeShape)
        {
            activeShape.MoveDown();

            if (!gameBoard.IsValidPosition(activeShape))
            {
                if (gameBoard.IsOverLimit(activeShape))
                {
                    GameOver();
                }
                else
                {
                    LandShape();
                    LevelCompleteTest();
                }
            }
        }
    }
    public void GoHomeButton()
    {
        Time.timeScale = 1;
        LevelManager._instance.GoHome();

    }
    public void GotoLevelSelectButton()
    {
        Time.timeScale = 1;
        LevelManager._instance.GoHome();
    }
    private void PlayerInput()
    {
        if (!gameBoard || !spawner)
        {
            return;
        }

        if ((Input.GetButton("MoveRight") && Time.time > timeToNextKeyLeftRight) ||
             Input.GetButtonDown("MoveRight"))
        {
            MoveRight();
        }
        else if ((Input.GetButton("MoveLeft") && Time.time > timeToNextKeyLeftRight) ||
                  Input.GetButtonDown("MoveLeft"))
        {
            MoveLeft();
        }
        else if (Input.GetButtonDown("Rotate") && Time.time > timeToNextKeyRotate)
        {
            Rotate();
        }
        else if ((Input.GetButton("MoveDown") && Time.time > timeToNextKeyDown) ||
                  Time.time > timeToDrop)
        {
            MoveDown();
        }
        else if ((swipeDirection == Direction.right && Time.time > timeToNextSwipeLeftRight) ||
                  swipeEndDirection == Direction.right)
        {
            MoveRight();

            swipeDirection = Direction.none;
            swipeEndDirection = Direction.none;
        }
        else if ((swipeDirection == Direction.left && Time.time > timeToNextSwipeLeftRight) ||
                  swipeEndDirection == Direction.left)
        {
            MoveLeft();

            swipeDirection = Direction.none;
            swipeEndDirection = Direction.none;
        }
        else if (swipeEndDirection == Direction.up)
        {
            Rotate();
            swipeEndDirection = Direction.none;
        }
        else if (swipeDirection == Direction.down && Time.time > timeToNextSwipeDown)
        {
            MoveDown();
            swipeDirection = Direction.none;
        }
        else if (Input.GetButtonDown("ToggleRotation"))
        {
            ToggleRotationDirection();
        }
        else if (Input.GetButtonDown("Pause"))
        {
            TogglePause();
        }
        else if (Input.GetButtonDown("Hold"))
        {
            Hold();
        }
    }

    private void GameOver()
    {
        if (GoogleAdsScript._instance.interstitial.CanShowAd())
        {
            GoogleAdsScript._instance.interstitial.Show();
        }
        GoogleAdsScript._instance.RequestInterstitial();
        levelScoreText.text = scoreManager.score.ToString();
        levelScoreBigText.text = scoreManager.score.ToString();
        if(scoreManager.score>LevelManager._instance.highScore)
        {
            LevelManager._instance.SetHighScore(scoreManager.score);
            highScoreText.text = scoreManager.score.ToString();
        }
        activeShape.MoveUp();

        StartCoroutine(GameOverRoutine());

        PlaySound(soundManager.gameOverSound, 5f);
        PlaySound(soundManager.gameOverVocalClip, 5f);

        gameOver = true;
    }

    private IEnumerator GameOverRoutine()
    {
        if (gameOverFx)
        {
            gameOverFx.Play();
        }

        yield return new WaitForSeconds(0.5f);

        if (gameOverPanel)
        {
            gameOverPanel.SetActive(true);
        }
    }

    private void LandShape()
    {
        timeToNextKeyLeftRight = Time.time;
        timeToNextKeyRotate = Time.time;
        timeToNextKeyDown = Time.time;

        activeShape.MoveUp();
        gameBoard.StoreShapeInGrid(activeShape);

        if (ghost)
        {
            ghost.Reset();
        }

        if (holder)
        {
            holder.canRelease = true;
        }

        activeShape = spawner.SpawnShape();

        gameBoard.StartCoroutine(gameBoard.ClearAllRows());

        PlaySound(soundManager.dropSound, 0.75f);

        if (gameBoard.completedRows > 0)
        {
            scoreManager.ScoreLines(gameBoard.completedRows);

            if (scoreManager.didLevelUp)
            {
                PlaySound(soundManager.levelUpVocalClip);
                //dropIntervalModded = dropInterval - Mathf.Clamp(
                //        (((float)scoreManager.level - 1) * 0.1f), 0.05f, 1f);
            }
            else
            {
                if (gameBoard.completedRows > 1)
                {
                    AudioClip randomVocal = soundManager.GetRandomClip(soundManager.vocalClips);
                    PlaySound(randomVocal);
                }
            }

            PlaySound(soundManager.clearRowSound);
        }
    }

    private void PlaySound(AudioClip clip, float volumeMultiplier = 1f)
    {
        if (clip && soundManager.fxEnabled)
        {
            AudioSource.PlayClipAtPoint(
                clip, Camera.main.transform.position,
                Mathf.Clamp(soundManager.fxVolume * volumeMultiplier, 0.05f, 1f));
        }
    }
    public void LevelCompleteTest()
    {
        if (!infiniteMode)
        {
            if (scoreManager.score > (int.Parse(levelInfo.levelName) * 1500))
            {
                levelInfo.levelCompleted = true;
                if (int.Parse(levelInfo.levelName) < GenerateLevelButtons._instance.levelInfo.Count)
                {
                    GenerateLevelButtons._instance.levelInfo[int.Parse(levelInfo.levelName)].levelUnlocked = true;
                    LevelManager._instance.currentLevelInfo = GenerateLevelButtons._instance.levelInfo[int.Parse(levelInfo.levelName)];
                    //levelInfo = GenerateLevelButtons._instance.levelInfo[int.Parse(levelInfo.levelName) + 1];
                    GenerateLevelButtons._instance.SaveLevelInfo();
                    Start();
                    //nextLevelButton.onClick.RemoveAllListeners();
                    //nextLevelButton.onClick.AddListener(NextLevelClicked);
                }
            }
        }
        else
        {
            levelNameText.text = scoreManager.level.ToString();

        }
    }
    private void Update()
    {
        //LevelCompleteTest();
        //Debug.Log(levelInfo.levelName);
        //if(!infiniteMode)
        //{
        //    if (scoreManager.score > (int.Parse(levelInfo.levelName) * 1500))
        //    {
        //        levelInfo.levelCompleted = true;
        //        if (int.Parse(levelInfo.levelName) < GenerateLevelButtons._instance.levelInfo.Count)
        //        {
        //            GenerateLevelButtons._instance.levelInfo[int.Parse(levelInfo.levelName) + 1].levelUnlocked = true;
        //            LevelManager._instance.currentLevelInfo = GenerateLevelButtons._instance.levelInfo[int.Parse(levelInfo.levelName)];
        //            //levelInfo = GenerateLevelButtons._instance.levelInfo[int.Parse(levelInfo.levelName) + 1];
        //            GenerateLevelButtons._instance.SaveLevelInfo();
        //            Start();
        //            //nextLevelButton.onClick.RemoveAllListeners();
        //            //nextLevelButton.onClick.AddListener(NextLevelClicked);
        //        }
        //    }
        //}
        //else
        //{
        //    levelNameText.text = scoreManager.level.ToString();

        //}
        if (!gameBoard || !spawner || !activeShape || gameOver || !soundManager || !scoreManager)
        {
            return;
        }

        PlayerInput();
    }
    public void NextLevelClicked()
    {
        LevelManager._instance.currentLevelInfo = GenerateLevelButtons._instance.levelInfo[int.Parse(levelInfo.levelName) + 1];
        Restart();
    }
    private void LateUpdate()
    {
        if (ghost)
        {
            ghost.DrawGhost(activeShape, gameBoard);
        }
    }

    private void SwipeHandler(Vector2 swipeMovement)
    {
        swipeDirection = GetDirection(swipeMovement);
    }

    private void SwipeEndHandler(Vector2 swipeMovement)
    {
        swipeEndDirection = GetDirection(swipeMovement);
    }

    private Direction GetDirection(Vector2 swipeMovement)
    {
        Direction swipeDirection = Direction.none;

        if (Mathf.Abs(swipeMovement.x) > Mathf.Abs(swipeMovement.y))
        {
            swipeDirection = swipeMovement.x >= 0 ? Direction.right : Direction.left;
        }
        else
        {
            swipeDirection = swipeMovement.y >= 0 ? Direction.up : Direction.down;
        }

        return swipeDirection;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    public void ToggleRotationDirection()
    {
        rotateClockwise = !rotateClockwise;

        if (rotateIconToggle)
        {
            rotateIconToggle.ToggleIcon(rotateClockwise);
        }
    }

    public void TogglePause()
    {
        if (gameOver)
        {
            return;
        }

        isPaused = !isPaused;

        if (pausePanel)
        {
            pausePanel.SetActive(isPaused);

            if (soundManager)
            {
                //soundManager.musicSource.volume =
                //    isPaused ? soundManager.musicVolume * 0.25f : soundManager.musicVolume;
            }

            Time.timeScale = isPaused ? 0 : 1;
        }
    }

    public void Hold()
    {
        if (!holder)
        {
            return;
        }

        if (!holder.heldShape)
        {
            holder.Catch(activeShape);
            activeShape = spawner.SpawnShape();
            PlaySound(soundManager.holdSound);
        }
        else if (holder.canRelease)
        {
            Shape shape = activeShape;
            activeShape = holder.Release();
            activeShape.transform.position = spawner.transform.position;
            holder.Catch(shape);
            PlaySound(soundManager.holdSound);
        }
        else
        {
            PlaySound(soundManager.errorSound);
        }

        if (ghost)
        {
            ghost.Reset();
        }
    }
}
