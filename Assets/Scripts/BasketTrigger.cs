using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BasketTrigger : MonoBehaviour
{
    // Reference to the FruitCollection script
    private FruitCollection fruitCollection;
    [SerializeField] GameObject spawnEffectPrefab; // Particle System Prefab



    void Start()
    {
        // Find the FruitCollection script in the scene
        fruitCollection = FindObjectOfType<FruitCollection>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has one of the fruit tags ("Apple", "Banana", "Grapes")
        if (other.CompareTag("Apple") || other.CompareTag("Banana") || other.CompareTag("Grapes"))
        {
            // Get the FruitType from the tag of the colliding object
            FruitType fruitType = GetFruitTypeFromTag(other.tag);
            if (fruitType != FruitType.None)
            {
                fruitCollection.OnFruitCollected(fruitType); // Pass the FruitType to the FruitCollection script
                if (spawnEffectPrefab != null)
                {
                    Instantiate(spawnEffectPrefab, transform.position, Quaternion.identity);
                }


                // Destroy(other.gameObject); // Destroy the fruit object after it is collected
            }
        }
    }

    // Method to convert the tag string to the corresponding FruitType enum value
    private FruitType GetFruitTypeFromTag(string tag)
    {
        switch (tag)
        {
            case "Apple":
                return FruitType.Apple;
            case "Banana":
                return FruitType.Banana;
            case "Grapes":
                return FruitType.Grapes;
            default:
                return FruitType.None; // Return None if the tag doesn't match any known fruits
        }
    }
}
