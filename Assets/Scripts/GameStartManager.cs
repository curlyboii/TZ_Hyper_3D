using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class GameStartManager : MonoBehaviour
{
    private const float ZERO_TIMER  = 0f;
    private const int LEVEL = 0;
    private const int DELAY_TIME_TO_RESTART = 1;

    public static GameStartManager instance; // access the GameManager from anywhere
    public GameObject gameOverPanel; // game over
    public GameObject tapToStartPanel; // tap to move panel
    public GameObject gamePlay; // task ui
    public GameObject winGame; // win screen
    public bool gameStarted; // game is Start?

    private bool isTaskCompleted = false;
    public bool isTaskCompletedGet { get { return isTaskCompleted; } }  // Check task

    public TextMeshProUGUI timerText; // UI Text element to display the timer countdown
    public float levelTime = 30f; // Total time for the level in seconds

    private float timer; // Timer to track the elapsed time after the game starts

    private FruitCollection fruitCollection; // Reference to the FruitCollection script
    private FruitSpawner fruitSpawner; // Reference to the FruitSpawner script

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        fruitCollection = FindObjectOfType<FruitCollection>();
        fruitSpawner = FindObjectOfType<FruitSpawner>();
    }

    private void Update()
    {
        if (!gameStarted)
        {
            if (Input.GetMouseButtonDown(0)) // first touch
            {
                GameStart();
            }
        }
        else
        {
            if (!isTaskCompleted)
            {
                // Update the timer after the game has started
                if (timer > ZERO_TIMER)
                {
                    timer -= Time.deltaTime;
                    UpdateTimerText();
                    if (timer <= ZERO_TIMER)
                    {
                        // Timer has reached 0, handle level failure here
                        fruitCollection.LevelFailed();
                    }
                }
            }
        }
    }

    void GameStart()
    {
        gameStarted = true;
        tapToStartPanel.SetActive(false);
        StartGame(); // Start the timer
        fruitCollection.StartGame(); // Start the fruit spawning
        fruitSpawner.StartFruitSpawn(); // Start the fruit spawning
        gamePlay.SetActive(true);
    }

    public void GameOver()
    {
        Invoke("DelayRestart", DELAY_TIME_TO_RESTART);
    }

    void DelayRestart()
    {
        gamePlay.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void GameWin()
    {
        gamePlay.SetActive(false);
        winGame.SetActive(true);

        isTaskCompleted = true;
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(LEVEL);
    }

    // Method to start the timer
    private void StartGame()
    {
        timer = levelTime; // Set the timer to 30 seconds or any other value you desire
        UpdateTimerText();
    }

    // Method to add time to the timer (called from FruitCollection)
    public void AddTime(float timeToAdd)
    {
        timer += timeToAdd;
    }

    // Method to get the remaining time on the timer
    public float GetRemainingTime()
    {
        return timer;
    }

    // Update the timer text
    private void UpdateTimerText()
    {
        float minimum = 0f;
        float clampedTimer = Mathf.Clamp(timer, minimum, Mathf.Infinity);

        int minutes = Mathf.FloorToInt(clampedTimer / 60f);
        int seconds = Mathf.FloorToInt(clampedTimer % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        int timeToColoredRed = 10;

        if (timer <= timeToColoredRed)
        {
            timerText.color = Color.red;
        }
        else
        {
            timerText.color = Color.white; // Change back to the original color (white, for example) if more than 10 seconds
        }
    }
}
