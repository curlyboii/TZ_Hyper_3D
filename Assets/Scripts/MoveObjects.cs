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

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    OnTouchBegin(touch.position);
                    break;

                case TouchPhase.Moved:
                    OnTouchMoved(touch.position);
                    break;

                case TouchPhase.Ended:
                    OnTouchEnded();
                    break;
            }
        }
    }

    private void OnTouchBegin(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                offset = transform.position - GetTouchWorldPosition(touchPosition);
                isDragging = true;

                // Temporarily disable physics while dragging
                wasKinematic = rb.isKinematic;
                rb.isKinematic = true;
            }
        }
    }

    private void OnTouchMoved(Vector2 touchPosition)
    {
        if (isDragging)
        {
            Vector3 touchPos = GetTouchWorldPosition(touchPosition);
            float adjustedHeight = Mathf.Clamp(touchPos.y + offset.y, initialHeight, initialHeight + maxHeight);
            transform.position = new Vector3(touchPos.x + offset.x, adjustedHeight, touchPos.z + offset.z);

            // Allow rotation while dragging
            float rotationSpeed = 5f;
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            transform.Rotate(Vector3.up, horizontalInput * rotationSpeed, Space.World);
            transform.Rotate(Vector3.right, verticalInput * rotationSpeed, Space.World);
        }
    }

    private void OnTouchEnded()
    {
        isDragging = false;
        rb.isKinematic = wasKinematic;
    }

    private Vector3 GetTouchWorldPosition(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float distance;
        groundPlane.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }
}
