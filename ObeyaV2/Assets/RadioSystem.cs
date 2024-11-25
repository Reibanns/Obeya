using UnityEngine;
using UnityEngine.UI;
using TMPro; // For TextMeshPro

public class RadioSystem : MonoBehaviour
{
    public TextMeshProUGUI radioText; // TextMeshPro component for displaying main text
    public TextMeshProUGUI mumblingGrowlText; // TextMeshPro component for mumbling growl
    public NightManager nightManager; // Reference to the NightManager to access current night

    private int currentLineIndex = 0; // Track the current line index
    private bool isInteracting = false; // Track if the player is interacting

    // Features learned flags
    public bool hasLearnedFeature1 = false;
    public bool hasLearnedFeature2 = false;
    public bool hasLearnedFeature3 = false;
    public bool hasLearnedFeature4 = false;

    // Lines for each night
    private string[][] nightLines = new string[][]
    {
        new string[]
        {
            "[E] Radio",
            "Good evening, survivors.",
            "We've got fresh reports about the aswang haunting our village.",
            "Right now, I'm with Aleng Tso...",
            "She claims to see them lurking in the shadows...",
            "<color=#FF9999>Oh God... </color>",
            "<color=#FF9999>Oh God... I saw an aswang right here!</color>",
            "<color=#FF9999>It had long limbs... and no heart.</color>",
            "<color=#FF9999>I'm scared...</color>"
        },
        new string[]
        {
            "[E] Radio",
            "As the nights roll on, the aswang sightings keep coming.",
            "People are vanishing without a trace, swallowed by the dark.",
            "Rumors say they disappear... into the shadows.",
            "Stay alert—don't trust strangers. Watch their eyes closely.",
            "Remember, the eyes of an aswang are pure darkness, no soul behind them."
        },
        new string[]
        {
            "[E] Radio",
            "We've gathered intel from local experts.",
            "Aswangs are said to wield dark magic; their true forms are elusive.",
            "You’ll spot an aswang by their unnaturally sharp teeth",
            "Be wary of celebrations; they might draw their attention.",
            "If you see someone wandering alone in the dark, tread carefully."
        },
        new string[]
        {
            "[E] Radio",
            "The elders advise staying on the path.",
            "Legends say the aswang stalks its prey, hiding behind a false smile.",
            "Listen closely to the sounds in the dark; don't ignore the unusual noises.",
            "Sometimes, the souls of the lost return to warn us.",
            "Keep your eyes peeled... there are watchers in the dark...",
            "So heads up and look out for each other—",
            "....",
            "It might be too late for me.",
            "...",
            "God save us all.",
            "*gunshot*",
            "..."
        },
    };

    private void Start()
    {
        radioText.gameObject.SetActive(false); // Initially hide the main text
        mumblingGrowlText.gameObject.SetActive(false); // Initially hide the mumbling growl text
    }

    private void Update()
    {
        // Handle interaction input only when interacting
        if (isInteracting && Input.GetKeyDown(KeyCode.E))
        {
            ProceedToNextLine(); // Show the next line
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
        currentLineIndex = 0; // Reset line index for the interaction
        radioText.gameObject.SetActive(true); // Show the main text
        ProceedToNextLine(); // Start displaying the first line
    }

    public void ProceedToNextLine()
    {
        int nightIndex = nightManager.currentNight - 1; // Get the correct night index (0-based)

        if (nightIndex >= 0 && nightIndex < nightLines.Length && currentLineIndex < nightLines[nightIndex].Length)
        {
            // Special condition for the mumbling growl on Night 4
            if (nightIndex == 3 && currentLineIndex == 6)
            {
                radioText.gameObject.SetActive(false); // Hide main text
                mumblingGrowlText.text = "<color=#FF9999>*mumbling growl*</color>";
                mumblingGrowlText.gameObject.SetActive(true);
            }
            else
            {
                mumblingGrowlText.gameObject.SetActive(false); // Hide growl text
                radioText.text = nightLines[nightIndex][currentLineIndex]; // Display current line
                radioText.gameObject.SetActive(true);
            }

            currentLineIndex++; // Move to the next line
        }
        else
        {
            LearnFeature(nightIndex); // Set feature as learned for the current night
            StopInteracting(); // Stop radio interaction when finished
        }
    }

    private void LearnFeature(int nightIndex)
    {
        // Update the feature flag based on the current night
        switch (nightIndex)
        {
            case 0:
                hasLearnedFeature1 = true; // Learned elongated limbs on Night 1
                Debug.Log("Learned feature 1.");
                break;
            case 1:
                hasLearnedFeature2 = true; // Learned about eyes on Night 2
                Debug.Log("Learned feature 2.");
                break;
            case 2:
                hasLearnedFeature3 = true; // Learned about sharp teeth on Night 3
                Debug.Log("Learned feature 3.");
                break;
            case 3:
                hasLearnedFeature4 = true; // Learned about false smiles on Night 4
                Debug.Log("Learned feature 4.");
                break;
            default:
                break;
        }
    }

    private void StopInteracting()
    {
        isInteracting = false;
        radioText.gameObject.SetActive(false); // Hide the main text
        mumblingGrowlText.gameObject.SetActive(false); // Hide the mumbling growl text
    }

    public bool HasLearnedFeature(int featureIndex)
    {
        // Return whether a specific feature has been learned
        switch (featureIndex)
        {
            case 1: return hasLearnedFeature1;
            case 2: return hasLearnedFeature2;
            case 3: return hasLearnedFeature3;
            case 4: return hasLearnedFeature4;
            default: return false;
        }
    }
}
