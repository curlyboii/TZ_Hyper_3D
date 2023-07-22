using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FruitCollection : MonoBehaviour
{
    public Text taskText; // UI Text element to display the task

    private FruitType currentTaskFruit; // The current fruit type that the player needs to collect
    private int fruitsToCollect; // Number of fruits required to complete the current task
    private int collectedFruits = 0; // Number of collected fruits of the current task

    private void Start()
    {
        SetRandomTask();
        UpdateTaskText();
    }

    // Called when a fruit is dropped into the basket
    public void OnFruitCollected(FruitType fruitType)
    {
        if (fruitType == currentTaskFruit)
        {
            collectedFruits++;
            UpdateTaskText();

            if (collectedFruits >= fruitsToCollect)
            {
                // Complete the current task and set a new random task
                Debug.Log("Task completed!");
                collectedFruits = 0;
                SetRandomTask();
                UpdateTaskText();
            }
        }
        else
        {
            // Incorrect fruit, level failed logic here
            Debug.Log("Incorrect fruit! Level failed!");
        }
    }

    // Set a random fruit type and number of fruits to collect for the current task
    private void SetRandomTask()
    {
        System.Random random = new System.Random();
        currentTaskFruit = (FruitType)random.Next(0, 3); // 0: Apple, 1: Banana, 2: Grapes
        fruitsToCollect = random.Next(1, 6); // Random number of fruits to collect from 1 to 5
    }

    // Update the task text based on the current task fruit and collected fruits
    private void UpdateTaskText()
    {
        int remainingFruits = Mathf.Max(fruitsToCollect - collectedFruits, 0);
        taskText.text = collectedFruits + " of " + fruitsToCollect + " " + currentTaskFruit.ToString().ToLower() + " must be collected";
    }
}

public enum FruitType
{
    Apple,
    Banana,
    Grapes
}
