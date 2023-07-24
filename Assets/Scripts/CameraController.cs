using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const int ZERO_ON_Z_AXIS = 0;


    public Transform player; // Reference to the player's transform
    public float moveSpeed = 5f; // Speed of the camera movement towards the player
    public float rotationSpeed = 5f; // Speed of the camera rotation
    public float xRotationAngle = 45f; // Custom angle for the camera on the X-axis
    public Animator playerAnimator; // Reference to the player's animator

    private string[] danceAnimations = { "Dance1", "Dance2", "Dance3" }; // all 3 dance

    private bool isTaskCompleted = false;

    // Call this function to trigger the camera movement and player animation
    public void HandleTaskCompleted()
    {
        if (isTaskCompleted)
            return;

        // Move the camera towards the player
        Vector3 targetPosition = player.position + new Vector3(0f, 2f, -5f); // Adjust the position 
        StartCoroutine(MoveCamera(targetPosition));

        //random dance
        int randomIndex = Random.Range(0, danceAnimations.Length);
        string danceAnimation = danceAnimations[randomIndex];

        //// Trigger the player's dance animation
        playerAnimator.SetTrigger(danceAnimation);

        isTaskCompleted = true;
    }

    // Coroutine to smoothly move the camera to the target position
    private IEnumerator MoveCamera(Vector3 targetPosition)
    {
        Quaternion targetRotation = Quaternion.Euler(xRotationAngle, transform.eulerAngles.y, ZERO_ON_Z_AXIS);

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f || Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
