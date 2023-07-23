using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDestroyer : MonoBehaviour
{
    // Reference to the destruction effect (particle system or animation)
    public GameObject destructionEffect;

    // This method is called when another collider enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        // Check if the entered object has the tag "Fruit"
        if (other.CompareTag("Apple") || other.CompareTag("Banana") || other.CompareTag("Grapes"))
        {
            // Instantiate the destruction effect at the object's position
            if (destructionEffect != null) // Check if destructionEffect is assigned before using it
            {
                Instantiate(destructionEffect, other.transform.position, other.transform.rotation);
            }

            // Destroy the entered object
            Destroy(other.gameObject);
        }
    }
}
