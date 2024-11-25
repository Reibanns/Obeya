using UnityEngine;
using System.Collections.Generic;

public class NightManager : MonoBehaviour
{
    public int currentNight = 1;
    private List<NPC> activeGuests = new List<NPC>(); // List of active NPC guests
    public List<GameObject> guestList = new List<GameObject>();


    // Reference to all the night scripts
    public Night1Script night1Script;
    public Night2Script night2Script;
    public Night3Script night3Script;
    public Night4Script night4Script;
    public Night5Script night5Script;

    private MonoBehaviour currentNightScript;

    // Positions where guests will go once accepted, assigned in the inspector
    public Transform[] guestPositionsInside;

    private int currentGuestPositionIndex = 0; // Keep track of the next guest position in the house

    void Start()
    {
        // Start with the first night
        currentNightScript = night1Script;
        night1Script.enabled = true; // Enable the first night script
    }

    public void AddGuest(GameObject guest)
    {
        guestList.Add(guest); // Add the guest to the global list
    }

    public void AssignGuestToPosition(GameObject guest, int guestsAccepted)
    {
        // Assign the accepted guest to the next available position inside the house
        if (currentGuestPositionIndex < guestPositionsInside.Length)
        {
            guest.transform.position = guestPositionsInside[currentGuestPositionIndex].position;
            currentGuestPositionIndex++; // Increment to the next position
        }
    }


    public void ProceedToNextNight()
    {
        // Disable the current night script
        currentNightScript.enabled = false;

        currentNight++; // Increment the night

        // Enable the next night's script based on current night
        switch (currentNight)
        {
            case 2:
                currentNightScript = night2Script;
                night2Script.enabled = true;
                break;
            case 3:
                currentNightScript = night3Script;
                night3Script.enabled = true;
                break;
            case 4:
                currentNightScript = night4Script;
                night4Script.enabled = true;
                break;
            case 5:
                currentNightScript = night5Script;
                night5Script.enabled = true;

                // Add the call to trigger the end of night 5
                ((Night5Script)currentNightScript).TriggerNightComplete(); // Use the public method to trigger OnNightComplete
                break;
            default:
                Debug.Log("All nights completed.");
                break;
        }

        // Reset guests for the new night
        FindObjectOfType<GuestManager>().PrepareGuestsForNewNight();
    }


    public void RemoveGuestFromList(NPC npc)
    {
        // Remove the NPC from the active guest list when they are killed
        if (activeGuests.Contains(npc))
        {
            activeGuests.Remove(npc);
        }
    }
}
