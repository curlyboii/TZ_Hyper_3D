using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FruitCollection : MonoBehaviour
{
    public TextMeshProUGUI taskText; // UI Text element to display the task

    private FruitType currentTaskFruit; // The current fruit type that the player needs to collect
    private int fruitsToCollect; // Number of fruits required to complete the current task
    private int collectedFruits = 0; // Number of collected fruits of the current task


    private bool isLevelFailed = false;

    public GameObject plusText; // Text +3 seconds

    public CameraController cameraController; // Reference to the CameraController script

    // Called when a fruit is dropped into the basket
    public void OnFruitCollected(FruitType fruitType)
    {
        if (!isLevelFailed && fruitType == currentTaskFruit)
        {
            collectedFruits++;
            UpdateTaskText();
            float timeToAdd = 3f;
            GameStartManager.instance.AddTime(timeToAdd); // Add 3 seconds to the timer on successful task completion

            if (plusText != null)
            {
                GameObject text = Instantiate(plusText, new Vector3(transform.position.x + 3f, transform.position.y + 5f, transform.position.z), plusText.transform.rotation); // spawn text
                Destroy(text, 1f); // destroy text
            }


            if (collectedFruits >= fruitsToCollect)
            {
                // Complete the current task and set a new random task

                collectedFruits = 0;
                SetRandomTask();
                UpdateTaskText();

                cameraController.HandleTaskCompleted();

                GameStartManager.instance.GameWin();


            }
        }
        else if (!isLevelFailed)
        {
            // Incorrect fruit, level failed 
            LevelFailed();
        }
    }

    // Set a random fruit type and number of fruits to collect for the current task
    private void SetRandomTask()
    {
        System.Random random = new System.Random();
        currentTaskFruit = (FruitType)random.Next(1, 4); // 1: Apple, 2: Banana, 3: Grapes
        fruitsToCollect = random.Next(1, 6); // Random number of fruits to collect from 1 to 5
    }

    // Update the task text based on the current task fruit and collected fruits
    private void UpdateTaskText()
    {
        int remainingFruits = Mathf.Max(fruitsToCollect - collectedFruits, 0);
        if(taskText != null)
        {
            taskText.text = collectedFruits + " of " + fruitsToCollect + " " + currentTaskFruit.ToString().ToLower() + " must be collected";
        }
        
    }

    public void LevelFailed()
    {
        isLevelFailed = true;
        GameStartManager.instance.GameOver();
    }

    // Method to start the game (called from GameStartManager)
    public void StartGame()
    {
        isLevelFailed = false;
        SetRandomTask();
        UpdateTaskText();
    }
}


public enum FruitType
{
    None,
    Apple,
    Banana,
    Grapes
}
