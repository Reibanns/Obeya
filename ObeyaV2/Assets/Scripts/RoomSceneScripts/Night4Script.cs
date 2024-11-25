using UnityEngine;

public class Night4Script : MonoBehaviour
{
    public NightManager nightManager; // Reference to the NightManager
    public int maxGuestsForTheNight = 3; // Number of guests for this night
    private int currentGuestCount = 0;

    void OnGuestInteraction(GameObject guest)
    {
        // This method should be called when a guest is interacted with (accepted)
        currentGuestCount++;
        nightManager.AddGuest(guest); // Add guest to the global list

        if (currentGuestCount >= maxGuestsForTheNight)
        {
            Debug.Log("Night 4 complete. Go to bed to proceed to the next night.");
            // Here you can activate bed interaction
        }
    }
}
