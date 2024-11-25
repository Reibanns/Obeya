using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DoorTrigger : MonoBehaviour
{
    public GuestManager guestManager;
    public TextMeshProUGUI promptText;

    private bool playerInRange = false;
    private bool canInteract = true;

    void Start()
    {
        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }
        guestManager.OnNewNightStarted += ReEnableInteraction;
    }

    void OnDestroy()
    {
        // Unsubscribe from events when the object is destroyed
        guestManager.OnNewNightStarted -= ReEnableInteraction;
        guestManager.OnGuestAccepted -= ReEnableInteraction;
        guestManager.OnGuestRejected -= ReEnableInteraction;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            UpdatePromptText();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (promptText != null)
            {
                promptText.gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (playerInRange && canInteract && Input.GetKeyDown(KeyCode.Q))
        {
            InteractWithDoor();
        }
    }

    private void InteractWithDoor()
    {
        // Disable interaction
        canInteract = false;
        
        // Hide the prompt
        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }

        // Interact with the door through GuestManager
        guestManager.InteractWithDoor();

        // Subscribe to events in GuestManager to re-enable interaction
        guestManager.OnGuestAccepted += ReEnableInteraction;
        guestManager.OnGuestRejected += ReEnableInteraction;
    }

    private void ReEnableInteraction()
    {
        canInteract = true;
        UpdatePromptText();

        // Unsubscribe from events
        guestManager.OnGuestAccepted -= ReEnableInteraction;
        guestManager.OnGuestRejected -= ReEnableInteraction;
    }

    private void UpdatePromptText()
    {
        if (promptText != null)
        {
            promptText.text = canInteract ? "[Q] Door" : "Locked";
            promptText.gameObject.SetActive(playerInRange);
        }
    }
}