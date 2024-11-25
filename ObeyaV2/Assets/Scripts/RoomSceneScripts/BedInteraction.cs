using UnityEngine;
using UnityEngine.UI; // Required for UI components
using TMPro; // For TextMeshPro
using System.Collections;

public class BedInteraction : MonoBehaviour
{
    public NightManager nightManager; // Reference to the NightManager
    private bool canSleep = false; // Indicates if the player can sleep
    private bool playerInBedArea = false; // Track if the player is in the bed area

    public GameObject nightPanel; // Panel to display night information
    public TextMeshProUGUI nightText; // TextMeshProUGUI component for displaying the night text
    public float fadeDuration = 1f; // Duration for fading in and out
    public PlayerMovement playerMovement; // Reference to the PlayerMovement script

    [Header("Cannot Sleep Message")]
    public GameObject cannotSleepPanel; // Panel to display the cannot sleep message
    public TextMeshProUGUI cannotSleepText; // TextMeshProUGUI component for displaying the cannot sleep message
    public float cannotSleepMessageDuration = 5f; // Duration to display the cannot sleep message

    private void Start()
    {
        if (cannotSleepPanel != null)
        {
            cannotSleepPanel.SetActive(false); // Ensure the panel is initially hidden
        }
        if (cannotSleepText != null)
        {
            cannotSleepText.enabled = false; // Ensure the text is initially disabled
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInBedArea = true; // Set the flag to true when player enters
            if (canSleep)
            {
                Debug.Log("Player is at the bed. Press 'E' to sleep and proceed to the next night.");
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInBedArea = false; // Set the flag to false when player exits
            Debug.Log("Player left the bed.");
        }
    }

    void Update()
    {
        // Allow the player to sleep if the night is complete and the player is in bed area
        if (playerInBedArea && Input.GetKeyDown(KeyCode.E) && canSleep)
        {
            StartCoroutine(ShowNightTransition()); // Show night transition UI
            canSleep = false; // Reset sleep state for the next night
        }
        else if (playerInBedArea && Input.GetKeyDown(KeyCode.E) && !canSleep)
        {
            Debug.Log("You can't sleep now!");
            ShowCannotSleepMessage();
        }
    }

    private void ShowCannotSleepMessage()
    {
        if (cannotSleepPanel != null && cannotSleepText != null)
        {
            cannotSleepText.text = "You have a visitor at your doorstep. You cannot sleep yet.";
            cannotSleepPanel.SetActive(true);
            cannotSleepText.enabled = true; // Explicitly enable the text
            StartCoroutine(HideCannotSleepMessage());
        }
        else
        {
            Debug.LogWarning("Cannot Sleep Panel or Text is not assigned in the inspector!");
        }
    }


    private IEnumerator HideCannotSleepMessage()
    {
        yield return new WaitForSeconds(cannotSleepMessageDuration);
        cannotSleepText.enabled = false; // Disable the text first
        cannotSleepPanel.SetActive(false); // Then disable the panel
    }

    public void EnableSleeping()
    {
        canSleep = true; // Allow the player to sleep once all guests are handled
    }

    public void DisableSleeping()
    {
        canSleep = false; // Disallow sleeping
    }

    private IEnumerator ShowNightTransition()
    {
        // Disable player movement
        playerMovement.enabled = false;

        int currentNight = nightManager.currentNight; // Get the current night

        // Set the text to display the current night
        nightText.text = "Night " + (currentNight + 1);

        // Fade in the panel
        nightPanel.SetActive(true);
        yield return StartCoroutine(FadePanel(0f, 1f)); // Fade in

        // Wait for a moment to display the night text
        yield return new WaitForSeconds(2f); // Display time for the night text

        // Fade out the panel
        yield return StartCoroutine(FadePanel(1f, 0f)); // Fade out

        nightPanel.SetActive(false); // Hide the panel after fading out
        nightManager.ProceedToNextNight(); // Proceed to the next night

        // Re-enable player movement
        playerMovement.enabled = true;
    }

    private IEnumerator FadePanel(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        CanvasGroup canvasGroup = nightPanel.GetComponent<CanvasGroup>();

        // Ensure the panel has a CanvasGroup component
        if (canvasGroup == null)
        {
            canvasGroup = nightPanel.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = startAlpha; // Set initial alpha
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            yield return null; // Wait for the next frame
        }

        canvasGroup.alpha = endAlpha; // Ensure final alpha is set
    }
}
