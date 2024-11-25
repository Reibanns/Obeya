using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro; // Import TextMesh Pro namespace

public class Night5Script : NightBaseScript
{
    public GameObject endingPanel; // Reference to the UI Panel for ending text
    public TextMeshProUGUI endingText; // Reference to the TextMesh Pro component for displaying the ending

    // Keep this protected
    protected override void OnNightComplete()
    {
        Debug.Log("Night 5 complete. Game is done.");
        ShowEnding(); // Show the ending based on the accepted guests
    }

    // Public method to trigger OnNightComplete
    public void TriggerNightComplete()
    {
        OnNightComplete(); // Calls the protected method
    }

    private void ShowEnding()
    {
        NightManager nightManager = FindObjectOfType<NightManager>();

        int humansAccepted = 0;
        int aswangsAccepted = 0;

        // Count accepted guests
        foreach (var guest in nightManager.guestList)
        {
            if (guest.CompareTag("Human"))
            {
                humansAccepted++;
            }
            else if (guest.CompareTag("Aswang"))
            {
                aswangsAccepted++;
            }
        }

        // Log how many humans and aswangs were accepted
        Debug.Log($"Humans Accepted: {humansAccepted}");
        Debug.Log($"Aswangs Accepted: {aswangsAccepted}");

        // Check conditions for the ending
        if (humansAccepted >= 8 && aswangsAccepted == 0)
        {
            StartCoroutine(DisplayEnding("Good Ending: Good job ggs well played xD"));
        }
        else if (humansAccepted >= 4 && aswangsAccepted == 0)
        {
            StartCoroutine(DisplayEnding("Basic Ending: You survived, but the future is uncertain as the Aswang still pose a threat to society."));
        }
        else
        {
            StartCoroutine(DisplayEnding("Bad Ending: You have failed to protect the house from the Aswangs. They have overrun your home."));
        }
    }

    private IEnumerator DisplayEnding(string endingMessage)
    {
        endingPanel.SetActive(true); // Activate the ending UI panel
        endingText.text = ""; // Clear the text first

        // Typewriter effect
        foreach (char letter in endingMessage.ToCharArray())
        {
            endingText.text += letter; // Add one letter at a time
            yield return new WaitForSeconds(0.05f); // Wait for a short duration before adding the next letter
        }

        Debug.Log(endingMessage); // Log the ending message for debugging
    }
}
