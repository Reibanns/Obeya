using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class GuestManager : MonoBehaviour
{
    public List<GameObject> guestPrefabs; // Prefabs for guests
    public Button acceptButton;
    public Button rejectButton;
    public GameObject guestPanel; // Panel for guests, to be set in the inspector
    public GameObject UIPanel;
    public TMP_Text dialogueText;
    public Button option1Button; // UI button for Option 1
    public Button option1_1Button; // UI button for Option 1.1
    public Button option2Button; // UI button for Option 2
    public Button option2_1Button; // UI button for Option 2.1

    private int currentGuestIndex = 0; // Track the current guest
    private int maxGuestsPerNight = 3; // Limit to 3 guests per night
    private int guestsProcessed = 0; // Track how many guests have been accepted or rejected

    public NightManager nightManager; // Reference to the NightManager
    private BedInteraction bedInteraction; // Reference to BedInteraction
    private GameObject currentGuest; // Track the current guest instance

    private int option1ResponseIndex = 0; // Index for Option 1 responses
    private int option1_1ResponseIndex = 0; // Index for Option 1.1 responses
    private int option2ResponseIndex = 0; // Index for Option 2 responses
    private int option2_1ResponseIndex = 0; // Index for Option 2.1 responses
    private int introIndex = 0;

    private bool isInDialogue = false; // Track if in dialogue
    private bool isInOptionDialogue = false; // Track if in option dialogue
    private NPCDialogue currentNPCDialogue; // Reference to the current NPC dialogue

    private bool hasOption2BeenSelected = false; // Track if Option 2 has been selected
    private bool hasOption2_1BeenSelected = false; // Track if Option 2.1 has been selected

        // New UI Image components
    public Image npcSpriteImage; // UI Image for displaying NPC sprite
    public Image backgroundImage; // UI Image for the background

    [Header("No More Guests Message")]
    public GameObject noMoreGuestsPanel; // Panel to display the no more guests message
    public TextMeshProUGUI noMoreGuestsText;
    public float noMoreGuestsMessageDuration = 3f;

    public event System.Action OnGuestAccepted;
    public event System.Action OnGuestRejected;
    public event System.Action OnNewNightStarted;



    private void Start()
    {
        // Get the BedInteraction reference
        bedInteraction = FindObjectOfType<BedInteraction>();

        // Setup UI buttons
        acceptButton.onClick.AddListener(AcceptGuest);
        rejectButton.onClick.AddListener(RejectGuest);
        option1Button.onClick.AddListener(Option1);
        option1_1Button.onClick.AddListener(Option1_1);
        option2Button.onClick.AddListener(Option2);
        option2_1Button.onClick.AddListener(Option2_1);

        PrepareGuestsForNewNight(); // Prepare guests at the start of the night
        
        // Initially hide the guest panel
        guestPanel.SetActive(false);
        UIPanel.SetActive(false);
        npcSpriteImage.gameObject.SetActive(false);
        backgroundImage.gameObject.SetActive(false);

        if (noMoreGuestsPanel != null)
        {
            noMoreGuestsPanel.SetActive(false);
        }
        if (noMoreGuestsText != null)
        {
            noMoreGuestsText.enabled = false;
        }
        OnNewNightStarted?.Invoke();
    }


    private void Update()
    {
        // Check for 'E' key press to progress dialogue
        if (Input.GetKeyDown(KeyCode.E) && isInDialogue)
        {
            if (!isInOptionDialogue) // If not in option dialogue, continue intro dialogue
            {
                ContinueIntroDialogue();
            }
            else
            {
                ContinueOptionDialogue();
            }
        }
    }

    private void ContinueOptionDialogue()
    {
        if (option1ResponseIndex > 0 && option1ResponseIndex <= currentNPCDialogue.option1Responses.Length)
        {
            DisplayOption1Response();
        }
        else if (option1_1ResponseIndex > 0 && option1_1ResponseIndex <= currentNPCDialogue.option1_1Responses.Length)
        {
            DisplayOption1_1Response();
        }
        else if (option2ResponseIndex > 0 && option2ResponseIndex <= currentNPCDialogue.option2Responses.Length)
        {
            DisplayOption2Response();
        }
        else if (option2_1ResponseIndex > 0 && option2_1ResponseIndex <= currentNPCDialogue.option2_1Responses.Length)
        {
            DisplayOption2_1Response();
        }
        else
        {
            FinishAllDialogue();
        }
    }

    public void InteractWithDoor()
    {
        if (guestsProcessed < maxGuestsPerNight && currentGuestIndex < guestPrefabs.Count)
        {
            SpawnNextGuest();
        }
        else
        {
            ShowNoMoreGuestsMessage();
            // Allow sleeping if all guests have been processed
            if (guestsProcessed >= maxGuestsPerNight)
            {
                bedInteraction.EnableSleeping();
            }
            // Trigger the OnGuestRejected event to re-enable door interaction
            OnGuestRejected?.Invoke();
        }
    }

    private void ShowNoMoreGuestsMessage()
    {
        if (noMoreGuestsPanel != null && noMoreGuestsText != null)
        {
            noMoreGuestsText.text = "No more guests can come in this night.";
            noMoreGuestsPanel.SetActive(true);
            noMoreGuestsText.enabled = true;
            
            // Use Invoke to hide the message after the specified duration
            Invoke(nameof(HideNoMoreGuestsMessage), noMoreGuestsMessageDuration);
        }
        else
        {
            Debug.LogWarning("No More Guests Panel or Text is not assigned in the inspector!");
        }
    }

    private void HideNoMoreGuestsMessage()
    {
        if (noMoreGuestsPanel != null && noMoreGuestsText != null)
        {
            noMoreGuestsText.enabled = false;
            noMoreGuestsPanel.SetActive(false);
        }
    }


    private void SpawnNextGuest()
    {
        // Ensure that guests are prepared to spawn
        if (currentGuestIndex < guestPrefabs.Count)
        {
            currentGuest = Instantiate(guestPrefabs[currentGuestIndex]);
            currentGuestIndex++; // Increment the index after instantiation

            hasOption2BeenSelected = false; // Reset selection state for new guest
            hasOption2_1BeenSelected = false; // Reset selection state for new guest

            // Get the NPCDialogue ScriptableObject from the guest prefab
            currentNPCDialogue = currentGuest.GetComponent<GuestData>().npcDialogue;

            // Activate the guest panel and UI buttons for accepting/rejecting guests
            UIPanel.SetActive(true);
            guestPanel.SetActive(true); // Re-enable the guest panel to show text
            npcSpriteImage.sprite = currentNPCDialogue.npcSprite; // Assign the NPC sprite
            npcSpriteImage.gameObject.SetActive(true); // Show the NPC sprite
            
            // Set the background image (set it from the inspector, or assign it directly)
            backgroundImage.gameObject.SetActive(true); // Show the background image

            // Start displaying introductory dialogue
            DisplayIntroductoryDialogue();
        }
        else
        {
            Debug.Log("All guests for this night have already been processed.");
        }
    }


    private void DisplayIntroductoryDialogue()
    {
        // Reset the dialogue index and clear any previous dialogue
        introIndex = 0; // Reset introductory dialogue index
        dialogueText.text = ""; // Clear dialogue text
        isInDialogue = true; // Set in dialogue to true
        isInOptionDialogue = false; // Ensure not in option dialogue
        UIPanel.SetActive(false);

        // Start displaying the first introductory line
        ContinueIntroDialogue();
    }

    private void ContinueIntroDialogue()
    {
        if (introIndex < currentNPCDialogue.introductoryDialogue.Count)
        {
            dialogueText.text = currentNPCDialogue.introductoryDialogue[introIndex];
            introIndex++;
        }
        else
        {
            // After displaying all introductory lines, show initial options
            ShowInitialOptions();
            isInDialogue = false; // End introductory dialogue
        }
    }

    private void ShowInitialOptions()
    {
        option1Button.gameObject.SetActive(true);
        option1_1Button.gameObject.SetActive(true);
        UIPanel.SetActive(true);
    }

    public void Option1()
    {
        option1ResponseIndex = 0;
        isInDialogue = true;
        isInOptionDialogue = true;
        UIPanel.SetActive(false);
        option1Button.gameObject.SetActive(false);
        option1_1Button.gameObject.SetActive(false);
        DisplayOption1Response();
    }

    public void Option1_1()
    {
        option1_1ResponseIndex = 0;
        isInDialogue = true;
        isInOptionDialogue = true;
        UIPanel.SetActive(false);
        option1Button.gameObject.SetActive(false);
        option1_1Button.gameObject.SetActive(false);
        DisplayOption1_1Response();
    }

    private void DisplayOption1Response()
    {
        if (option1ResponseIndex < currentNPCDialogue.option1Responses.Length)
        {
            dialogueText.text = currentNPCDialogue.option1Responses[option1ResponseIndex];
            option1ResponseIndex++;
        }
        else
        {
            ShowSecondOptions();
        }
    }

    private void DisplayOption1_1Response()
    {
        if (option1_1ResponseIndex < currentNPCDialogue.option1_1Responses.Length)
        {
            dialogueText.text = currentNPCDialogue.option1_1Responses[option1_1ResponseIndex];
            option1_1ResponseIndex++;
        }
        else
        {
            ShowSecondOptions();
        }
    }

    private void ShowSecondOptions()
    {
        isInDialogue = false;
        isInOptionDialogue = false;
        
        // Only show Option 2 buttons if they haven't been selected already
        if (!hasOption2BeenSelected && !hasOption2_1BeenSelected)
        {
            UIPanel.SetActive(true);
            option2Button.gameObject.SetActive(true);
            option2_1Button.gameObject.SetActive(true);
        }

        dialogueText.text = "";
    }

    public void Option2()
    {
        hasOption2BeenSelected = true; // Mark Option 2 as selected
        option2ResponseIndex = 0;
        isInDialogue = true;
        isInOptionDialogue = true;
        UIPanel.SetActive(false);
        option2Button.gameObject.SetActive(false);
        option2_1Button.gameObject.SetActive(false);
        DisplayOption2Response();
    }

    public void Option2_1()
    {
        hasOption2_1BeenSelected = true; // Mark Option 2.1 as selected
        option2_1ResponseIndex = 0;
        isInDialogue = true;
        isInOptionDialogue = true;
        option2Button.gameObject.SetActive(false);
        option2_1Button.gameObject.SetActive(false);
        UIPanel.SetActive(false);
        DisplayOption2_1Response();
    }

    private void DisplayOption2Response()
    {
        if (option2ResponseIndex < currentNPCDialogue.option2Responses.Length)
        {
            dialogueText.text = currentNPCDialogue.option2Responses[option2ResponseIndex];
            option2ResponseIndex++;
            acceptButton.gameObject.SetActive(true);
            rejectButton.gameObject.SetActive(true);
            UIPanel.SetActive(true);
            // Additional logic for Option 2 dialogue
        }
        else
        {
            FinishAllDialogue();
        }
    }

    private void DisplayOption2_1Response()
    {
        if (option2_1ResponseIndex < currentNPCDialogue.option2_1Responses.Length)
        {
            dialogueText.text = currentNPCDialogue.option2_1Responses[option2_1ResponseIndex];
            option2_1ResponseIndex++;
            acceptButton.gameObject.SetActive(true);
            rejectButton.gameObject.SetActive(true);
            UIPanel.SetActive(true);
            // Additional logic for Option 2.1 dialogue
        }
        else
        {
            FinishAllDialogue();
        }
    }

    private void FinishAllDialogue()
    {
        // Allow the player to accept or reject the guest
        acceptButton.gameObject.SetActive(true);
        rejectButton.gameObject.SetActive(true);
        UIPanel.SetActive(true);
    }

    private void AcceptGuest()
    {
        Debug.Log("Guest Accepted");
        nightManager.AddGuest(currentGuest);
        nightManager.AssignGuestToPosition(currentGuest, guestsProcessed);
        guestsProcessed++;

        guestPanel.SetActive(false);
        UIPanel.SetActive(false);
        acceptButton.gameObject.SetActive(false);
        rejectButton.gameObject.SetActive(false);
        npcSpriteImage.gameObject.SetActive(false); // Hide the NPC sprite
        backgroundImage.gameObject.SetActive(false); // Hide the background image

        if (guestsProcessed >= maxGuestsPerNight)
        {
            bedInteraction.EnableSleeping(); // Allow sleeping if all guests have been processed
        }
        OnGuestAccepted?.Invoke();
    }

    private void RejectGuest()
    {
        Debug.Log("Guest Rejected");
        Destroy(currentGuest); // Destroy the guest GameObject
        guestsProcessed++;
        guestPanel.SetActive(false);
        UIPanel.SetActive(false);
        acceptButton.gameObject.SetActive(false);
        rejectButton.gameObject.SetActive(false);
        npcSpriteImage.gameObject.SetActive(false); // Hide the NPC sprite
        backgroundImage.gameObject.SetActive(false); // Hide the background image

        if (guestsProcessed >= maxGuestsPerNight)
        {
            bedInteraction.EnableSleeping(); // Allow sleeping if all guests have been processed
        }
        OnGuestAccepted?.Invoke();
    }

    private void CleanUpAfterDialogue()
    {
        guestPanel.SetActive(false);
        UIPanel.SetActive(false);
        npcSpriteImage.gameObject.SetActive(false); // Hide the NPC sprite
        backgroundImage.gameObject.SetActive(false); // Hide the background image
        currentGuest = null; // Reset current guest
    }


    public void PrepareGuestsForNewNight()
    {
        guestsProcessed = 0;
        currentGuestIndex = 0;
        OnNewNightStarted?.Invoke();
    }
}