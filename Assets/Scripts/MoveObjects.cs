using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjects : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private float initialHeight;
    private float maxHeight = 5f; // You can adjust this maximum height as needed
    private Rigidbody rb;
    private bool wasKinematic;

    // Reference to the FruitCollection script
    private FruitCollection fruitCollection;

    // Dictionary to map fruit tags to FruitType enum values
    private Dictionary<string, FruitType> fruitTagToType;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialHeight = transform.position.y;

        // Find the FruitCollection script in the scene
        fruitCollection = FindObjectOfType<FruitCollection>();

        // Initialize the fruit tag to FruitType mapping
        fruitTagToType = new Dictionary<string, FruitType>
        {
            { "Apple", FruitType.Apple },
            { "Banana", FruitType.Banana },
            { "Grapes", FruitType.Grapes }
            // Add more fruit tags and their corresponding FruitType enum values as needed
        };
    }

    void OnMouseDown()
    {
        offset = transform.position - GetMouseWorldPosition();
        isDragging = true;

        // Temporarily disable physics while dragging
        wasKinematic = rb.isKinematic;
        rb.isKinematic = true;
    }

    void OnMouseUp()
    {
        isDragging = false;

        // Re-enable physics when dragging is done
        rb.isKinematic = wasKinematic;

        // Check if the object is dropped inside the basket and collect the fruit
        //if (IsInsideBasket())
        //{
        //    string fruitTag = gameObject.tag; // Get the tag of the fruit GameObject

        //    // Convert the fruit tag to FruitType enum value using the dictionary
        //    if (fruitTagToType.TryGetValue(fruitTag, out FruitType fruitType))
        //    {
        //        fruitCollection.OnFruitCollected(fruitType); // Pass the FruitType to the FruitCollection script
        //        Destroy(gameObject); // Destroy the fruit object after it is collected
        //    }
        //}
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePos = GetMouseWorldPosition();
            float adjustedHeight = Mathf.Clamp(mousePos.y + offset.y, initialHeight, initialHeight + maxHeight);
            transform.position = new Vector3(mousePos.x + offset.x, adjustedHeight, mousePos.z + offset.z);

            // Allow rotation while dragging
            float rotationSpeed = 5f;
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            transform.Rotate(Vector3.up, horizontalInput * rotationSpeed, Space.World);
            transform.Rotate(Vector3.right, verticalInput * rotationSpeed, Space.World);
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float distance;
        groundPlane.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }
}
