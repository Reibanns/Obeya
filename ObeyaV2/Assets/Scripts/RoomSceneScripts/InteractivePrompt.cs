using UnityEngine;
using TMPro; // For TextMeshPro

public class InteractivePrompt : MonoBehaviour
{
    public TextMeshProUGUI promptText; // TextMeshPro component for displaying the prompt text
    public string interactionText = "Press 'E' to interact"; // Default text for interaction

    private bool isInteracting = false; // Track if the player is interacting

    private void Start()
    {
        promptText.gameObject.SetActive(false); // Initially hide the prompt text
    }

    private void Update()
    {
        // Handle interaction input only when interacting
        if (isInteracting && Input.GetKeyDown(KeyCode.E))
        {
            // Here you can call a function to handle interaction if needed
            HandleInteraction();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartInteracting(); // Start interaction
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StopInteracting(); // Stop interaction
        }
    }

    private void StartInteracting()
    {
        isInteracting = true;
        promptText.text = interactionText; // Set the prompt text
        promptText.gameObject.SetActive(true); // Show the prompt text
    }

    private void HandleInteraction()
    {
        // Implement what happens when the player interacts
        // For example, you might want to open a dialogue or perform an action
        Debug.Log("Interacted with the object!");
    }

    private void StopInteracting()
    {
        isInteracting = false;
        promptText.gameObject.SetActive(false); // Hide the prompt text
    }
}


